using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using System.Reflection;
using System.Configuration;

namespace Persistence
{
    public static class Database
    {
        internal static string _ConnectionString = "";
        private static string _SpPrefix = "";
        public static Exception InitializationError = null;

        static Database()
        {
            try
			{
                _ConnectionString = ConfigurationManager.ConnectionStrings["Persistence"].ConnectionString;
                _SpPrefix = ConfigurationManager.AppSettings["PersistenceSpPrefix"];
            }
            catch (Exception e) { InitializationError = e; }
        }

        public static string SpPrefix { get { return _SpPrefix; } }
        //public static bool AllowsDirectSql { get { return SqlServer._AllowsDirectSql; } }

        public static void SetConnectionString(string namedConnString)
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings[namedConnString].ConnectionString;
        }

        internal static DbConnection GetConnection()
        {
            return SqlServer.GetConnection(Database._ConnectionString);
        }

        public static Connection Open()
        {
            return new Connection(false);
        }

        public static Connection OpenWithTransaction()
        {
            return new Connection(true);
        }

        public static void MapToTable(object o, DataTable table)
		{
			DataRow row = table.NewRow();
			foreach (DataColumn col in table.Columns)
			{
				object value = Class.GetPropertyValueByName(o, col.ColumnName);

				if (Value.IsNull(value))
					row[col.ColumnName] = DBNull.Value;
				else
					row[col.ColumnName] = value;
			}
			table.Rows.Add(row);
		}

		public static void MapToInstance(IDataReader reader, object o)
		{
			for (int i = 0; i < reader.FieldCount; i++)
			{
                string name = reader.GetName(i);

                FieldInfo field = o.GetType().GetField("_" + name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)  //if field exists, map directly to the field (bypassing property set)
                    if (reader.IsDBNull(i))
                        if (reader.GetFieldType(i) == typeof(String))
                            field.SetValue(o, "");
                        else
                            field.SetValue(o, null);
                    else
                        field.SetValue(o, reader.GetValue(i));
                else
                    if (reader.IsDBNull(i))
                        if (reader.GetFieldType(i) == typeof(String))
                            Class.SetPropertyValueByName(o, name, "");
                        else
                            Class.SetPropertyValueByName(o, name, null);
                    else
                        Class.SetPropertyValueByName(o, name, reader.GetValue(i));
			}
		}

        public static DataTable ExecuteDataTable(string spName, object parameters, Connection cn)
        {
            return SqlServer.ExecuteDataTable(spName, parameters, cn);
        }

        public static DataTable ExecuteDataTable(string spName, object parameters)
        {
            using (Connection cn = Database.Open())
            {
                DataTable table = SqlServer.ExecuteDataTable(spName, parameters, cn);
                cn.Complete();
                return table;
            }
        }

		public static void MapParameterValues(object o, DbCommand cmd)
		{
			foreach (DbParameter parm in cmd.Parameters)
			{
				string name = parm.ParameterName;
				if (name.StartsWith("@"))
					name = name.Substring(1);

				object value = Class.GetPropertyValueByName(o, name);

				if (Value.IsNull(value))
				{
					if (!parm.IsNullable)
						throw new ApplicationException(String.Format("Property value cannot be null on {0}: {1}.", o.GetType().Name, name));

					parm.Value = DBNull.Value;
				} else
					parm.Value = value;
			}
		}

        public static void Load(object o)
        {
            Database.Load(o, "");
        }

        public static void Load(object o, string action)
        {
            using (Connection cn = Database.Open())
            {
                Database.Load(o, action, cn);
                cn.Complete();
            }
        }

        public static void Load(object o, Connection cn)
        {
            Database.Load(o, "", cn);
        }

        public static void Load(object o, string action, Connection cn)
        {
            //if (Class.MethodExists(o, "Database_BeforeLoad"))
            //    Class.CallMethodByName(o, "Database_BeforeLoad");

            SqlServer.Load(o, action, cn);

            //if (Class.MethodExists(o, "Database_AfterLoad"))
            //    Class.CallMethodByName(o, "Database_AfterLoad");
        }

        public static int LoadAll<T>(PersistentList<T> list, string action, Connection cn)
        {
            return SqlServer.LoadAll(list, action, list, cn);
        }

		public static int LoadAll<T>(PersistentList<T> list, string action)
        {
            using (Connection cn = Database.Open())
            {
                int returned = Database.LoadAll(list, action, cn);
                cn.Complete();
                return returned;
            }
		}

        public static int LoadAll<T>(PersistentList<T> list, string action, object parameters, Connection cn)
        {
            return SqlServer.LoadAll(list, action, parameters, cn);
        }

        public static int Store(object o)
        {
            return Database.Store(o, "");
        }

        public static int Store(object o, string action)
        {
            using (Connection cn = Database.Open())
            {
                int affected = Database.Store(o, action, cn);
                cn.Complete();
                return affected;
            }
        }

        public static int Store(object o, Connection cn)
        {
            return Database.Store(o, "", cn);
        }

        public static int Store(object o, string action, Connection cn)
		{
            int affected = 0;
            //if (Class.MethodExists(o, "Database_BeforeStore"))
            //    Class.CallMethodByName(o, "Database_BeforeStore");

            affected = SqlServer.Store(o, action, cn);

            //if (Class.MethodExists(o, "Database_AfterStore"))
            //    Class.CallMethodByName(o, "Database_AfterStore");

            return affected;
            //try
            //{

            //}
            //catch (Exception)
            //{
            //    //if Store fails, re-load the data from the database so the memory one isn't out of sync.
            //    if (o.GetPersistenceInfo().PrimaryKeyValue > 0)
            //        Database.Load(o);

            //    throw;
            //}
		}

        public static bool Delete(object o)
        {
            return Database.Delete(o, "");
        }

        public static bool Delete(object o, string action)
        {
            using (Connection cn = Database.Open())
            {
                bool deleted = Database.Delete(o, action, cn);
                cn.Complete();
                return deleted;
            }
        }

        public static bool Delete(object o, Connection cn)
        {
            return Database.Delete(o, "", cn);
        }

		public static bool Delete(object o, string action, Connection cn)
		{
            //if (Class.MethodExists(o, "Database_BeforeDelete"))
            //    Class.CallMethodByName(o, "Database_BeforeDelete");

            bool deleted = SqlServer.Delete(o, action, cn);

            //if (Class.MethodExists(o, "Database_AfterDelete"))
            //    Class.CallMethodByName(o, "Database_AfterDelete");
            
			return deleted;
		}

		public static DataTable GetTableStructure(string tableName)
		{
			return SqlServer.GetTableStructure(tableName, Database._ConnectionString);
		}

		public static void StoreBulk<T>(PersistentList<T> list)
		{
			DataTable table = Database.GetTableStructure(list.GetItemDataSource);
 			list.ToDataTable(table);
			SqlServer.StoreBulk(list.GetItemDataSource, table, Database._ConnectionString);
		}

        internal static string DisplayParameterValues(DbParameterCollection parms)
        {
            string s = "";
            foreach (DbParameter p in parms)
                s += String.Format("{0} = {1} and ", p.ParameterName, p.Value);
            return Value.TruncateLast(s, 5);
        }
	}
}
