using Dapper;
using DapperRipTutorial.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Z.Dapper.Plus;

namespace DapperRipTutorial.Bulk_Insert_Update_Delete_Merge
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

            using (_connection)
            {
                _connection.BulkInsert(suppliers)
                    .ThenForEach(x => x.Product.SupplierID = x.SupplierID)
                    .ThenBulkInsert(x => x.Product);
            }
        }

        public void InsertOneToMany()
        {
            // Example 1:
            // get the data
            var suppliers = new List<Supplier>()
            {
                new Supplier()
                {
                    SupplierName = "ExampleSupplierBulkInsert",
                    Products = new List<Product>
                    {
                        new Product() {ProductName = "ExampleProductBulkInsert", Unit = "1"},
                        new Product() {ProductName = "ExampleProductBulkInsert", Unit = "2"} ,
                        new Product() {ProductName = "ExampleProductBulkInsert", Unit = "3"}
                    }
                }
            };

            // map the Class with the Table
            DapperPlusManager.Entity<Supplier>().Table("Suppliers").Identity(x => x.SupplierID);
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.ProductID);

            using (_connection)
            {
                _connection.BulkInsert(suppliers)
                    .ThenForEach(x => x.Products.ForEach(y => y.SupplierID = x.SupplierID))
                    .ThenBulkInsert(x => x.Products);
            }


            // Exemple 2 -- very hard to understand
            suppliers = null;

            using (_connection)
            {
                var supplierDictionary = new Dictionary<int, Supplier>();
                var sql = @"Select A.SupplierID, A.SupplierName, B.ProductID, B.ProductName 
                            FROM Suppliers as A 
                            inner join Products as B on B.SupplierID =A.SupplierID 
                            where A.SupplierName = 'ExampleSupplierBulkInsert'";

                suppliers = _connection.Query<Supplier, Product, Supplier>(sql,
                (supplier, product) =>
                {
                    Supplier supplierEntry;

                    if (!supplierDictionary.TryGetValue(supplier.SupplierID, out supplierEntry))
                    {
                        supplierEntry = supplier;
                        supplierEntry.Products = new List<Product>();
                        supplierDictionary.Add(supplier.SupplierID, supplierEntry);
                    }

                    supplierEntry.Products.Add(product);
                    return supplierEntry;
                }, splitOn: "ProductID").Distinct().ToList();
            }
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
