namespace ParPr_Lb2;

public static class Task1
{
    public static void Show()
    {
        int arrayLength = 1 << 20;

        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task1 ~#~#~#~#~");
        Console.WriteLine();

        var arr = Utils.GetArray(arrayLength);

        var notOptTime = Utils.GetTime(() => GetPositiveNumbersInArray(arr)) 
            / (double) TimeSpan.TicksPerMillisecond;
        var optTime = Utils.GetTime(() => GetPositiveNumbersInArrayOptimized(arr)) 
            / (double)TimeSpan.TicksPerMillisecond;

        Console.WriteLine($"Length: {arrayLength} " +
            $"| Not optimized time: {notOptTime:F4} ms " +
            $"| Optimized time {optTime:F4} ms");

        var predictionsFaults = GetPositiveNumbersWithPredictor(arr);
        Console.WriteLine($"Length: {arrayLength} " +
            $"| Predictions faults: {predictionsFaults} ");

        Console.WriteLine();
        Console.WriteLine("~#~#~#~#~ Task1 ~#~#~#~#~");
        Console.WriteLine();
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

    private static int GetPositiveNumbersWithPredictor(int[] arr)
    {
        int counter = 0;
        int faults = 0;
        byte address = (byte)RandomNumberGenerator.GetInt32(0, 256);
        byte[] binaryFlags = new byte[64];
        byte[] history = new byte[16];

        foreach (var number in arr)
        {
            bool actual = number >= 0;
            bool expected = Utils.Prediction(address, binaryFlags, history);

            counter += actual ? 1 : 0;
            faults += actual != expected ? 1 : 0;
            Utils.SetFlags(actual, address, ref binaryFlags, ref history);
        }

        return faults;
    }
}
