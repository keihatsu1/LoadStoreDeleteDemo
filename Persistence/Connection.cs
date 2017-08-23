using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;

namespace Persistence
{
    public class Connection : IDisposable
    {
        private DbConnection _cn;
        private DbTransaction _trans;

        internal Connection(bool withTransaction)
        {
            _cn = Database.GetConnection();
            _cn.Open();
            if (withTransaction)
                _trans = _cn.BeginTransaction();

        }

        public void Complete()
        {
            if (_trans != null)
                _trans.Commit();
        }

        public void Dispose()
        {
            if (_trans != null && _trans.Connection != null && _trans.Connection.State == ConnectionState.Open)
                _trans.Rollback();

            if (_trans != null)
                _trans.Dispose();

            if (_cn != null && _cn.State == ConnectionState.Open)
                _cn.Close();

            _cn.Dispose();

            GC.SuppressFinalize(_cn);
            GC.SuppressFinalize(this);
        }

        internal DbConnection DbConnection
        {
            get { return _cn; }
        }

        internal DbTransaction DbTransaction
        {
            get { return _trans; }
        }
    }
}
