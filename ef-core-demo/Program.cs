using System;
using System.Linq;
using ef_core_demo.db;

namespace ef_core_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BloggingContext())
            {
                var blogs = db.Blogs
                    .Where(b => b.Rating > 3)
                    .OrderBy(b => b.Url)
                    .ToList();
                Console.WriteLine(blogs.Count);
            }
        }
    }
}