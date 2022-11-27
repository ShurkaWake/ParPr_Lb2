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
}
