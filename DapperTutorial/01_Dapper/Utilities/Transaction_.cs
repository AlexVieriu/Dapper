using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DapperTutorial._Dapper.Utilities
{
    public class Transaction_
    {
        private readonly string _connectionString;

        public Transaction_(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Transation()
        {
            string sql = "INSERT INTO Customers (FirstName) Values (@FirstName);";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    // Dapper
                    var affectedRows1 = connection.Execute(sql, new { FirstName = "Mark" }, transaction: transaction);

                    // Dapper Transaction
                    //var affectedRows2 = transaction.Execute(sql, new { CustomerName = "Mark" });

                    transaction.Commit();

                    Console.WriteLine(affectedRows1);
                }
            }
        }
    }
}
