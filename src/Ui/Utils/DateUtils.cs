namespace PassRegulaParser.Ui.Utils;

public static class DateUtils
{
    public static DateTime CalculateEndDate(string period)
    {
        DateTime today = DateTime.Today;
        return period switch
        {
            "1 день" => today,
            "1 месяц" => today.AddMonths(1),
            "1 год" => today.AddYears(1),
            "10 лет" => today.AddYears(10),
            _ => today,
        };
    }
}