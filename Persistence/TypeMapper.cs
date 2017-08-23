using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace Persistence
{
	public sealed class TypeMapper
	{
		private struct TypeMap
		{
			public Type Type;
			public DbType DbType;
			public SqlDbType SqlDbType;
			public string TypeName;
			public bool IsDefault;

			public TypeMap(Type type, DbType dbType, SqlDbType sqlDbType, string typeName, bool isDefault)
			{
				this.Type = type;
				this.DbType = dbType;
				this.SqlDbType = sqlDbType;
				this.TypeName = typeName;
				this.IsDefault = isDefault;
			}

			public TypeMap(Type type, DbType dbType, SqlDbType sqlDbType, string typeName)
			{
				this.Type = type;
				this.DbType = dbType;
				this.SqlDbType = sqlDbType;
				this.TypeName = typeName;
				this.IsDefault = false;
			}
		}

		private static List<TypeMap> _list = new List<TypeMap>();
	
		#region Constructors
		private TypeMapper() { }

		static TypeMapper()
		{
			TypeMap typeMap;

			typeMap = new TypeMap(typeof(string), DbType.String, SqlDbType.VarChar, "string", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(string), DbType.String, SqlDbType.VarChar, "String");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(string), DbType.String, SqlDbType.VarChar, "varchar");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(string), DbType.String, SqlDbType.NVarChar, "nvarchar");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(string), DbType.AnsiStringFixedLength, SqlDbType.Char, "char");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(string), DbType.StringFixedLength, SqlDbType.NChar, "nchar");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(string), DbType.String, SqlDbType.Text, "text");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(string), DbType.String, SqlDbType.NText, "ntext");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(string), DbType.Xml, SqlDbType.Xml, "xml");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(bool), DbType.Boolean, SqlDbType.Bit, "bool", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(bool), DbType.Boolean, SqlDbType.Bit, "bit", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(byte), DbType.Byte, SqlDbType.TinyInt, "byte", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(byte), DbType.Byte, SqlDbType.TinyInt, "Byte");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(byte), DbType.Byte, SqlDbType.TinyInt, "tinyint");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(byte[]), DbType.Binary, SqlDbType.VarBinary, "Byte[]", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(byte[]), DbType.Binary, SqlDbType.VarBinary, "byte[]");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(byte[]), DbType.Binary, SqlDbType.VarBinary, "varbinary");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(byte[]), DbType.Binary, SqlDbType.Image, "image");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(byte[]), DbType.Binary, SqlDbType.Binary, "Byte[]");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(byte[]), DbType.Binary, SqlDbType.Binary, "binary");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(byte[]), DbType.Binary, SqlDbType.Timestamp, "timestamp");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime, "DateTime", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime, "datetime", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(DateTime), DbType.DateTime, SqlDbType.SmallDateTime, "smalldatetime");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(DateTime), DbType.Date, SqlDbType.Date, "DateTime");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(DateTime), DbType.Date, SqlDbType.Date, "date");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(DateTime), DbType.DateTime2, SqlDbType.DateTime2, "datetime2");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(DateTime), DbType.Time, SqlDbType.Time, "DateTime");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(DateTime), DbType.Time, SqlDbType.Time, "time");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Decimal), DbType.Currency, SqlDbType.Money, "Decimal", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Decimal), DbType.Currency, SqlDbType.Money, "money", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Decimal), DbType.Currency, SqlDbType.SmallMoney, "smallmoney");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Decimal), DbType.Currency, SqlDbType.Money, "decimal");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Decimal), DbType.Decimal, SqlDbType.Decimal, "Decimal");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Decimal), DbType.VarNumeric, SqlDbType.Decimal, "numeric");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(double), DbType.Double, SqlDbType.Float, "double", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(float), DbType.Double, SqlDbType.Float, "float", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(float), DbType.Double, SqlDbType.Real, "real", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier, "Guid", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier, "uniqueidentifier", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Int16), DbType.Int16, SqlDbType.SmallInt, "Int16", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Int16), DbType.Int16, SqlDbType.SmallInt, "smallint", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Int32), DbType.Int32, SqlDbType.Int, "int", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Int32), DbType.Int32, SqlDbType.Int, "Int32");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Int64), DbType.Int64, SqlDbType.BigInt, "Int64", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(Int64), DbType.Int64, SqlDbType.BigInt, "bigint");
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(object), DbType.Object, SqlDbType.Variant, "object", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(object), DbType.Object, SqlDbType.Variant, "variant", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(object), DbType.Object, SqlDbType.Variant, "sql_variant", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(DateTimeOffset), DbType.DateTimeOffset, SqlDbType.DateTimeOffset, "DateTimeOffset", true);
			_list.Add(typeMap);

			typeMap = new TypeMap(typeof(DateTimeOffset), DbType.DateTimeOffset, SqlDbType.DateTimeOffset, "datetimeoffset", true);
			_list.Add(typeMap);
		}

		#endregion

		#region Methods
		
		public static Type ToNetType(DbType dbType)
		{
			TypeMap map = Find(dbType);
			return map.Type;
		}

		public static Type ToNetType(SqlDbType sqlDbType)
		{
			TypeMap map = Find(sqlDbType);
			return map.Type;
		}

		public static Type ToNetType(string typeName)
		{
			TypeMap map = Find(typeName);
			return map.Type;
		}

		public static DbType ToDbType(Type type)
		{
			TypeMap map = Find(type);
			return map.DbType;
		}

		public static DbType ToDbType(SqlDbType sqlDbType)
		{
			TypeMap map = Find(sqlDbType);
			return map.DbType;
		}

		public static DbType ToDbType(string typeName)
		{
			TypeMap map = Find(typeName);
			return map.DbType;
		}

		public static SqlDbType ToSqlDbType(Type type)
		{
			TypeMap map = Find(type);
			return map.SqlDbType;
		}

		public static SqlDbType ToSqlDbType(DbType dbType)
		{
			TypeMap map = Find(dbType);
			return map.SqlDbType;
		}

		public static SqlDbType ToSqlDbType(string typeName)
		{
			TypeMap map = Find(typeName);
			return map.SqlDbType;
		}

		#endregion

		#region FindMethods
		private static TypeMap Find(Type type)
		{
			//look for default
			foreach (TypeMap map in _list)
				if (map.Type == type && map.IsDefault)
					return map;

			//look for non-default
			foreach (TypeMap map in _list)
				if (map.Type == type)
					return map;

			throw new ApplicationException("Unsupported Type: " + type.Name);
		}

		private static TypeMap Find(DbType dbType)
		{
			foreach (TypeMap map in _list)
				if (map.DbType == dbType && map.IsDefault)
					return map;

			foreach (TypeMap map in _list)
				if (map.DbType == dbType)
					return map;

			throw new ApplicationException("Unsupported DbType: " + dbType.ToString());
		}

		private static TypeMap Find(SqlDbType sqlDbType)
		{
			foreach (TypeMap map in _list)
				if (map.SqlDbType == sqlDbType && map.IsDefault)
					return map;

			foreach (TypeMap map in _list)
				if (map.SqlDbType == sqlDbType)
					return map;

			throw new ApplicationException("Unsupported SqlDbType: " + sqlDbType.ToString());
		}

		private static TypeMap Find(string typeName)
		{
			foreach (TypeMap map in _list)
				if (map.TypeName == typeName && map.IsDefault)
					return map;

			foreach (TypeMap map in _list)
				if (map.TypeName == typeName)
					return map;

			throw new ApplicationException("Unsupported TypeName: " + typeName);
		}

		#endregion
	}
}
