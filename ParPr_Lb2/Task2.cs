namespace ParPr_Lb2;

public static class Task2
{
    public static void Show()
    {
        int arrayLength = 1 << 14;

        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task2 ~#~#~#~#~");
        Console.WriteLine();

        ShowWorking();
        Console.WriteLine();
        ShowTime();
        Console.WriteLine();
        ShowPrediction();

        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task2 ~#~#~#~#~");
        Console.WriteLine();
    }

    private static void ShowWorking()
    {
        int arrayLength = 1 << 3;
        var arrOrig = Utils.GetArray(arrayLength);

        var arrUnopt = arrOrig.Clone() as int[];
        BubbleSort(ref arrUnopt);
        var arrOpt = arrOrig.Clone() as int[];
        BubbleSortOptimized(ref arrOpt);
        var arrPred = arrOrig.Clone() as int[];
        BubbleSortPredicted(ref arrPred);

        Console.WriteLine("Array to sort:");
        Utils.PrintArray(arrOrig);
        Console.WriteLine("Unoptimized method:");
        Utils.PrintArray(arrUnopt);
        Console.WriteLine("Optimized method:");
        Utils.PrintArray(arrOpt);
        Console.WriteLine("Method with predictor:");
        Utils.PrintArray(arrPred);
    }

    private static void ShowTime()
    {
        int arrayLength = 1 << 14;

        int[] arr1 = Utils.GetArray(arrayLength, int.MinValue, int.MaxValue);
        int[] arr2 = arr1.Clone() as int[];
        var notOptTime = Utils.GetTime(() => BubbleSort(ref arr1))
            / (double)TimeSpan.TicksPerMillisecond; ;
        var optTime = Utils.GetTime(() => BubbleSortOptimized(ref arr2))
            / (double)TimeSpan.TicksPerMillisecond; ;

        Console.WriteLine($"Length: {arrayLength} " +
            $"| Not optimized time: {notOptTime:F4} ms " +
            $"| Optimized time {optTime:F4} ms");
    }

    private static void ShowPrediction()
    {
        int arrayLength = 1 << 14;

        int[] arr3 = Utils.GetArray(arrayLength, int.MinValue, int.MaxValue);
        var predictionFaults = BubbleSortPredicted(ref arr3);
        Console.WriteLine($"Length: {arrayLength} " +
            $"| Prediction success ratio: {(1 - predictionFaults) * 100:F2}%");
    }

    private static void BubbleSort(ref int[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = i + 1; j < arr.Length; j++) 
            { 
                if (arr[i] > arr[j])
                {
                    int temp = arr[j];
                    arr[j] = arr[i];
                    arr[i] = temp;
                }
            }
        }
    }

    private static void BubbleSortOptimized(ref int[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            for (int j = i - 1; j > 0; j--)
            {
                if (arr[i] < arr[j])
                {
                    int temp = arr[j];
                    arr[j] = arr[i];
                    arr[i] = temp;
                }
            }
        }
    }

    private static double BubbleSortPredicted(ref int[] arr)
    {
        byte firstCycleAddress = 0;
        byte secondCycleAddress = 1;
        byte ifAdress = 2;
        byte[] binaryFlags = new byte[64];
        byte[] history = new byte[16];

        int faults = 0;
        int total = 0;

        int i = 0;
        while (true)
        {
            bool firstCycleActual = i >= arr.Length;
            bool firstCyclePredict = Utils.Prediction(
                firstCycleAddress, 
                binaryFlags, 
                history);

            faults += firstCycleActual != firstCyclePredict ? 1 : 0;
            total++;
            Utils.SetFlags(firstCycleActual, firstCycleAddress,
                ref binaryFlags, ref history);

            if (firstCycleActual)
            {
                break;
            }

            int j = i + 1;
            while (true)
            {
                bool secondCycleActual = j >= arr.Length;
                bool secondCyclePredict = Utils.Prediction(
                    secondCycleAddress,
                    binaryFlags,
                    history
                    );

                faults += secondCycleActual != secondCyclePredict ? 1 : 0;
                total++;
                Utils.SetFlags(secondCycleActual, secondCycleAddress,
                    ref binaryFlags, ref history);

                if (secondCycleActual)
                {
                    break;
                }

                bool ifActual = arr[i] > arr[j];
                bool ifPredict = Utils.Prediction(ifAdress, binaryFlags, history);
                faults += ifActual != ifPredict ? 1 : 0;
                total++;
                Utils.SetFlags(ifActual, ifAdress, ref binaryFlags, ref history);

                if (ifActual)
                {
                    int temp = arr[j];
                    arr[j] = arr[i];
                    arr[i] = temp;
                }

                j++;
            }

            i++;
        }

        return faults / (double) total;
    }
}