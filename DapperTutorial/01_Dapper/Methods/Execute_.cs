using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DapperTutorial.Dapper.Methods
{
    public class Execute_
    {
        // Parameters : sql, param, transation, commandTimeout, commandType
        // Execute Stored Procedure

        private readonly string _connetionString;
        public Execute_(string connetionString)
        {
            _connetionString = connetionString;
        }

        public void SingleProcedure(string procedure)
        {
            using (var connection = new SqlConnection(_connetionString))
            {
                var affectedRows = connection.
                    Execute(procedure,
                    new { FirstName = "Vieriu1", LastName = "Alexandru1", Email = "" },
                    commandType: CommandType.StoredProcedure) * -1;

                Console.WriteLine($"SingleProcedure: {affectedRows}");
            }
        }

        public void ManyProcedure(string procedure)
        {
            using (var connection = new SqlConnection(_connetionString))
            {
                var affectedRows = connection.
                    Execute(procedure,
                    new[] {
                        new { FirstName = "Vieriu2", LastName= "Alexandru2",  Email = ""  },
                        new { FirstName = "Vieriu3", LastName= "Alexandru3",  Email = ""  },
                        new { FirstName = "Vieriu4", LastName= "Alexandru4",  Email = ""  }
                    },
                    commandType: CommandType.StoredProcedure) * -1;

                Console.WriteLine($"ManyProcedure: {affectedRows}");
            }
        }

        public void SingleInsert(string sql)
        {
            using (var connection = new SqlConnection(_connetionString))
            {
                var affectedRows = connection.Execute(sql, new { FirstName = "Alina" }) * -1;
                Console.WriteLine($"Single: {affectedRows}");
            }
        }

        public void ManyInsert(string sql)
        {
            using (var connection = new SqlConnection(_connetionString))
            {
                var affectedRows = connection.
                    Execute(sql,
                            new[]
                            {
                                new { FirstName = "Vieriu" },
                                new { FirstName = "Dumitrascu"},
                                new { FirstName = "Cercel" }
                            }) * -1;
                Console.WriteLine($"Many: {affectedRows}");
            }
        }

        public void SingleUpdate(string sql)
        {
            using (var connection = new SqlConnection(_connetionString))
            {
                var affectedRows = connection.
                    Execute(sql,
                            new { CustomerId = 6, FirstName = "Ciobanu"});

                Console.WriteLine($"SingleUpdate: {affectedRows}");
            }
        }

        public void ManyUpdate(string sql)
        {
            using (var connection = new SqlConnection(_connetionString))
            {
                var affectedRows = connection.
                    Execute(sql,
                            new[] {
                                new { CustomerId = 6, FirstName = "Ciobanu" },
                                new { CustomerId = 7, FirstName = "Vieriu" },
                                new { CustomerId = 8, FirstName = "Dumitrascu" }
                                }
                            );

                Console.WriteLine($"ManyUpdate: {affectedRows}");
            }
        }

        public void SingleDelete(string sql)
        {
            using(var connection = new SqlConnection(_connetionString))
            {
                var affectedRows = connection.Execute(sql, new { CustomerId = 9 });
                Console.WriteLine($"SingleDelete: {affectedRows}");
            }
        }

        public void ManyDelete(string sql)
        {
            using (var connection = new SqlConnection(_connetionString))
            {
                var affectedRows = connection.
                    Execute(sql, 
                            new[]{
                                  new { CustomerId = 11 }, 
                                  new { CustomerId = 15 }, 
                                  new { CustomerId = 16 } 
                            });

                Console.WriteLine($"ManyDelete: {affectedRows}");
            }
        }
    }
}
