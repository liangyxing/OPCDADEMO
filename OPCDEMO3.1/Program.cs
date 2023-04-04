using OPCDALIB.LIB.OPCDA;
using System;

namespace OPCDEMO3._1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DA dA = new DA();
            dA.GetServers();
            Console.WriteLine("Hello World!");
        }
    }
}
