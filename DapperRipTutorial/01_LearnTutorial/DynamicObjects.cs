using Dapper;
using DapperRipTutorial.Models;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DapperRipTutorial.LearnTutorial
{
    public class DynamicObjects
    {
        /*
         * Dynamic objects were added in C# 4 and are useful in many 
         * dynamic scenarios when dealing with JSON objects
         */
        private readonly string _connectionString;

        public DynamicObjects(string connectionString)
        {
            _connectionString = connectionString;
        }
     
        public void GetAuthors()
        {
            int[] ids = new[] { 1, 2, 3, 4 };
            var sql = "Select * from Authors where Id in @Ids";

            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                var authors = connection.Query<Author>(sql, new { Ids = ids }).ToList();

                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.FirstName} - {author.LastName}");
                }                
            }            
        }

        public void GetDynamicAuthors()
        {
            int[] ids = new[] { 1, 2, 3, 4 };
            var sql = "Select * from Authors where Id in @Ids";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var authors = connection.Query(sql, new { Ids = ids }).ToList();

                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.FirstName} - {author.LastName}");
                }
            }
        }
    }
}
