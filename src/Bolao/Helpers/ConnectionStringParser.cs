using System;

namespace Bolao.Helpers
{
    public static class ConnectionStringParser
    {
        /// <summary>
        /// Parses a PostgreSQL connection URI (e.g. postgres://user:pass@host:port/db) 
        /// into a standard connection string format expected by Npgsql.
        /// </summary>
        public static string Parse(string connectionUri)
        {
            if (string.IsNullOrEmpty(connectionUri))
            {
                return connectionUri;
            }

            if (connectionUri.StartsWith("postgres://") || connectionUri.StartsWith("postgresql://"))
            {
                try
                {
                    var uri = new Uri(connectionUri);
                    var userInfo = uri.UserInfo.Split(':');
                    var username = userInfo[0];
                    var password = userInfo.Length > 1 ? userInfo[1] : "";
                    var host = uri.Host;
                    var port = uri.Port > 0 ? uri.Port : 5432;
                    var database = uri.AbsolutePath.TrimStart('/');

                    // Render uses SSL connections by default. We configure SSL Mode=Require 
                    // and Trust Server Certificate=true for easier container-to-service handshake.
                    return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true;";
                }
                catch (Exception)
                {
                    // Fallback to original string if URI parsing fails
                    return connectionUri;
                }
            }

            return connectionUri;
        }
    }
}
