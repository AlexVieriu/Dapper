using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DapperRipTutorial.LearnTutorial
{
    public class DeleteData
    {
        private readonly string _connectionString;

        public DeleteData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void DeleteSingleAuthor()
        {
            string sql = "Delete from Authors where Id = @Id;";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rowsAffected = connection.Execute(sql, new { Id = 6 });

                Console.WriteLine($"RowsAffected - {rowsAffected}");                
            }
        }

        public void DeleteMultipleAuthors()
        {
            string sql = "Delete from Authors where Id = @Id;";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var rowsAffected = connection.Execute(sql,
                            new[]
                            {
                                new{ Id = 5},
                                new{ Id = 6},
                                new{ Id = 7},
                            });
                
                Console.WriteLine($"RowsAffected - {rowsAffected}");
            }
        }
    }
}
