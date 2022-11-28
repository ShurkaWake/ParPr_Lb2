namespace ParPr_Lb2;

public static class Task4
{
    public static void Show()
    {
        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task4 ~#~#~#~#~");
        Console.WriteLine();

        CheckWorking();


        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task4 ~#~#~#~#~");
        Console.WriteLine();
    }

    private static void CheckWorking()
    {
        int arrayLength = 1 << 3;

        double[] arr1 = Utils.GetArray(arrayLength).
            Select(elem => elem / (double)RandomNumberGenerator.GetInt32(-128, 128)).
            ToArray();

        Console.WriteLine("Array to round:");
        Utils.PrintArray(arr1);
        Console.WriteLine("Unoptimized function");
        Utils.PrintArray(RoundArray(arr1));
        Console.WriteLine("Optimized function");
        Utils.PrintArray(RoundArrayOptimized(arr1));
    }

    private static double[] RoundArray(double[] array)
    {
        double[] result = new double[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] < 0)
            {
                result[i] = (long)(array[i] - 0.5); 
            }
            else
            {
                result[i] = (long)(array[i] + 0.5);
            }
        }

        return result;
    }

    private static double[] RoundArrayOptimized(double[] array)
    {
        double[] result = new double[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            result[i] = (long) (array[i] + ((((long) (array[i] - 1)) >> 63)) + 0.5);
        }

        return result;
    }
}
