namespace ParPr_Lb2;

internal static class Utils
{
    const int upperGenBound = 128;
    const int lowerGenBound = -128;

    public static int[] GetArray(int length)
    {
        int[] array = new int[length];
        for(int i = 0; i < length; i++)
        {
            array[i] = RandomNumberGenerator.GetInt32(
                lowerGenBound, upperGenBound);
        }
        return array;
    }

    public static void PrintArray(int[] array)
    {
        StringBuilder sb = new StringBuilder("[");
        foreach(var elem in array[..^1])
        {
            sb.Append(elem.ToString());
            sb.Append(", ");
        }
        sb.Append($"{array[^1]}]");
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
        byte historyIndex = (byte) (address & 0x0F);
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
}
