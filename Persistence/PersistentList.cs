using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace Persistence
{
	public abstract class PersistentList<T> : IEnumerable<T>, System.Collections.ICollection
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }

        public int PageIndex
        {
            get { return this.PageNumber - 1; }
            set { this.PageNumber = value + 1; }
        }

		//thread-safe for adds/removes.  enumerations make a copy through GetList().
		private object _lock = new object();

		List<T> _list = new List<T>();
		public List<T> GetList()
		{
			lock (_lock)
				return _list.ToList();
		}

		public virtual T CreateNewItem()
        {
            try
            {
                return Activator.CreateInstance<T>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Unable to create new {0} in {1}: {2}", typeof(T).Name, this.GetType().Name, ex.Message), ex);
            }
        }
		public Type GetItemType() { return typeof(T); }
		public string GetItemDataSource { get { return Class.GetPersistenceInfo(GetItemType()).DataSource; } }

        private object locking = new object();

		protected virtual int Load() { return this.Load(""); }
		protected virtual int Load(string action)
		{
            lock (locking)
                using (Connection cn = Database.Open())
                {
                    int returned = this.Load(action, cn);
                    cn.Complete();
                    return returned;
                }
		}

        protected virtual int Load(Connection cn) { return this.Load("", cn); }
        protected virtual int Load(string action, Connection cn)
        {
            lock (locking)
                return Database.LoadAll(this, action, cn);
        }

        protected int Load(string action, object parameters, Connection cn)
        {
            lock (locking)
                return Database.LoadAll(this, action, parameters, cn);
        }

        //public static PersistentList<T> For(string action, object parameters)
        //{
        //    using (Connection cn = Database.Open())
        //    {
        //        PersistentList<T> list = For(action, parameters, cn);
        //        cn.Complete();
        //        return list;
        //    }
        //}

        //public static PersistentList<T> For(string action, object parameters, Connection cn)
        //{
        //    Type thisType = MethodInfo.GetCurrentMethod().DeclaringType;
        //    PersistentList<T> list = (PersistentList<T>)Activator.CreateInstance(thisType);
        //    list.Load(action, parameters, cn);
        //    return list;
        //}

        public int Count
		{
			get
			{
				lock (_lock)
					return _list.Count(); 
			}
		}

		//if paginated, will be different than count
		internal int _totalRows;
		public int TotalRows { get { return (_totalRows == 0) ? this.Count() : _totalRows; } }

		public void Add(T o)
		{
			lock (_lock)
				if (!_list.Contains(o))
					_list.Add(o);
		}

		public void Remove(T o)
		{
			lock (_lock)
				_list.Remove(o);
		}

		public void StoreBulk()
		{
            lock (_lock)
                Database.StoreBulk(this);
		}

		public DataTable ToDataTable(DataTable table)
		{
			foreach (T o in this)
				Database.MapToTable(o, table);

			return table;
		}

		public DataTable ToDataTable()
		{
			DataTable table = ToDataTableStructureOnly();
			return ToDataTable(table);
		}

		private DataTable ToDataTableStructureOnly()
		{
			DataTable table = new DataTable();

			foreach (PropertyInfo pi in this.GetItemType().GetProperties())
			{
				DataColumn col = new DataColumn();
				col.ColumnName = pi.Name;
				col.DataType = pi.PropertyType;
				table.Columns.Add(col);
			}
			return table;
		}

		public T ItemById(int id)
		{
            string primaryKey = (Class.GetPersistenceInfo(typeof(T))).PrimaryKeyName;
            PropertyInfo pi = (typeof(T)).GetProperty(primaryKey);
            if (pi.PropertyType != typeof(int))
                throw new ApplicationException(String.Format("Primary key of '{0}' must be of type Int32.", typeof(T).ToString()));

            foreach (T o in this)
                if ((int)pi.GetValue(o, null) == id)
                    return o;

            return default(T);

            //if (Class.GetPersistenceInfo(o).PrimaryKeyValue == id)
            //    return o;
            //throw new ApplicationException(String.Format("{0} '{1}' was not found in {2}.", this.GetItemType().Name, id, this.GetType().Name));
		}

		public void Sort<TValue>(Func<T, TValue> selector)
		{
			lock (_lock)
				_list.Sort((x, y) => Comparer<TValue>.Default.Compare(selector(x), selector(y)));
		}

		public void SortDescending<TValue>(Func<T, TValue> selector)
		{
			lock (_lock)
				_list.Sort((x, y) => Comparer<TValue>.Default.Compare(selector(y), selector(x)));
		}

		public void Store()
		{
			foreach (T o in this)
				Database.Store(o);
		}

		public void Delete()
		{
			foreach (T o in this)
			{
				Database.Delete(o);
				this.Remove(o);
			}
		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return this.GetList().GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetList().GetEnumerator();
		}

		#endregion

        #region ICollection Members

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            //index = this.PageIndex * this.PageSize;

            foreach (T item in this.GetList())
                array.SetValue(Class.GetPersistenceInfo(item).PrimaryKeyValue, index++);
        }

        int System.Collections.ICollection.Count
        {
            get { return this.GetList().Count; }
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return true; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get { return _lock; }
        }

        #endregion
    }
}
