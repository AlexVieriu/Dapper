using Dapper;
using DapperRipTutorial.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DapperRipTutorial.LearnTutorial
{
    public class MapColNames_with_ClassProp
    {
        private readonly string _connectionString;

        public MapColNames_with_ClassProp(string connectionString)
        {
            _connectionString = connectionString;
        }

        // If the class property Name doesn't match the DB column name, the record will display
        // 
        public void GetAuthors()
        {
            var sql = "Select * from Authors";
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                var authors = connection.Query<AuthorTest>(sql);

                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.FName} - {author.LastName}");
                }
            }
        }
    }
}
