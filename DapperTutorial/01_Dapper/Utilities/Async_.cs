using Dapper;
using DapperTutorial.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DapperTutorial.Dapper.Utilities
{
    public class Async_
    {
        private readonly string _connectionString;

        public Async_(string connectionString)
        {
            _connectionString = connectionString;
        }


        public void ExecuteAsync()
        {
            string sql = "INSERT INTO Customers (FirstName) Values (@FirstName);";

            using (var connection = new SqlConnection(_connectionString))
            {
                // result : Gets the result value of this System.Threading.Tasks.Task`1
                var affectedRows = connection.ExecuteAsync(sql, new { FirstName = "Mark" }).Result;

                Console.WriteLine($"affectedRows: {affectedRows}");

                var customers = connection.Query<Customer>("Select * FROM CUSTOMERS WHERE FirstName = 'Mark'").ToList();

                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} - {customer.LastName}");
                }                
            }
        }

        public void QueryAsync()
        {
            string sql = "SELECT top 10 * FROM Customers";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customers = connection.QueryAsync<Customer>(sql).Result.ToList();

                Console.WriteLine(customers.Count());

                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} - {customer.LastName}");
                }
            }
        }
        public void QueryFirstAsync()
        {
            string sql = "SELECT * FROM Customers where CustomerId = @CustomerId";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = connection.QueryFirstAsync<Customer>(sql, new { CustomerId = 1}).Result;

                Console.WriteLine($"{customer.FirstName} - {customer.LastName}");
            }
        }
        public void QueryFirstOrDefaultAsync()
        {
            string sql = "SELECT * FROM Customers where CustomerId = @CustomerId";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = connection.QueryFirstOrDefaultAsync<Customer>(sql, new { CustomerId = 1 }).Result;

                Console.WriteLine($"{customer.FirstName} - {customer.LastName}");
            }
        }
        public void QuerySingleAsync()
        {
            string sql = "SELECT * FROM Customers where CustomerId = @CustomerId";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = connection.QuerySingleAsync<Customer>(sql, new { CustomerId = 1 }).Result;

                Console.WriteLine($"{customer.FirstName} - {customer.LastName}");
            }
        }

        public void QuerySingleOrDefaultAsync()
        {
            string sql = "SELECT * FROM Customers where CustomerId = @CustomerId";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = connection.QuerySingleOrDefaultAsync<Customer>(sql, new { CustomerId = 1 }).Result;

                Console.WriteLine($"{customer.FirstName} - {customer.LastName}");
            }
        }
    }
}
