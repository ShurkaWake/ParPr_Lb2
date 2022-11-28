namespace ParPr_Lb2;

public static class Task1
{
    public static void Show()
    {   
        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task1 ~#~#~#~#~");
        Console.WriteLine();

        ShowWorking();
        Console.WriteLine();
        ShowTime();
        Console.WriteLine();
        ShowPrediction();
        

        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task1 ~#~#~#~#~");
        Console.WriteLine();
    }

    private static void ShowWorking()
    {
        int arrayLength = 1 << 4;

        var arr = Utils.GetArray(arrayLength);
        Console.WriteLine("Array to process:");
        Utils.PrintArray(arr);
        Console.WriteLine($"Not optimized counted positive: {GetPositiveNumbersInArray(arr)}");
        Console.WriteLine($"Optimized counted positive: {GetPositiveNumbersInArrayOptimized(arr)}");
        Console.WriteLine($"Not optimized with predictor counted positive: {GetPositiveNumbersInArrayOptimized(arr)}");

    }

    private static void ShowTime()
    {
        int arrayLength = 1 << 22;

        var arr = Utils.GetArray(arrayLength);

        var notOptTime = Utils.GetTime(() => GetPositiveNumbersInArray(arr))
            / (double)TimeSpan.TicksPerMillisecond;
        var optTime = Utils.GetTime(() => GetPositiveNumbersInArrayOptimized(arr))
            / (double)TimeSpan.TicksPerMillisecond;

        Console.WriteLine($"Length: {arrayLength} " +
            $"| Not optimized time: {notOptTime:F4} ms " +
            $"| Optimized time {optTime:F4} ms");
    }

    private static void ShowPrediction()
    {
        int arrayLength = 1 << 22;
        var arr = Utils.GetArray(arrayLength);
        var predictionsFaults = GetPositiveNumbersWithPredictor(arr);
        Console.WriteLine($"Length: {arrayLength} " +
            $"| Prediction success ratio: {(1 - predictionsFaults) * 100:F2}%");
    }

    private static int GetPositiveNumbersInArray(int[] arr)
    {
        int counter = 0;
        foreach(var number in arr)
        {
            if (number > 0)
            {
                counter++;
            }
        }
        return counter;
    }

    private static int GetPositiveNumbersInArrayOptimized(int[] arr)
    {
        int counter = 0;
        foreach (var number in arr)
        {
            counter += number >> (sizeof(int) * 8 - 1);
        }
        return counter + arr.Length;
    }

    private static double GetPositiveNumbersWithPredictor(int[] arr)
    {
        int result = 0;
        int faults = 0;
        int total = 0;

        byte cycleAddress = 0;
        byte ifAdress = 1;
        byte[] binaryFlags = new byte[64];
        byte[] history = new byte[16];

        int i = 0;
        while(true)
        {
            bool cycleActual = i >= arr.Length;
            bool cyclePrediction = Utils.Prediction(cycleAddress, binaryFlags, history);
            faults += cycleActual != cyclePrediction ? 1 : 0;
            total++;
            Utils.SetFlags(cycleActual, cycleAddress, ref binaryFlags, ref history);

            if (cycleActual)
            {
                break;
            }

            bool ifActual = arr[i] >= 0;
            bool ifPrediction = Utils.Prediction(ifAdress, binaryFlags, history);

            result += ifActual ? 1 : 0;
            faults += ifActual != ifPrediction ? 1 : 0;
            total++;
            Utils.SetFlags(ifActual, ifAdress, ref binaryFlags, ref history);
            
            i++;
        }

        return faults / (double) total;
    }
}
