using System;

namespace DapperRipTutorial.Models
{
    public class Dog
    {
        public int? Age { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public int IgnoredProperty
        {
            get { return 1; }
        }
    }
}
