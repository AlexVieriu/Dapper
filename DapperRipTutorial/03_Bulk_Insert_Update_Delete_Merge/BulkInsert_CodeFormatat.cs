using Dapper;
using DapperRipTutorial.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Z.Dapper.Plus;

namespace DapperRipTutorial.BulkInsert
{
    public class BulkInsert_CodeFormatat
    {
        // https://dapper-tutorial.net/bulk-insert

        private IDbConnection _connection;

        public BulkInsert_CodeFormatat(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void InsertSingle()
        {
            DapperPlusManager.Entity<Customer>().Table("Customer");

            using (_connection)
            {
                var customerList = new List<Customer>()
                {
                    new Customer() { CustomerName = "ExampleBulkInsert", ContactName="Example Name: " + 1}
                };

                _connection.BulkInsert(customerList);
            }

            ReadCustomers();
        }

        public void InsertMany()
        {
            var customers = new List<Customer>();
            for (int i = 0; i < 20; i++)
            {
                customers.Add(new Customer() { CustomerName = "ExemapleBulkInsert", ContactName = "Example Name :" + i });
            }

            DapperPlusManager.Entity<Customer>().Table("Customers");

            using (_connection)
            {
                _connection.BulkInsert(customers);
            }

            ReadCustomers();
        }

        public void InsertOneToOne()
        {
            var suppliers = new List<Supplier>()
            {
               new Supplier() {
                   SupplierName = "ExampleSupplierBulkInsert",
                   Product = new Product() { ProductName = "ExampleProductBulkInsert" }
               }
            };

            DapperPlusManager.Entity<Supplier>().Table("Suppliers").Identity(x => x.SupplierID);
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.ProductID);
        }

        private void ReadCustomers()
        {
            using (_connection)
            {
                var sql = "Select * from Customers where CustomerName = 'ExampleBulkInsert'";
                var results = _connection.Query<Customer>(sql).ToList();

                foreach (var result in results)
                {
                    Console.WriteLine($"{result.CustomerName} - {result.ContactName}");
                }
            }
        }
    }
}
