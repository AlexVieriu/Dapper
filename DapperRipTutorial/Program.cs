using DapperRipTutorial.LearnTutorial;
using System;

namespace DapperRipTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Learn Operations

            var sql = "Server=(local); Database=Dapper; Trusted_Connection = True;";

            var asyncOperations = new AsyncOperations(sql);
            asyncOperations.GetAllAuthorAsync();
            asyncOperations.InsertSingleAuthorAsync();
            



            var mapColNameswithClassProp = new MapColNames_with_ClassProp(sql);
            mapColNameswithClassProp.GetAuthors();
        }
    }
}
