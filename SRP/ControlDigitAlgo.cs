namespace SRP.ControlDigit;

public static class Extensions
{
    public static IEnumerable<int> GetDigitsFromRight(this long number)
    {
        do
        {
            yield return (int)(number % 10);
            number /= 10;
        }
        while (number > 0);
    }

    public static int Sum(this IEnumerable<int> source, Func<int, int, int> selector)
    {
        int sum = 0;
        int index = 0;
        foreach (var item in source)
        {
            sum += selector(item, index);
            index++;
        }
        return sum;
    }

    public static int GetReminder(this int number, int divisor)
    {
        return (divisor - number % divisor) % divisor;
    }

    public static char ToChar(this int number, int selector)
    {
        return number == selector ? 'X' : (char)('0' + number);
    }
}

public static class ControlDigitAlgo
{
    public static int Upc(long number)
    {
        return number
            .GetDigitsFromRight()
            .Take(11)
            .Sum((digit, index) => digit * (index % 2 == 0 ? 3 : 1))
            .GetReminder(10);
    }

    public static char Isbn10(long number)
    {
        return number
            .GetDigitsFromRight()
            .Sum((digit, index) => digit * (index + 2))
            .GetReminder(11)
            .ToChar(10);
    }

    public static int Luhn(long number)
    {
        return number.GetDigitsFromRight().Sum((digit, index) =>
        {
            if (index % 2 == 0)
            {
                digit *= 2;
                if (digit > 9) digit -= 9;
            }
            return digit;
        }).GetReminder(10);
    }
}