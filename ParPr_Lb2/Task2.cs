namespace ParPr_Lb2;

public static class Task2
{
    public static void Show()
    {
        int arrayLength = 1 << 10;

        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task2 ~#~#~#~#~");
        Console.WriteLine();

        int[] arr1 = Utils.GetArray(arrayLength);
        int[] arr2 = arr1.Clone() as int[];
        var notOptTime = Utils.GetTime(() => BubbleSort(ref arr1))
            / (double)TimeSpan.TicksPerMillisecond; ;
        var optTime = Utils.GetTime(() => BubbleSort(ref arr2))
            / (double)TimeSpan.TicksPerMillisecond; ;

        Console.WriteLine($"Length: {arrayLength} " +
            $"| Not optimized time: {notOptTime:F4} ms " +
            $"| Optimized time {optTime:F4} ms");

        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task2 ~#~#~#~#~");
        Console.WriteLine();
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

    private static void BubbleSortOtpimized(ref int[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            bool flag = false;
            for (int j = i + 1; j < arr.Length; j++)
            {
                if (arr[i] > arr[j])
                {
                    int temp = arr[j];
                    arr[j] = arr[i];
                    arr[i] = temp;
                    flag = true;
                }
            }

            if (!flag)
            {
                break;
            }
        }
    }
}