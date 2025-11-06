using System;
using System.Globalization;

public class WeekHelper
{
    public  (int weekNumber, DateTime startDate, DateTime endDate) GetCurrentWeekInfo()
    {
        // Lấy ngày hiện tại
        DateTime today = DateTime.Today;

        // Lấy số tuần theo chuẩn ISO 8601
        int weekNumber = ISOWeek.GetWeekOfYear(today);

        // Lấy ngày đầu tuần (Thứ Hai)
        DateTime startDate = ISOWeek.ToDateTime(today.Year, weekNumber, DayOfWeek.Monday);

        // Lấy ngày cuối tuần (Chủ Nhật)
        DateTime endDate = startDate.AddDays(6);

        return (weekNumber, startDate, endDate);
    }

    //public static void Main(string[] args)
    //{
    //    var (week, start, end) = GetCurrentWeekInfo();

    //    Console.WriteLine($"Ngày hiện tại: {DateTime.Today.ToShortDateString()}");
    //    Console.WriteLine($"Tuần số: {week}");
    //    Console.WriteLine($"Ngày bắt đầu: {start.ToShortDateString()}");
    //    Console.WriteLine($"Ngày kết thúc: {end.ToShortDateString()}");
    //}
}