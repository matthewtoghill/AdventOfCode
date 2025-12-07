namespace AdventOfCode.Tools;

internal static class GenericExtensions
{
    public static TResult Map<TSource, TResult>(this TSource source, Func<TSource, TResult> selector)
    {
        return selector(source);
    }
}
