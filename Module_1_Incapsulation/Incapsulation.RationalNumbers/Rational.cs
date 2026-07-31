using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Incapsulation.RationalNumbers;

public struct Rational
{
    public readonly int Numerator;
    public readonly int Denominator;

    public Rational(int numerator)
    {
        Numerator = numerator;
        Denominator = 1;
    }

    public Rational(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
        if (Denominator == 0)
            return;

        var nod = Nod(numerator, denominator);
        Denominator /= nod;
        Numerator /= nod;
        if (Denominator < 0)
        {
            Denominator *= -1;
            Numerator *= -1;
        }
    }

    public static Rational operator -(Rational operand) => new(-operand.Numerator, operand.Denominator);

    public static Rational operator +(Rational left, Rational right)
    {
        if (left.IsNan || right.IsNan)
            return new Rational(0, 0);
        return new(left.Numerator * right.Denominator + right.Numerator *
            left.Denominator, left.Denominator * right.Denominator);
    }

    public static Rational operator -(Rational left, Rational right)
    {
        if (left.IsNan || right.IsNan)
            return new Rational(0, 0);
        return left + (-right);
    }

    public static Rational operator *(Rational left, Rational right)
    {
        if (left.IsNan || right.IsNan)
            return new Rational(0, 0);
        return new(left.Numerator * right.Numerator, left.Denominator * right.Denominator);
    }

    public static Rational operator /(Rational left, Rational right)
    {
        if (left.IsNan || right.IsNan || right.Numerator == 0)
            return new Rational(0, 0);
        return new(left.Numerator * right.Denominator, left.Denominator * right.Numerator);
    }

    public static implicit operator Rational(int value)
    {
        return new Rational(value);
    }

    public static implicit operator double(Rational value)
    {
        if (value.IsNan) return double.NaN;
        return (double)value.Numerator / value.Denominator;
    }

    public static explicit operator int(Rational value)
    {
        if (value.IsNan)
            throw new InvalidOperationException("Cannot convert NaN to int");
        if (value.Numerator % value.Denominator != 0)
            throw new InvalidOperationException("Cannot convert non-integer rational to int");
        return value.Numerator / value.Denominator;
    }

    public readonly bool IsNan => Denominator == 0;

    private static int Nod(int numerator, int denominator)
    {
        if (denominator == 0) return 1;
        if (numerator == 0) return denominator;
        var n = Math.Abs(numerator);
        var d = Math.Abs(denominator);
        while (d != 0)
        {
            var temp = d;
            d = n % d;
            n = temp;
        }
        return n;
    }
}