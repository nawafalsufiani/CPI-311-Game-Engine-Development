using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class SimplifyFraction
    {


        public Fraction Simplify(Fraction fraction)
        {
            int gcd = GCD(fraction.Numerator, fraction.Denominator);
            fraction.Numerator /= gcd;
            fraction.Denominator /= gcd;
            return fraction;
        }

        static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

    }
}
