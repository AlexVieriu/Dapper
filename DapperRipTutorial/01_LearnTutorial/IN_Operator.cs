using Dapper;
using DapperRipTutorial.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DapperRipTutorial.LearnTutorial
{
    public class IN_Operator
    {
        private readonly string _connectionString;

        public IN_Operator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void GetAuthors()
        {
            var sql = "Select * from Authors where Id in @Ids";
            var ids = new[] { 3, 4, 5, };

            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                var authors = connection.Query<Author>(sql, new { Ids = ids}).ToList();

                foreach (var author in authors)
                {
                    Console.WriteLine(author.FirstName + " - " + author.LastName);
                }
            }
        }
    }
}
