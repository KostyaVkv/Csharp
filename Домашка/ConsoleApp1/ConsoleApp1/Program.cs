﻿using System;
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
            int[,] homework = new int[7, 7];
            for (int i =0; i < 4; i++)
            {
                for (int j = 3-i; j <=3+i ; j++)
                {
                    homework[i, j] = i+1;
                    homework[6 - i, j] = i + 1;
                }
            }

            //for(int i=0;i<8;i++)
            //{
            //    for(int j=0;j<8;j++)
            //    {
            //        homework[i, j] = 1 + j + i;
            //    }
            //}
            int[,] Work = new int[8, 8];
            //for(int a=0;a<8;a++)
            //{
            //    for(int b=0;b<=a;b++)
            //    {
            //        Work[a, b] = b + 1;
            //    }
            //}
            NonMain(homework);
            //NonMain(Work);
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
