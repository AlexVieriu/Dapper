using Dapper;
using DapperRipTutorial.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DapperRipTutorial.LearnTutorial
{
    public class UpdateData
    {
        private readonly string _conectionString;

        public UpdateData(string conectionString)
        {
            _conectionString = conectionString;
        }

        public void UpdateAuthor()
        {
            var sql = @"Update Authors 
                        Set FirstName = 'ParamPamPam'
                        Where Id = @Id";

            using (IDbConnection connection = new SqlConnection(_conectionString))
            {
                var rowsAffected = connection.Execute(sql, new { Id = 2 });

                Console.WriteLine($"RowsAffected - {rowsAffected}");
            }
        }

        public void UpdateAuthors()
        {
            var sql = @"Update Authors
                        Set FirstName = @FirstName
                        Where Id = @Id";

            using(IDbConnection connection = new SqlConnection(_conectionString))
            {
                var rowsAffected = connection.Execute(sql,
                    new[]
                    {
                        new{ Id = 8, FirstName = "Alex"},
                        new{ Id = 9, FirstName = "Alina"},
                        new{ Id = 10, FirstName = "Ioana"},
                    });

                Console.WriteLine($"RowsAffected - {rowsAffected}");
            }
        }
    }
}
