using Dapper;
using Microsoft.Data.SqlClient;
using System;

namespace DapperTutorial.Dapper.Result
{
    public class Result_Anonymous
    {
        private readonly string _connectionString;

        public Result_Anonymous(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Query()
        {
            string sql = "SELECT * FROM Customers";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = connection.QueryFirstOrDefault(sql);

                Console.WriteLine(customer);
            }
        }

        public void QueryFirst()
        {
            string sql = "SELECT * FROM Customers where CustomerId = @CustomerID";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = connection.QueryFirst(sql, new { CustomerId = 1});

                Console.WriteLine(customer);
            }
        }

        public void QueryFirstOrDefault()
        {
            string sql = "SELECT * FROM Customers where CustomerId = @CustomerID";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = connection.QueryFirstOrDefault(sql, new { CustomerId = 1 });

                Console.WriteLine(customer);
            }
        }

        public void QuerySingle()
        {
            string sql = "SELECT * FROM Customers where CustomerId = @CustomerID";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = connection.QuerySingle(sql, new { CustomerId = 1 });

                Console.WriteLine(customer);
            }
        }

        public void QuerySingleOrDefault()
        {
            string sql = "SELECT * FROM Customers where CustomerId = @CustomerID";

            using (var connection = new SqlConnection(_connectionString))
            {
                var customer = connection.QueryFirstOrDefault(sql, new { CustomerId = 1 });

                Console.WriteLine(customer);
            }
        }
    }
}
