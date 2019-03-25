using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    { 
        
        static void Main(string[] args)
        {
            int[,] homework = new int[8, 8];
            for(int i=0;i<8;i++)
            {
                for(int j=0;j<8;j++)
                {
                    homework[i, j] = 1 + j + i;

                }
            }
            NonMain(homework);
            Console.ReadKey();
        }
        static void NonMain(int [,] a)
        {
            for(int i = 0;i<a.GetLength(0);i++)
            {
                for(int j=0;j<a.GetLength(1);j++)
                {
                    
                    Console.Write(a[i, j]);
                 
                    Console.Write("\t");
                }
                Console.WriteLine();
            }
            
        }
    }
}
