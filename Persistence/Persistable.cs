using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
	public class PersistableAttribute : Attribute
	{
		public string DataSource;
		public string PrimaryKeyName;
        //public bool UseDirectSql = false;
		public object Instance = null;

		/// <summary></summary>
		/// <param name="dataSource">Name of table or stored procedure (eg. Customer).  Stored procedure naming convention: load_Customer, store_Customer, delete_Customer, list_CustomerForXorY</param>
		/// <param name="primaryKeyName">Must be a property of type int and maps to a stored procedure parameter of the same name or the identity column on the table.</param>
        ///// <param name="useDirectSql">If true, allows dynamic Sql generation for singleton persistence.  If false, load, store, delete stored procedures will be required.</param>
        //public PersistableAttribute(string dataSource, string primaryKeyName, bool useDirectSql)
        public PersistableAttribute(string dataSource, string primaryKeyName)
        {
			DataSource = dataSource;
			PrimaryKeyName = primaryKeyName;
            //UseDirectSql = useDirectSql;
		}

		public PersistableAttribute() { }

		public int PrimaryKeyValue
		{
			get { return Class.GetInt32PropertyValueByName(Instance, PrimaryKeyName); }
			set { Class.SetPropertyValueByName(Instance, PrimaryKeyName, value); }
		}
	}
}
