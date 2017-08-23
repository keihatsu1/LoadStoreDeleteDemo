using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using System.Collections;
using System.Configuration;

/*
 * todo:
 * throw error from sys_ParametersForSP if SP not found
 * CopyStructure to DataTable from SP resultset
*/

namespace Persistence
{
	internal class SqlServer
    {
        //internal static readonly bool _AllowsDirectSql = false;
        //public static readonly Exception InitializationError = null;

        //static SqlServer()
        //{
        //    try
        //    {
        //        using (SqlConnection cn = (SqlConnection)Database.GetConnection())
        //        using (SqlCommand cmd = new SqlCommand("sys_ExtendedProperty"))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@PropertyName", "AllowsDirectSql");
        //            cmd.Connection = cn;
        //            cn.Open();
        //            object value = cmd.ExecuteScalar();
        //            int i = 0;
        //            if (value != null && int.TryParse(Convert.ToString(value), out i) && i == 1)
        //                _AllowsDirectSql = true;
        //        }
        //    }
        //    catch (Exception ex) { InitializationError = ex; }
        //}

		private static Hashtable tables = new Hashtable();

		public static DataTable GetTableStructure(string tableName, string connString)
        {
			if (tables.ContainsKey(tableName))
				return ((DataTable)tables[tableName]).Clone();

			DataTable table = new DataTable();
			table.TableName = tableName;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sys_StructureForTable", conn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@TableName", tableName);
				
                using (SqlDataReader reader = cmd.ExecuteReader())
				while (reader.Read())
                {
                    DataColumn col = new DataColumn();
                    col.ColumnName = reader["Name"].ToString();
                    col.DataType = TypeMapper.ToNetType(reader["Type"].ToString());
                    col.AllowDBNull = Convert.ToBoolean(reader["IsNullable"]);
					col.AutoIncrement = Convert.ToBoolean(reader["AutoIncrement"]);
                    table.Columns.Add(col);
                }
			
				if (table.Columns.Count < 1)
					throw new ApplicationException(String.Format("Unable to retrieve schema information on table {0} in {1}.  {2} '{3}' returned 0 rows.", cmd.Parameters[0].Value, cmd.Connection.Database, cmd.CommandText, cmd.Parameters[0].Value));
			}

			tables.Add(tableName, table.Clone());

