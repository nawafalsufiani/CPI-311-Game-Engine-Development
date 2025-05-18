using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{


    public class Fraction
    {

        private int numerator; // field

        public int Numerator   // property
        {
            get { return numerator; }   // get method
            set { numerator = value; }  // set method
        }


        private int denominator; // field

        public int Denominator   // property
        {
            get { return denominator; }   // get method
            set { denominator = value; }  // set method
        }



        public Fraction(int n, int d)
        {
            Numerator = n;
            Denominator = d;


        }




    }




}
