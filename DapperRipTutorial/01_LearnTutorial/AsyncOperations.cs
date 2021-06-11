using Dapper;
using DapperRipTutorial.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DapperRipTutorial.LearnTutorial
{

    public class AsyncOperations
    {
        private readonly string _conectionString;

        public AsyncOperations(string conectionString)
        {
            _conectionString = conectionString;
        }

        public async void GetAllAuthorAsync()
        {
            var sql = "Select * from Authors";
            using(IDbConnection connection = new SqlConnection(_conectionString))
            {
                var authors = await connection.QueryAsync<Author>(sql);

                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.FirstName} - {author.LastName}");
                }
            }
        }

        public async void InsertSingleAuthorAsync()
        {
            var sql = "Insert into Authors(FirstName, LastName) Values(@FirstName, @LastName)";

            using(IDbConnection connection = new SqlConnection(_conectionString))
            {
                var rowAffected = await connection.ExecuteAsync(sql, new { FirstName = "Vieriu", LastName = "Alexandru" });

                Console.WriteLine($"RowAffected - {rowAffected}");               
            }            
        }

    }
}
