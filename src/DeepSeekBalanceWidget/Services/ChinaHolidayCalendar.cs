using System;
using System.Collections.Generic;

namespace DeepSeekBalanceWidget.Services;

/// <summary>
/// 中国法定节假日（放假日期）日历，供 DeepSeek 峰谷判定使用。
/// 数据来源：国务院办公厅历年《关于部分节假日安排的通知》
/// —— 2025 年（国办发明电〔2024〕7 号）、2026 年（国办发明电〔2025〕7 号）。
/// 只登记「放假」日期：DeepSeek 官方口径是 UTC 周一至周五、不含中国法定节假日，
/// 调休补班的周六周日仍属周末，本来就按谷时计，因此无需维护补班表。
/// 次年的放假安排通常每年 11 月发布，未覆盖年份自动回退为「只按周一至周五」判定，
/// 调用方应通过 <see cref="IsCovered"/> 判断是否需要提示用户更新。
/// </summary>
public static class ChinaHolidayCalendar
{
    /// <summary>已内建的最早年份。</summary>
    public const int FirstCoveredYear = 2025;

    /// <summary>已内建的最晚年份；次年安排公布后需补数据并抬高此值。</summary>
    public const int LastCoveredYear = 2026;

    private static readonly HashSet<DateTime> Holidays = BuildHolidays();

    /// <summary>该年份的放假安排是否已内建。</summary>
    public static bool IsCovered(int year) => year >= FirstCoveredYear && year <= LastCoveredYear;

    /// <summary>给定北京时间是否落在法定节假日（放假）当天。未覆盖年份恒为 false。</summary>
    public static bool IsHoliday(DateTime beijingTime) => Holidays.Contains(beijingTime.Date);

    /// <summary>已内建范围的界面文案，如“2025–2026 年法定节假日”。</summary>
    public static string DescribeCoverage() => $"{FirstCoveredYear}–{LastCoveredYear} 年法定节假日";

    /// <summary>某年份未内建时的提示文案。</summary>
    public static string DescribePending(int year) => year > LastCoveredYear
        ? $"{year} 年放假安排国务院尚未公布（通常每年 11 月发布），该年按周一至周五判定"
        : $"{year} 年节假日日历未内建，该年按周一至周五判定";

    private static HashSet<DateTime> BuildHolidays()
    {
        var set = new HashSet<DateTime>();

        void Range(int year, int month, int day, int endMonth, int endDay)
        {
            var end = new DateTime(year, endMonth, endDay);
            for (var d = new DateTime(year, month, day); d <= end; d = d.AddDays(1))
                set.Add(d);
        }

        // 2025（国办发明电〔2024〕7 号）
        set.Add(new DateTime(2025, 1, 1));                 // 元旦
        Range(2025, 1, 28, 2, 4);                          // 春节
        Range(2025, 4, 4, 4, 6);                           // 清明
        Range(2025, 5, 1, 5, 5);                           // 劳动节
        Range(2025, 5, 31, 6, 2);                          // 端午
        Range(2025, 10, 1, 10, 8);                         // 国庆、中秋

        // 2026（国办发明电〔2025〕7 号）
        Range(2026, 1, 1, 1, 3);                           // 元旦
        Range(2026, 2, 15, 2, 23);                         // 春节
        Range(2026, 4, 4, 4, 6);                           // 清明
        Range(2026, 5, 1, 5, 5);                           // 劳动节
        Range(2026, 6, 19, 6, 21);                         // 端午
        Range(2026, 9, 25, 9, 27);                         // 中秋
        Range(2026, 10, 1, 10, 7);                         // 国庆

        return set;
    }
}
