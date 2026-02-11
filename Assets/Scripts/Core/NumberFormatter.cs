/// <summary>
/// Утилита для форматирования больших чисел в компактный вид (12.5K, 3.1M и т.д.).
/// </summary>
public static class NumberFormatter
{
    /// <summary>
    /// Форматирует число в компактный вид.
    /// </summary>
    public static string Format(long value)
    {
        if (value >= 1000000000)
            return (value / 1000000000f).ToString("0.0") + "B";
        if (value >= 1000000)
            return (value / 1000000f).ToString("0.0") + "M";
        if (value >= 1000)
            return (value / 1000f).ToString("0.0") + "K";
        return value.ToString();
    }

    /// <summary>
    /// Форматирует float число в компактный вид.
    /// </summary>
    public static string Format(float value)
    {
        if (value >= 1000000000f)
            return (value / 1000000000f).ToString("0.0") + "B";
        if (value >= 1000000f)
            return (value / 1000000f).ToString("0.0") + "M";
        if (value >= 1000f)
            return (value / 1000f).ToString("0.0") + "K";
        return value.ToString("0.0");
    }
}
