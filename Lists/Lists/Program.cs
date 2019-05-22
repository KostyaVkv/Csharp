using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lists
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<string> strList;
            //strList = new List<string>();
            //strList.Add("Paris");
            //strList.Add("Paris");
            //strList.Add("Amsterdam");
            //strList.Add("Madrid");
            //strList.Add("London");
            //strList.Add("Paris");
            //Print(strList);
            //Console.WriteLine();
            //Remove(strList, "Paris");
            //Print(strList);
            List<Gem> gems;
            gems = new List<Gem>();
            Gem G = new Gem();
            gems.Add(G);
            G = new Gem();
            gems.Add(G);

        }
        static void Remove(List<string> Liststr,string element)
        {
            int i = 0;
            while(i<Liststr.Count)
            {
                if(Liststr[i]==element)
                {
                    Liststr.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
        static void Print(List<string> Liststr)
        {
            for(int i=0;i<Liststr.Count;i++)
            {
                Console.WriteLine(Liststr[i]);
            }
            Console.ReadKey();
        }
    }
    class Gem
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public string Picture;
        public string ContainerName;
        public void SetCoordinates(int x, int y)
        {
            X = x;
            Y = y;
            //Helper.map.ContainerSetCoordinate(ContainerName, x, y);
        }
    }
}
