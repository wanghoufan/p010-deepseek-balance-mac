using System;
using System.Collections.Generic;
using System.Linq;
using DeepSeekBalanceWidget.Models;

namespace DeepSeekBalanceWidget.Services;

/// <summary>
/// 高峰时段判断。官方口径（api-docs.deepseek.com/quick_start/pricing，2026-10-20 核对）：
/// Peak hours 为 UTC 周一至周五 01:00-04:00、06:00-10:00，折北京时间 09:00-12:00、14:00-18:00；
/// 其余时段（含周末与中国法定节假日全天）为谷时，谷时按半价计费。
/// 法定节假日以 <see cref="ChinaHolidayCalendar"/> 内建的放假日期为准；表外年份自动回退为
/// 「只按周一至周五」判定。统一以北京时间（UTC+8）判断；输入时间（Kind=Utc 或 Local）内部转换到北京时间。
/// </summary>
public static class PeakHourCalculator
{
    /// <summary>官方默认区间（半开 [Start, End)，支持 End=24 表达次日 0 点）。周末全天非高峰。</summary>
    public static readonly IReadOnlyList<PeakRange> DefaultRanges = new[]
    {
        new PeakRange(9, 12, WeekdaysOnly: true),
        new PeakRange(14, 18, WeekdaysOnly: true)
    };

    private static readonly TimeZoneInfo BeijingTz =
        TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");

    /// <summary>把给定时间转换到北京时间（输入 Kind=Utc 则按 UTC 转换，Local/Unspecified 按本地转换）。</summary>
    public static DateTime ToBeijing(DateTime localNow)
        => TimeZoneInfo.ConvertTime(localNow, BeijingTz);

    /// <summary>判断给定时间是否处于高峰（北京时间视角）。区间为空/非法时回退官方默认。</summary>
    public static bool IsPeak(DateTime localNow, IReadOnlyList<PeakRange>? ranges)
    {
        var list = Normalize(ranges);
        var bj = ToBeijing(localNow);
        return IsPeakAt(bj, list);
    }

    /// <summary>给定时间（转换为北京时间后）是否为法定节假日。用于界面提示与取证日志。</summary>
    public static bool IsHoliday(DateTime localNow)
        => ChinaHolidayCalendar.IsHoliday(ToBeijing(localNow));

    /// <summary>距下一个高峰状态边界的分钟数（1~1440），供一次性定时器精确对齐。</summary>
    public static int MinutesUntilNextBoundary(DateTime localNow, IReadOnlyList<PeakRange>? ranges)
    {
        var list = Normalize(ranges);
        var bj = ToBeijing(localNow);
        bool cur = IsPeakAt(bj, list);
        for (int m = 1; m <= 24 * 60; m++)
        {
            if (IsPeakAt(bj.AddMinutes(m), list) != cur) return m;
        }
        return 24 * 60;
    }

    /// <summary>
    /// 把区间列表描述成界面可读文案（北京时间），例如
    /// “周一至周五 09:00–12:00、14:00–18:00”。
    /// </summary>
    public static string Describe(IReadOnlyList<PeakRange>? ranges)
    {
        var list = Normalize(ranges);
        var weekdays = string.Join("、", list.Where(r => r.WeekdaysOnly).Select(FormatRange));
        var everyday = string.Join("、", list.Where(r => !r.WeekdaysOnly).Select(FormatRange));
        return (weekdays, everyday) switch
        {
            (_, "") => $"周一至周五 {weekdays}",
            ("", _) => $"每天 {everyday}",
            _ => $"周一至周五 {weekdays}；每天 {everyday}"
        };
    }

    /// <summary>
    /// 下一次真正的峰谷切换时刻（北京时间，供界面展示）。周五收盘后要等到下周一，
    /// 遇到春节、国庆这类长假跨度更长，所以最多向后找 15 天；区间恒定不变时返回 null。
    /// </summary>
    public static DateTime? NextBoundary(DateTime localNow, IReadOnlyList<PeakRange>? ranges)
    {
        var list = Normalize(ranges);
        var bj = ToBeijing(localNow);
        bool cur = IsPeakAt(bj, list);
        const int limit = 15 * 24 * 60;
        for (int m = 1; m <= limit; m++)
        {
            if (IsPeakAt(bj.AddMinutes(m), list) != cur) return bj.AddMinutes(m);
        }
        return null;
    }

    /// <summary>下一次「高峰↔谷时」切换的时刻（北京时间），如“12:00”“明天 09:00”。</summary>
    public static string DescribeNextBoundary(DateTime localNow, IReadOnlyList<PeakRange>? ranges)
    {
        var bjNow = ToBeijing(localNow);
        if (NextBoundary(localNow, ranges) is not { } next) return "无（区间恒定）";
        return next.Date == bjNow.Date ? next.ToString("HH:mm")
            : next.Date == bjNow.Date.AddDays(1) ? $"明天 {next:HH:mm}"
            : next.ToString("M月d日 HH:mm");
    }

    private static string FormatRange(PeakRange r) => $"{r.StartHour:00}:00–{r.EndHour:00}:00";

    /// <summary>
    /// 区间命中判断。WeekdaysOnly 表示「工作日」：周末与法定节假日都算非工作日，
    /// 因此节假日只压制 WeekdaysOnly 区间；用户自定义的「每天」区间保持原样，不被日历改写。
    /// </summary>
    private static bool IsPeakAt(DateTime bj, IReadOnlyList<PeakRange> ranges)
        => ranges.Any(r => r.Contains(bj.Hour, bj.DayOfWeek)
                           && !(r.WeekdaysOnly && ChinaHolidayCalendar.IsHoliday(bj)));

    /// <summary>校验区间列表：非空、每段 Start 0-23、End 1-24、Start&lt;End。非法返回官方默认。</summary>
    private static IReadOnlyList<PeakRange> Normalize(IReadOnlyList<PeakRange>? ranges)
    {
        if (ranges is not { Count: > 0 }) return DefaultRanges;
        foreach (var r in ranges)
        {
            if (r.StartHour < 0 || r.StartHour > 23) return DefaultRanges;
            if (r.EndHour < 1 || r.EndHour > 24) return DefaultRanges;
            if (r.StartHour >= r.EndHour) return DefaultRanges; // 不支持跨午夜
        }
        return ranges;
    }
}
