using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationTest
{
    internal class FractionCalculator
    {

        public Fraction MultiplayFraction (Fraction fraction1, Fraction fraction2)
        
        {
            
            return new Fraction(fraction1.Numerator * fraction2.Numerator, fraction1.Denominator * fraction2.Denominator);

        }

        public Fraction DivisionFraction(Fraction fraction1, Fraction fraction2)

        {

            return new Fraction(fraction1.Numerator * fraction2.Denominator, fraction1.Denominator * fraction2.Numerator);

        }

        public Fraction SumFraction(Fraction fraction1, Fraction fraction2)

        {
            int numerator = (fraction1.Numerator * fraction2.Denominator) + (fraction2.Numerator * fraction1.Denominator);
            int denominator = fraction1.Denominator * fraction1.Numerator;
            return new Fraction((fraction1.Numerator * fraction2.Denominator) + (fraction2.Numerator * fraction1.Denominator), fraction1.Denominator * fraction2.Denominator);

        }


        

             public Fraction Subdivision(Fraction fraction1, Fraction fraction2)

        {
            int numerator = (fraction1.Numerator * fraction2.Denominator) - (fraction2.Numerator * fraction1.Denominator);
            int denominator = fraction1.Denominator * fraction1.Numerator;
            return new Fraction((fraction1.Numerator * fraction2.Denominator) - (fraction2.Numerator * fraction1.Denominator), fraction1.Denominator * fraction2.Denominator);

        }

    }
}
