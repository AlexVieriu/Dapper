using Dapper;
using DapperRipTutorial.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Z.Dapper.Plus;

namespace DapperRipTutorial.Bulk_Insert_Update_Delete_Merge
{
    public class BulkDelete_
    {
        // https://dapper-tutorial.net/bulk-delete

        private readonly string _connectionString;

        public BulkDelete_(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void DeleteSingle()
        {
            DapperPlusManager.Entity<Customer>().Table("Customers").Key("CustomerID");

            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "Select * FROM CUSTOMERS WHERE CustomerID = 5)";
                var customer = connection.Query<Customer>(sql).ToList();

                connection.BulkDelete(customer);
            }
        }

        public void DeleteMany()
        {
            DapperPlusManager.Entity<Customer>().Table("Customers").Key("CustomerID");

            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "Select * FROM CUSTOMERS WHERE CustomerID in (5,6)";
                var customers = connection.Query<Customer>(sql).ToList();

                connection.BulkDelete(customers);
            }
        }

        public void DeleteOneToOne()
        {
            // Get the Data 
            var suppliers = new List<Supplier>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"Select A.SupplierID, A.SupplierName, B.ProductID, B.ProductName 
                            FROM Suppliers as A 
                            inner join Products as B 
                            on B.SupplierID =A.SupplierID 
                            WHERE productID = 4";

                suppliers = connection.Query<Supplier, Product, Supplier>(sql,
                    (supplier, product) =>
                    {
                        supplier.Product = product;
                        return supplier;
                    },
                     splitOn: "ProductID")
                    .ToList();
            }

            DapperPlusManager.Entity<Supplier>().Table("Suppliers").Identity(x => x.SupplierID);
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.ProductID);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.BulkDelete(suppliers.Select(x => x.Product))
                    .BulkDelete(suppliers);
            }
        }

        public void DeleteOneToMany()
        {
            var suppliers = new List<Supplier>();

            using (var connection = new SqlConnection(_connectionString))
            {
                // get data
                var supplierDictionary = new Dictionary<int, Supplier>();

                var sql = "Select A.SupplierID, A.SupplierName, B.ProductID, B.ProductName " +
                            "FROM Suppliers as A " +
                            "inner join Products as B " +
                            "on B.SupplierID =A.SupplierID " +
                            "where A.supplierID = 1";

                suppliers = connection.Query<Supplier, Product, Supplier>(sql,
                    (supplier, product) =>
                    {
                        Supplier supplierEntry;
                        if (!supplierDictionary.TryGetValue(supplier.SupplierID, out supplierEntry))
                        {
                            supplierEntry = supplier;
                            supplierEntry.Products = new List<Product>();
                            supplierDictionary.Add(supplier.SupplierID, supplierEntry);
                        };
                        supplierEntry.Products.Add(product);
                        return supplierEntry;
                    },
                    splitOn: "ProductID")
                    .Distinct()
                    .ToList();
            }

            // map data
            DapperPlusManager.Entity<Supplier>().Table("Suppliers").Identity(x => x.SupplierID);
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.ProductID);

            // delete data
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.BulkDelete(suppliers.SelectMany(x => x.Products))
                    .BulkDelete(suppliers);
            }
        }
    }
}

