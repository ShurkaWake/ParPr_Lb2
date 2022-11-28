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
        ShowTime();
        Console.WriteLine();
        ShowPrediction();


        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task4 ~#~#~#~#~");
        Console.WriteLine();
    }

    private static void CheckWorking()
    {
        int arrayLength = 1 << 3;

        double[] arr1 = Utils.GetArray(arrayLength).
            Select(elem => elem / (double)RandomNumberGenerator.GetInt32(-16, 17)).
            ToArray();

        Console.WriteLine("Array to round:");
        Utils.PrintArray(arr1);
        Console.WriteLine("Unoptimized function");
        Utils.PrintArray(RoundArray(arr1));
        Console.WriteLine("Optimized function");
        Utils.PrintArray(RoundArrayOptimized(arr1));
        Console.WriteLine("Unoptimized with prediction function");
        Utils.PrintArray(RoundArrayPredictor(arr1).result);
    }

    private static void ShowTime()
    {
        int arrayLength = 1 << 22;

        double[] arr = Utils.GetArray(arrayLength).Select(elem => elem / (double) RandomNumberGenerator.GetInt32(-32, 32)).ToArray();
        var notOptTime = Utils.GetTime(() => RoundArray(arr))
            / (double)TimeSpan.TicksPerMillisecond; ;
        var optTime = Utils.GetTime(() => RoundArrayOptimized(arr))
            / (double)TimeSpan.TicksPerMillisecond; ;

        Console.WriteLine($"Length: {arrayLength} " +
            $"| Not optimized time: {notOptTime:F4} ms " +
            $"| Optimized time {optTime:F4} ms");
    }

    private static void ShowPrediction()
    {
        int arrayLength = 1 << 22;

        double[] arr = Utils.GetArray(arrayLength).Select(elem => elem / (double)RandomNumberGenerator.GetInt32(-32, 32)).ToArray();
        var predictionFaults = RoundArrayPredictor(arr).faultRate;
        Console.WriteLine($"Length: {arrayLength} " +
            $"| Prediction success ratio: {(1 - predictionFaults) * 100:F2}%");
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

    private static (double[] result, double faultRate) RoundArrayPredictor(double[] array)
    {
        byte cycleAdress = 0;
        byte ifAdress = 1;
        byte[] binaryFlags = new byte[64];
        byte[] history = new byte[16];
        int faults = 0;
        int total = 0;

        double[] result = new double[array.Length];

        int i = 0;
        while (true)
        {
            bool cycleActual = i >= array.Length;
            bool cyclePredict = Utils.Prediction(cycleAdress, binaryFlags, history);
            faults += cycleActual != cyclePredict ? 1 : 0;
            total++;
            Utils.SetFlags(cycleActual, cycleAdress, ref binaryFlags, ref history);

            if (cycleActual)
            {
                break;
            }

            bool ifActual = array[i] < 0;
            bool ifPrediction = Utils.Prediction(ifAdress, binaryFlags, history);
            faults += ifActual != ifPrediction ? 1 : 0;
            total++;
            Utils.SetFlags(ifActual, ifAdress, ref binaryFlags, ref history);

            if (ifActual)
            {
                result[i] = (long)(array[i] - 0.5);
            }
            else
            {
                result[i] = (long)(array[i] + 0.5);
            }

            i++;
        }

        return (result, faults / (double)total);
    }
}
