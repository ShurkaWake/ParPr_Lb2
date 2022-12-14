using System.Diagnostics;

namespace ParPr_Lb2;

internal static class Utils
{
   public static int[] GetArray(int length, int lowerInclusive = -128, int upperExclusive = 128)
    {
        int[] array = new int[length];
        for(int i = 0; i < length; i++)
        {
            array[i] = RandomNumberGenerator.GetInt32(
                lowerInclusive, upperExclusive);
        }
        return array;
    }

    public static void PrintArray(int[] array)
    {
        StringBuilder sb = new StringBuilder("[");
        foreach(var elem in array[..^1])
        {
            sb.Append(elem.ToString());
            sb.Append("; ");
        }
        sb.Append($"{array[^1]}]");
        Console.WriteLine(sb.ToString());
    }

    public static void PrintArray(double[] array)
    {
        StringBuilder sb = new StringBuilder("[");
        foreach (var elem in array[..^1])
        {
            sb.Append(elem.ToString("F2"));
            sb.Append("; ");
        }
        sb.Append($"{array[^1]:F2}]");
        Console.WriteLine(sb.ToString());
    }

    public static bool Prediction(byte address, byte[] binaryFlags, byte[] history)
    {
        return binaryFlags[((address & 0x03) << 4) + (history[address & 0x0F] & 0x0F)] > 1;
    }

    public static void SetFlags(
        bool isOk, 
        byte address, 
        ref byte[] binaryFlags, 
        ref byte[] history)
    {
        byte historyIndex = (byte) (address & 0x03);
        byte index = (byte) (((address & 0x03) << 4) + (history[historyIndex] & 0x0F));
        history[historyIndex] <<= 1;

        if (isOk)
        {
            if (binaryFlags[index] < 3)
            {
                binaryFlags[index]++;
            }
            history[historyIndex]++;
        }
        else if (binaryFlags[index] > 1) 
        {
            binaryFlags[index]--;
        }
    }

    public static long GetTime(Action act)
    {
        long start = Stopwatch.GetTimestamp();
        act.Invoke();
        long end = Stopwatch.GetTimestamp();
        return end - start;
    }
}
