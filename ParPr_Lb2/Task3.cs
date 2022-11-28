namespace ParPr_Lb2;

public static class Task3
{
    public static void Show()
    {
        int arrayLength = 1 << 14;

        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task3 ~#~#~#~#~");
        Console.WriteLine();

        ShowWorking();
        Console.WriteLine();
        ShowTime();

        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task3 ~#~#~#~#~");
        Console.WriteLine();
    }

    private static void ShowWorking()
    {
        int arrayLength = 1 << 2;

        int[] arr1 = Utils.GetArray(arrayLength);
        int[] arr2 = Utils.GetArray(arrayLength, -1, 2);

        Console.WriteLine("First argument:");
        Utils.PrintArray(arr1);
        Console.WriteLine("Second argument:");
        Utils.PrintArray(arr2);

        int[] arrUnopt = PolynomMultiply(arr1, arr2);
        int[] arrOpt = PolynomMultiplyOpt(arr1, arr2);

        Console.WriteLine("Unoptimized method:");
        Utils.PrintArray(arrUnopt);
        Console.WriteLine("Optimized method:");
        Utils.PrintArray(arrOpt);
    }

    private static void ShowTime()
    {
        int arrayLength = 1 << 14;

        int[] arr1 = Utils.GetArray(arrayLength);
        int[] arr2 = Utils.GetArray(arrayLength, -1, 2);
        var notOptTime = Utils.GetTime(() => PolynomMultiply(arr1, arr2))
            / (double)TimeSpan.TicksPerMillisecond; ;
        var optTime = Utils.GetTime(() => PolynomMultiplyOpt(arr1, arr2))
            / (double)TimeSpan.TicksPerMillisecond; ;

        Console.WriteLine($"Length: {arrayLength} " +
            $"| Not optimized time: {notOptTime:F4} ms " +
            $"| Optimized time {optTime:F4} ms");
    }

    private static int[] PolynomMultiply(int[] x, int[] y)
    {
        int[] res = new int[x.Length + y.Length];
        for (int i = 0; i < x.Length; i++)
        {
            for (int j = 0; j < y.Length; j++)
            {
                res[i + j] += x[i] * y[j];
            }
        }
        return res;
    }

    private static int[] PolynomMultiplyOpt(int[] x, int[] y)
    {
        int[] res = new int[x.Length + y.Length];
        for(int j = 0; j < y.Length; j++)
        {
            if (y[j] != 0)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    res[i + j] += y[j] * x[i];
                }
            }
        }
        return res;
    }
}
