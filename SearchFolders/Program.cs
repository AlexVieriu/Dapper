using System;
using System.IO;

namespace SearchFolders
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootdir = @"\\FILESERVER\Arhive_foto\Arhiva foto 2019\Inspectii_2019\BUCURESTI";

            var dirs = Directory.GetDirectories(rootdir, "*", SearchOption.AllDirectories);
            Console.WriteLine(String.Join(Environment.NewLine, dirs));


            Console.ReadLine();
        }
    }
}
