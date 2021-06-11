using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;

namespace DapperTutorial.Dapper.Utilities
{
    public class StoreProcedure_
    {
        private readonly string _connectionString;

        public StoreProcedure_(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ExecuteSingle()
        {
            var sql = "sp_Customer_Insert";
            using(var connection = new SqlConnection(_connectionString))
            {

                var affectedrows = connection.Execute(sql,
                                                      new { FirstName = "Tzigan", LastName = "Tzatzichi", Email = "" },
                                                      commandType: CommandType.StoredProcedure) * -1;
                Console.WriteLine(affectedrows);
            }
        }

        public void QuerySingle()
        {

        }
        
        
        public void ExecuteMany()
        {
            var sql = "sp_Customer_Insert";
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.ExecuteAsync(
                    sql,
                    new[]
                    {
                        new { FirstName = "Tzigan1", LastName = "Tzatzichi1", Email = "" },
                        new { FirstName = "Tzigan2", LastName = "Tzatzichi2", Email = "" },
                        new { FirstName = "Tzigan3", LastName = "Tzatzichi3", Email = "" },                                               
                    }).Result;

                Console.WriteLine(result);
            }
        }

        public void QueryMany()
        {
            var sql = "sp_SelectCustomerById";
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.QueryAsync(
                    sql,
                    new[]
                    {
                        new{CustomerId = 1},
                        new{CustomerId = 2},
                        new{CustomerId = 3}
                    },
                    commandType: CommandType.StoredProcedure).Result;

                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }                
            }
        }
    }
}
