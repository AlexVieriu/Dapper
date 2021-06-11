using Dapper;
using Microsoft.Data.SqlClient;
using System;

namespace DapperTutorial.Dapper.Parameters
{
    public class Param_Anonymous
    {
        private readonly string _connectionString;

        public Param_Anonymous(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Single(string sql)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = connection.Execute(sql, new { FirstName = "Alexxx" });
                
                Console.WriteLine($"Single: {affectedRows}");
            }
        }

        public void Many(string sql)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = connection.Execute
                    (sql, 
                    new[]{
                        new { FirstName = "Alex1" },
                        new { FirstName = "Alex2" },
                        new { FirstName = "Alex3" },
                    });

                Console.WriteLine($"Many: {affectedRows}");                
            }
        }
    }
}
