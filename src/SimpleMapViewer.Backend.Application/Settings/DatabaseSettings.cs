using SimpleMapViewer.Infrastructure.Settings;

namespace SimpleMapViewer.Backend.Application.Settings {
    internal class DatabaseSettings : IDatabaseSettings {
        public string ConnectionString { get; }

        public DatabaseSettings(string connectionString) {
            ConnectionString = connectionString;
        }
    }
}