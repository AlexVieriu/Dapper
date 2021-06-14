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
    public class BulkInsert_
    {
        // https://dapper-tutorial.net/bulk-insert

        private readonly string _connectionString;

        public BulkInsert_(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Insert Single
        public void InsertSingle()
        {
            DapperPlusManager.Entity<Customer>().Table("Customers");

            var customers = new List<Customer>()
            {
                new Customer(){ CustomerName = "Vieriu", City= "Bucharest"}
            };

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var result = connection.BulkInsert(customers);
            };
        }

        // Insert Many
        public void InsertMany()
        {
            DapperPlusManager.Entity<Customer>().Table("Customers");

            var customers = new List<Customer>()
            {
                new Customer(){ CustomerName = "Vieriu", City= "Bucharest"},
                new Customer(){ CustomerName = "Dumitrascu", City= "Bucharest"},
                new Customer(){ CustomerName = "Cercel", City= "Bucharest"}
            };

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var result = connection.BulkInsert(customers);

            }

        }

        // Insert One to One
        public void InsertOneToOne()
        {
            DapperPlusManager.Entity<Supplier>().Table("Suppliers").Identity(x => x.SupplierID);
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.ProductID);

            var suppliers = new List<Supplier>()
            {
                new Supplier()
                {
                    ContactName = "Vieriu Alexandru",
                    Product = new Product() { ProductName = "Book" }
                }
            };

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var result = connection.BulkInsert(suppliers)
                    .ThenForEach(x => x.Product.SupplierID = x.SupplierID)
                    .ThenBulkInsert(x => x.Product);
            }
        }

        // Insert One to Many
        public void InsertOneToMany()
        {
            // Method 1
            DapperPlusManager.Entity<Supplier>().Table("Suppliers").Identity(x => x.SupplierID);
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.ProductID);

            var suppliers = new List<Supplier>()
            {
                new Supplier()
                {
                    ContactName = "Vieriu_2",
                    Products = new List<Product>
                    {
                        new Product{ ProductName = "Product1"},
                        new Product{ ProductName = "Product2"},
                        new Product{ ProductName = "Product3"},
                    }
                }
            };

            using (IDbConnection connection = new SqlConnection())
            {
                connection.BulkInsert(suppliers)
                    .ThenForEach(x => x.Products.ForEach(y => y.SupplierID = x.SupplierID))
                    .ThenBulkInsert(x => x.Products);
            }

            // Method 2
            suppliers = null;

            using (IDbConnection connection = new SqlConnection())
            {
                var supplierDictionary = new Dictionary<int, Supplier>();
                var sql = @"Select A.SupplierID, A.SupplierName, B.ProductID, B.ProductName 
                            FROM Suppliers as A 
                            inner join Products as B on B.SupplierID =A.SupplierID 
                            where A.SupplierName = 'ExampleSupplierBulkInsert'";

                suppliers = connection.Query<Supplier, Product, Supplier>(sql,
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
                }, splitOn: "ProductID")
                    .Distinct()
                    .ToList();
            }
        }
    }
}
