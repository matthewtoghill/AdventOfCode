namespace AdventOfCode.Tools;

internal static class ArrayExtensions
{
    public static IEnumerable<T> SliceColumn<T>(this T[,] arr, int col)
    {
        for (int i = 0; i < arr.GetLength(0); i++)
            yield return arr[i, col];
    }

    public static IEnumerable<T> SliceRow<T>(this T[,] arr, int row)
    {
        for (int i = 0; i < arr.GetLength(1); i++)
            yield return arr[row, i];
    }

    public static IEnumerable<T> Flatten<T>(this T[,] arr)
    {
        for (int row = 0; row < arr.GetLength(0); row++)
            for (int col = 0; col < arr.GetLength(1); col++)
                yield return arr[row, col];
    }
}
