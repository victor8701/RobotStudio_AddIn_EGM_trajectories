using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  TFG_offline.MATHEMATICS
{
    internal class Approximation
    {
        public static double ToZero(double a)
        {
            if (a <= 0.000001) a = 0.0;
            return a;
        }
    }
}
