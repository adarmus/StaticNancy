using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;

namespace StaticNancy
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:12345")))
            {
                host.Start();
                Console.WriteLine("Running on http://localhost:12345");
                Console.ReadLine();
            }
        }
    }
}
