using NHibernate;
using NHibernate.SqlCommand;
using Serilog;

namespace SimpleMapViewer.Infrastructure.Database {
    internal class DatabaseLoggerInterceptor : EmptyInterceptor {
        public override SqlString OnPrepareStatement(SqlString sqlString) {
            Log.Information(sqlString.ToString());
            return sqlString;
        }
    }
}