            return table;
        }

        public static void StoreBulk(string tableName, DataTable table, string connString)
        {
            using (SqlConnection cn = new SqlConnection(connString))
            using (SqlCommand createTemp = new SqlCommand(String.Format("select top 0 * into #{0} from {0}", tableName), cn))
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
            using (SqlCommand merge = new SqlCommand(Database.SpPrefix + tableName + "_Merge", cn))
            {
                cn.Open();

                //create temporary table
                createTemp.ExecuteNonQuery();

                //bulk copy
                bulkCopy.DestinationTableName = "#" + tableName;
                bulkCopy.WriteToServer(table);

                //merge temp table into production
                merge.ExecuteNonQuery();
            }
        }

        public static int LoadAll<T>(PersistentList<T> list, string action, object parameters, Connection cn)
        {
            string spName = Database.SpPrefix + list.GetItemDataSource + "_List" + action;

            using (SqlCommand cmd = GetCommand(spName, Database._ConnectionString))
            //using (SqlCommand cmd = GetCommand(spName, cn.DbConnection.ConnectionString))
            {
                Database.MapParameterValues(parameters, cmd);

                int pageNumber = list.PageNumber;
                int pageSize = list.PageSize;

                cmd.Connection = (SqlConnection)cn.DbConnection;
                cmd.Transaction = (SqlTransaction)cn.DbTransaction;
                int rows = 0;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    //pagination skip to appropriate row number
                    int endRow = pageNumber * pageSize;
                    while (rows < (endRow - pageSize) && reader.Read())
                        rows++;

                    while (reader.Read())
                    {
                        T newItem = list.CreateNewItem();

                        //if (Class.MethodExists(newItem, "Database_BeforeLoad"))
                        //    Class.CallMethodByName(newItem, "Database_BeforeLoad");

                        Database.MapToInstance(reader, newItem);
                        list.Add(newItem);
                        rows++;

                        //if (Class.MethodExists(newItem, "Database_AfterLoad"))
                        //    Class.CallMethodByName(newItem, "Database_AfterLoad");

                        //end of page, read to the end to get total count
                        if (rows == pageSize * pageNumber)
                            while (reader.Read())
                                rows++;
                    }
                }
				list._totalRows = rows;
                return rows;
            }
        }

        public static void Load(object o, Connection trans)
        {
            SqlServer.Load(o, "", trans);
        }

		public static void Load(object o, string action, Connection trans)
        {
            PersistableAttribute info = Class.GetPersistenceInfo(o);

            //if (info.UseDirectSql)
            //{
            //    LoadViaDirectSql(o, trans);
            //    return;
            //}

            string spName = Database.SpPrefix + info.DataSource + "_Load" + action;

            SqlCommand cmd = GetCommand(spName, Database._ConnectionString);
            //SqlCommand cmd = GetCommand(spName, trans.DbConnection.ConnectionString);
			Database.MapParameterValues(o, cmd);

            cmd.Connection = (SqlConnection)trans.DbConnection;
            cmd.Transaction = (SqlTransaction)trans.DbTransaction;
            using (SqlDataReader reader = cmd.ExecuteReader())
                if (reader.Read())
				    Database.MapToInstance(reader, o);
                else
                    throw new ApplicationException(String.Format("No record found in {0} where {1}.", info.DataSource, Database.DisplayParameterValues(cmd.Parameters)));
		}

        public static DataTable ExecuteDataTable(string spName, object parameters, Connection trans)
        {
            using (SqlCommand cmd = SqlServer.GetCommand(spName, Database._ConnectionString))
            //using (SqlCommand cmd = SqlServer.GetCommand(spName, trans.DbConnection.ConnectionString))
            {
                if (parameters == null)
                {
                    //no parameters
                }
                else if (parameters.GetType() == typeof(List<SqlParameter>))
                {
                    cmd.Parameters.Clear();
                    foreach (SqlParameter p in (List<SqlParameter>)parameters)
                        cmd.Parameters.Add(p);
                }
                else
                    Database.MapParameterValues(parameters, cmd);

                cmd.Connection = (SqlConnection)trans.DbConnection;
                cmd.Transaction = (SqlTransaction)trans.DbTransaction;

                DataTable table = new DataTable();
                using (SqlDataReader reader = cmd.ExecuteReader())
                    table.Load(reader);
                return table;
            }
        }

        //public static void LoadViaDirectSql(object o, Connection trans)
        //{
        //    if (!Database.AllowsDirectSql)
        //        throw new ApplicationException("The database does not allow direct sql.  To enable, set AllowsDirectSql = 1 in SqlServer's extended properties (sys.extended_properties).");

        //    PersistableAttribute info = Class.GetPersistenceInfo(o);
        //    string sql = String.Format("select * from {0} where {1}={2}", info.DataSource, info.PrimaryKeyName, Class.GetPropertyValueByName(o, info.PrimaryKeyName));

        //    using (SqlCommand cmd = new SqlCommand(sql, (SqlConnection)trans.DbConnection, (SqlTransaction)trans.DbTransaction))
        //    using (SqlDataReader reader = cmd.ExecuteReader())
        //        if (reader.Read())
        //            Database.MapToInstance(reader, o);
        //        else
        //            throw new ApplicationException(String.Format("{0} id {1} was not found.", info.DataSource, info.PrimaryKeyValue));
        //}

        //public static int StoreViaDirectSql(object o, Connection trans)
        //{
        //    if (!Database.AllowsDirectSql)
        //        throw new ApplicationException("The database does not allow direct sql.  To enable, set AllowsDirectSql = 1 in SqlServer's extended properties (sys.extended_properties).");

        //    PersistableAttribute info = Class.GetPersistenceInfo(o);

        //    SqlCommand cmd;
        //    int rowsAffected = 0;

        //    if (info.PrimaryKeyValue > 0)
        //        cmd = SqlServer.GetUpdateCommand(o, Database._ConnectionString); // trans.DbConnection.ConnectionString);
        //    else
        //        cmd = SqlServer.GetInsertCommand(o, Database._ConnectionString); // trans.DbConnection.ConnectionString);

        //    cmd.Connection = (SqlConnection)trans.DbConnection;
        //    cmd.Transaction = (SqlTransaction)trans.DbTransaction;

        //    if (info.PrimaryKeyValue > 0)
        //    {
        //        rowsAffected = cmd.ExecuteNonQuery();
        //        if (rowsAffected == 0)
        //            throw new ApplicationException(String.Format("No rows affected while updating {0} ({1}={2}).", info.DataSource, info.PrimaryKeyName, info.PrimaryKeyValue));
        //    }
        //    else
        //    {
        //        object id = cmd.ExecuteScalar();
        //        if (id == null || Convert.ToInt32(id) == 0)
        //            throw new ApplicationException(String.Format("SqlServer did not return a primary key after inserting {0}.", info.DataSource));

        //        info.PrimaryKeyValue = Convert.ToInt32(id);
        //    }
        //    cmd.Dispose();
        //    return rowsAffected;
        //}

        //public static bool DeleteViaDirectSql(object o, Connection cn)
        //{
        //    if (!Database.AllowsDirectSql)
        //        throw new ApplicationException("The database does not allow direct sql.  To enable, set AllowsDirectSql = 1 in SqlServer's extended properties (sys.extended_properties).");

        //    PersistableAttribute info = Class.GetPersistenceInfo(o);
        //    string sql = String.Format("delete from {0} where {1}={2}", info.DataSource, info.PrimaryKeyName, info.PrimaryKeyValue);
        //    using (SqlCommand cmd = new SqlCommand(sql, (SqlConnection)cn.DbConnection, (SqlTransaction)cn.DbTransaction))
        //        return (cmd.ExecuteNonQuery() > 0);
        //}

        public static SqlConnection GetConnection(string connString)
        {
            return new SqlConnection(connString);
        }

        public static int Store(object o, string action, Connection cn)
        {
            PersistableAttribute pi = Class.GetPersistenceInfo(o);
            //if (pi.UseDirectSql)
            //    return StoreViaDirectSql(o, cn);

            string spName = Database.SpPrefix + pi.DataSource + "_Store" + action;

            using (SqlCommand cmd = GetCommand(spName, Database._ConnectionString)) // cn.DbConnection.ConnectionString))
            {
                Database.MapParameterValues(o, cmd);

                int rowsAffected = 0;
                cmd.Connection = (SqlConnection)cn.DbConnection;
                cmd.Transaction = (SqlTransaction)cn.DbTransaction;
                rowsAffected = cmd.ExecuteNonQuery();

                //set output parameter on object (eg. identity key)
                foreach (SqlParameter p in cmd.Parameters)
                {
                    if (p.ParameterName.ToLower() == "@" + pi.PrimaryKeyName.ToLower()
                            && p.Direction == ParameterDirection.Input)
                        throw new ApplicationException("Primary key must be set as an output parameter in the stored procedure: " + spName);
                    if (p.Direction != ParameterDirection.Input)
                    {
                        string name = p.ParameterName;
                        if (name.StartsWith("@"))
                            name = name.Substring(1);

						if (o.FieldExists("_" + name))
							o.SetFieldValueByName("_" + name, p.Value);
						else
							o.SetPropertyValueByName(name, p.Value);
                    }
                }
                return rowsAffected;
            }
        }

        public static bool Delete(object o, string action, Connection trans)
        {
            PersistableAttribute info = Class.GetPersistenceInfo(o);
            //if (info.UseDirectSql)
            //    return DeleteViaDirectSql(o, trans);

			string spName = Database.SpPrefix + info.DataSource + "_Delete" + action;

            using (SqlCommand cmd = GetCommand(spName, Database._ConnectionString)) // trans.DbConnection.ConnectionString))
            {
                Database.MapParameterValues(o, cmd);

                cmd.Connection = (SqlConnection)trans.DbConnection;
                cmd.Transaction = (SqlTransaction)trans.DbTransaction;
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //public static int ExecuteNonQuery(string sql, string connString)
        //{
        //    if (!Database.AllowsDirectSql)
        //        throw new ApplicationException("The database does not allow direct sql.  To enable, set AllowsDirectSql = 1 in SqlServer's extended properties (sys.extended_properties).");

        //    using (SqlConnection cn = new SqlConnection(connString))
        //    {
        //        SqlCommand cmd = new SqlCommand(sql);
        //        cmd.Connection = cn;
        //        cn.Open();
        //        return cmd.ExecuteNonQuery();
        //    }
        //}

        //public static SqlDataReader ExecuteReader(string sql, string connString)
        //{
        //    if (!Database.AllowsDirectSql)
        //        throw new ApplicationException("The database does not allow direct sql.  To enable, set AllowsDirectSql = 1 in SqlServer's extended properties (sys.extended_properties).");

        //    using (SqlConnection cn = new SqlConnection(connString))
        //    {
        //        SqlCommand cmd = new SqlCommand(sql);
        //        cmd.Connection = cn;
        //        cn.Open();
        //        return cmd.ExecuteReader();
        //    }
        //}

        //public static object ExecuteScalar(string sql, string connString)
        //{
        //    if (!Database.AllowsDirectSql)
        //        throw new ApplicationException("The database does not allow direct sql.  To enable, set AllowsDirectSql = 1 in SqlServer's extended properties (sys.extended_properties).");

        //    using (SqlConnection cn = new SqlConnection(connString))
        //    {
        //        SqlCommand cmd = new SqlCommand(sql);
        //        cmd.Connection = cn;
        //        cn.Open();
        //        return cmd.ExecuteScalar();
        //    }
        //}

        public static void ClearSqlCommandHashtable()
        {
            lock (locking)
                cmds = new Hashtable();
        }

		private static Hashtable cmds = new Hashtable();
        private static object locking = new object();
        public static SqlCommand GetCommand(string spName, string connString)
        {
            lock (locking)
            {
                if (! cmds.ContainsKey(spName))
                {
                    SqlCommand cmd = new SqlCommand(spName);
                    cmd.CommandType = CommandType.StoredProcedure;
                    CreateParameters(cmd, spName, connString);
                    cmds.Add(spName, cmd);
                }
                return ((SqlCommand)cmds[spName]).Clone();
            }
        }

        public static void CreateParameters(SqlCommand cmd, string spName, string connString)
        {
			using (SqlConnection cn = new SqlConnection(connString))
            using (SqlCommand getParms = new SqlCommand("sys_ParametersForSP", cn))
            {
                getParms.CommandType = CommandType.StoredProcedure;
                getParms.Parameters.AddWithValue("@SPName", spName);

				cn.Open();

                using (SqlDataReader reader = getParms.ExecuteReader())
				while (reader.Read())
                {
                    SqlParameter parm = new SqlParameter();
                    parm.ParameterName = "@" + Convert.ToString(reader["Name"]);
                    parm.IsNullable = Convert.ToBoolean(reader["IsNullable"]);
                    parm.Precision = Convert.ToByte(reader["XPrec"]);
                    parm.Size = Convert.ToInt32(reader["Length"]);
                    parm.SqlDbType = TypeMapper.ToSqlDbType(Convert.ToString(reader["Type"]));
                    if (Convert.ToBoolean(reader["IsOutParam"]))
						parm.Direction = ParameterDirection.InputOutput;
					else
						parm.Direction = ParameterDirection.Input;

					cmd.Parameters.Add(parm);
				}
			}
		}

        //public static SqlCommand GetUpdateCommand(object o, string connString)
        //{
        //    string sql = "update {0} set {1} where {2}={3}";
        //    string sets = "";

        //    PersistableAttribute pi = Class.GetPersistenceInfo(o);

        //    SqlCommand cmd = new SqlCommand();

        //    DataTable table = SqlServer.GetTableStructure(pi.DataSource, connString);
        //    foreach (DataColumn col in table.Columns)
        //    {
        //        if (col.AutoIncrement)
        //            continue;
        //        if (col.ColumnName == pi.PrimaryKeyName)
        //            continue;

        //        if (!string.IsNullOrEmpty(sets))
        //            sets += ", ";

        //        sets += col.ColumnName + "=@" + col.ColumnName;
			
        //        object value = Class.GetPropertyValueByName(o, col.ColumnName);

        //        if (Value.IsNull(value))
        //            value = DBNull.Value;

        //        cmd.Parameters.Add("@" + col.ColumnName, TypeMapper.ToSqlDbType(col.DataType), col.MaxLength).Value = value;
        //    }

        //    if (String.IsNullOrEmpty(sets))
        //        throw new ApplicationException("Table not found in database: " + Class.GetPersistenceInfo(o).DataSource);

        //    cmd.CommandText = String.Format(sql, pi.DataSource, sets, pi.PrimaryKeyName, pi.PrimaryKeyValue);

        //    return cmd;
        //}

        //public static SqlCommand GetInsertCommand(object o, string connString)
        //{
        //    string sql = "insert into {0} ({1}) values ({2}); select scope_identity();";
        //    string fields = "";
        //    string parms = "";

        //    PersistableAttribute pi = Class.GetPersistenceInfo(o);

        //    SqlCommand cmd = new SqlCommand();

        //    DataTable table = SqlServer.GetTableStructure(pi.DataSource, connString);
        //    foreach (DataColumn col in table.Columns)
        //    {
        //        if (col.AutoIncrement)
        //            continue;
        //        if (col.ColumnName == pi.PrimaryKeyName)
        //            continue;

        //        if (!string.IsNullOrEmpty(fields))
        //            fields += ", ";
        //        if (!string.IsNullOrEmpty(parms))
        //            parms += ", ";

        //        fields += col.ColumnName;
        //        parms += "@" + col.ColumnName;

        //        object value = Class.GetPropertyValueByName(o, col.ColumnName);

        //        if (Value.IsNull(value))
        //            value = DBNull.Value;

        //        cmd.Parameters.Add("@" + col.ColumnName, TypeMapper.ToSqlDbType(col.DataType), col.MaxLength).Value = value;
        //    }

        //    if (String.IsNullOrEmpty(fields))
        //        throw new ApplicationException("Table not found in database: " + pi.DataSource);

        //    cmd.CommandText = String.Format(sql, pi.DataSource, fields, parms);

        //    return cmd;
        //}
	}
}
