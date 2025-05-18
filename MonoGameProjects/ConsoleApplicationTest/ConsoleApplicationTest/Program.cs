// See https://aka.ms/new-console-template for more information

using ConsoleApplicationTest;

Fraction a = new Fraction(3,4);

Fraction b = new Fraction(8, 3);

FractionCalculator fractionCalculator = new FractionCalculator();
Fraction c = fractionCalculator.MultiplayFraction(a, b);

SimplifyFraction simplifyFraction = new SimplifyFraction();
simplifyFraction.Simplify(c);

Fraction d = fractionCalculator.SumFraction(a, b);


simplifyFraction.Simplify(d);

Fraction e = fractionCalculator.Subdivision(a, b);


simplifyFraction.Simplify(e);


Fraction f = fractionCalculator.DivisionFraction(a, b);


simplifyFraction.Simplify(f);



Console.WriteLine(a.Numerator + "/" + a.Denominator+"*"+ b.Numerator + "/" + b.Denominator+"="+ c.Numerator + "/" + c.Denominator);

Console.WriteLine(a.Numerator + "/" + a.Denominator + "+" + b.Numerator + "/" + b.Denominator + "=" + d.Numerator + "/" + d.Denominator);

Console.WriteLine(a.Numerator + "/" + a.Denominator + "-" + b.Numerator + "/" + b.Denominator + "=" + e.Numerator + "/" + e.Denominator);

Console.WriteLine(a.Numerator + "/" + a.Denominator + "/" + b.Numerator + "/" + b.Denominator + "=" + f.Numerator + "/" + f.Denominator);





