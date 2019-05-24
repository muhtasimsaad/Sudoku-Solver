using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication4
{
    class packet
    {

       public string [][] arr;
       public int[][] situation;
       public int[][] grid;

       public packet() { }
        public packet(int[][] g,string[][] a,int[][] s) {
             arr = a;
             situation = s;
             grid = g;




        }
    }
}
