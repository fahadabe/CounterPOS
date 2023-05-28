namespace CounterPOS;

public static class TOC
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
    {
        return new ObservableCollection<T>(source);
    }

    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
    {
        return source ?? Enumerable.Empty<T>();
    }

    public static bool ReturnSuccess<TI>(this TI input) where TI : class
    {
        return input != null;
    }

    public static ObservableCollection<T> Empty<T>()
    {
        return new ObservableCollection<T>();
    }
}