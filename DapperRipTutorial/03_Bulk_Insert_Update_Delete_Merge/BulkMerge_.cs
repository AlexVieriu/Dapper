using Dapper;
using DapperRipTutorial.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Z.Dapper.Plus;

namespace DapperRipTutorial.Bulk_Insert_Update_Delete_Merge
{
    public class BulkMerge_
    {
        private readonly string _connectionstring;

        public BulkMerge_(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        public void MergeSingle()
        {
            var customer = new List<Customer>
            {
                new Customer{ CustomerName = "BulkMerge", ContactName = "Vieriu" + 1}
            };
            // map class to table 
            DapperPlusManager.Entity<Customer>().Table("Customers");

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.BulkMerge(customer);
            }
        }

        public void MergeMany()
        {
            var customers = new List<Customer>();
            var sql = "Select * from Customers";
            using (var connection = new SqlConnection(_connectionstring))
            {
                customers = connection.Query<Customer>(sql).ToList();
            }
            for (int i = 0; i < 5; i++)
            {
                customers.Add(new Customer() { CustomerName = "ExampleBulkMerge", ContactName = "Example Name:" + i });
            }

            customers.ForEach(x => { x.Address = "DapperPlus"; x.CustomerName = "ExampleBulkMerge"; });

            DapperPlusManager.Entity<Customer>().Table("Customers");

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.BulkMerge(customers);
            }
        }

        public void MergeOneToOne()
        {
            var suppliers = new List<Supplier>()
            {
                new Supplier()
                {
                    SupplierName = "ExampleSupplierBulkMerge",
                    Product = new Product()  { ProductName = "ExampleProductBulkMerge" }
                }
            };


            Console.WriteLine("ID of the new Supplier, before Merge : " + suppliers.First().SupplierID);

            var sql = "Select A.SupplierID, A.SupplierName, B.ProductID, B.ProductName " +
                        "FROM Suppliers as A " +
                        "inner join Products as B " +
                        "on B.SupplierID = A.SupplierID " +
                        "where productID = 24";

            using (var connection = new SqlConnection(_connectionstring))
            {
                var supplier = connection.Query<Supplier, Product, Supplier>(sql,
                    (supplier, product) => { supplier.Product = product; return supplier; },
                    splitOn: "ProductID")
                    .ToList();

                suppliers.AddRange(supplier);
            }

            suppliers.ForEach(x => x.SupplierName = "SupplierBulkMerge");
            suppliers.ForEach(x => x.Product.ProductName = "ProductBulkMerge");

            DapperPlusManager.Entity<Supplier>().Table("Suppliers").Identity(x => x.SupplierID);
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.ProductID);

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.BulkMerge(suppliers)
                    .ThenForEach(x => x.Product.SupplierID = x.SupplierID)
                    .ThenBulkMerge(x => x.Product);
            }
        }

        public void MergeOneToMany()
        {
            var suppliers = new List<Supplier>();

            using (var connection = new SqlConnection(_connectionstring))
            {
                var supplierDictionary = new Dictionary<int, Supplier>();
                var sql = "Select A.SupplierID, A.SupplierName, B.ProductID, B.ProductName, B.Unit " +
                            "FROM Suppliers as A " +
                            "inner join Products as B " +
                            "on B.SupplierID = A.SupplierID " +
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
                    }

                    supplierEntry.Products.Add(product);
                    return supplierEntry;
                }, splitOn: "ProductID").Distinct().ToList();

            }

            suppliers.ForEach(x =>
            {
                x.SupplierName = "SupplierBulkMerge";
                x.Products.ForEach(y => y.ProductName = "ProductBulkMerge");
            });

            suppliers.Add(new Supplier()
            {
                SupplierName = "SupplierBulkMerge",
                Products = new List<Product>()
                {
                    new Product() { ProductName = "ProductBulkMerge", Unit = "1"},
                    new Product() { ProductName = "ExampleProductBulkMerge" , Unit = "2"} ,
                    new Product() { ProductName = "ExampleProductBulkMerge", Unit = "3" }
                }
            });



            DapperPlusManager.Entity<Supplier>().Table("Suppliers").Identity(x => x.SupplierID);
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.ProductID);

            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.BulkMerge(suppliers)
                    .ThenForEach(x => x.Products.ForEach(y => y.SupplierID = x.SupplierID))
                    .ThenBulkMerge(x => x.Products);
            }
        }
    }
}
