using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WebApp.Charts;
using WebApp.Domain.Dashboard;
using WebApp.Services;

namespace WebApp.Pages;

public partial class Dashboard : IDisposable
{
    [Inject] private ThemeService ThemeService { get; set; } = default!;

    private bool _isDarkMode;
    [Parameter]
    public string PageTitleText { get; set; } = "Dashboard";

    [Parameter]
    public bool RenderPageTitle { get; set; } = true;
    private static class Palette
    {
        public const string Primary = "#1A56DB";
        public const string Secondary = "#3B82F6";
        public const string AccentTeal = "#0EA5E9";
        public const string AccentCyan = "#38BDF8";
    }

    private IReadOnlyList<ChartSeriesDefinition<NumericPoint>> RevenueSeries { get; set; } = Array.Empty<ChartSeriesDefinition<NumericPoint>>();
    private IReadOnlyList<ChartSeriesDefinition<NumericPoint>> ProductQuantitySeries { get; set; } = Array.Empty<ChartSeriesDefinition<NumericPoint>>();
    private IReadOnlyList<ChartSeriesDefinition<NumericPoint>> UserSeries { get; set; } = Array.Empty<ChartSeriesDefinition<NumericPoint>>();
    private IReadOnlyList<ChartSeriesDefinition<NumericPoint>> CategorySalesSeries { get; set; } = Array.Empty<ChartSeriesDefinition<NumericPoint>>();
    private IReadOnlyList<NumericPoint> TrafficSlices { get; set; } = Array.Empty<NumericPoint>();
    private IReadOnlyList<DeviceSummary> Devices { get; set; } = Array.Empty<DeviceSummary>();
    private IReadOnlyList<ProductStatistic> TopProducts { get; set; } = Array.Empty<ProductStatistic>();
    private IReadOnlyList<CustomerStatistic> TopCustomers { get; set; } = Array.Empty<CustomerStatistic>();
    private IReadOnlyList<string> TransactionHeaders { get; set; } = Array.Empty<string>();
    private IReadOnlyList<TransactionEntry> Transactions { get; set; } = Array.Empty<TransactionEntry>();
    private List<StatusFilterOption> StatusFilters { get; set; } = new();
    private List<DateRangeOption> DateRangeOptions { get; set; } = new();
    private DateRangeOption? SelectedDateRange { get; set; } = default;

    protected override void OnInitialized()
    {
        BuildSeries();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isDarkMode = await ThemeService.InitializeAsync();
            ThemeService.ThemeChanged += HandleThemeChanged;
        }
    }

    private void BuildSeries()
    {
        RevenueSeries = new[]
        {
      new ChartSeriesDefinition<NumericPoint>(
        "Revenue",
        CreatePoints(DashboardChartOptions.MainChartCategoryLabels, new decimal[] { 6356m, 6218m, 6156m, 6526m, 6356m, 6256m, 6056m }),
        Palette.Primary),
      new ChartSeriesDefinition<NumericPoint>(
        "Revenue (previous period)",
        CreatePoints(DashboardChartOptions.MainChartCategoryLabels, new decimal[] { 6556m, 6725m, 6424m, 6356m, 6586m, 6756m, 6616m }),
        Palette.AccentCyan)
    };

        ProductQuantitySeries = new[]
        {
      new ChartSeriesDefinition<NumericPoint>(
        "Quantity",
        new List<NumericPoint>
        {
          new("01 Feb", 170m),
          new("02 Feb", 180m),
          new("03 Feb", 164m),
          new("04 Feb", 145m),
          new("05 Feb", 194m),
          new("06 Feb", 170m),
          new("07 Feb", 155m)
        },
        Palette.Primary)
    };

        var userValues = new decimal[] { 1334m, 2435m, 1753m, 1328m, 1155m, 1632m, 1336m };
        UserSeries = new[]
        {
      new ChartSeriesDefinition<NumericPoint>(
        "Users",
        CreatePoints(DashboardChartOptions.MainChartCategoryLabels, userValues))
    };

        CategorySalesSeries = new[]
        {
      new ChartSeriesDefinition<NumericPoint>(
        "Desktop PC",
        new List<NumericPoint>
        {
          new("01 Feb", 170m),
          new("02 Feb", 180m),
          new("03 Feb", 164m),
          new("04 Feb", 145m),
          new("05 Feb", 194m),
          new("06 Feb", 170m),
          new("07 Feb", 155m)
        },
        Palette.Primary),
      new ChartSeriesDefinition<NumericPoint>(
        "Phones",
        new List<NumericPoint>
        {
          new("01 Feb", 120m),
          new("02 Feb", 294m),
          new("03 Feb", 167m),
          new("04 Feb", 179m),
          new("05 Feb", 245m),
          new("06 Feb", 182m),
          new("07 Feb", 143m)
        },
        Palette.AccentCyan),
      new ChartSeriesDefinition<NumericPoint>(
        "Gaming / Console",
        new List<NumericPoint>
        {
          new("01 Feb", 220m),
          new("02 Feb", 194m),
          new("03 Feb", 217m),
          new("04 Feb", 279m),
          new("05 Feb", 215m),
          new("06 Feb", 263m),
          new("07 Feb", 183m)
        },
        Palette.AccentTeal)
    };

        TrafficSlices = new List<NumericPoint>
    {
      new("Desktop", 70m),
      new("Tablet", 5m),
      new("Phone", 25m)
    };

        Devices = new[]
        {
      new DeviceSummary("Desktop", "234k", "+4%", "text-emerald-500 dark:text-emerald-400"),
      new DeviceSummary("Phone", "94k", "-1%", "text-red-500 dark:text-red-400"),
      new DeviceSummary("Tablet", "16k", "-0.6%", "text-red-500 dark:text-red-400")
    };

        TopProducts = new[]
        {
      new ProductStatistic("iPhone 14 Pro", "/images/dashboard/products/iphone.png", 445_467m, 2.5m),
      new ProductStatistic("Apple iMac 27", "/images/dashboard/products/imac.png", 256_982m, 12.5m),
      new ProductStatistic("Apple Watch SE", "/images/dashboard/products/watch.png", 201_869m, -1.35m),
      new ProductStatistic("Apple iPad Air", "/images/dashboard/products/ipad.png", 103_967m, 12.5m),
      new ProductStatistic("Apple iMac 24", "/images/dashboard/products/imac.png", 98_543m, -2m)
    };

        TopCustomers = new[]
        {
      new CustomerStatistic("Neil Sims", "neil.sims@flowbite.com", "/images/dashboard/users/neil-sims.png", 5_660m),
      new CustomerStatistic("Roberta Casas", "roberta.casas@flowbite.com", "/images/dashboard/users/roberta-casas.png", 7_081m),
      new CustomerStatistic("Michael Gough", "michael.gough@flowbite.com", "/images/dashboard/users/michael-gough.png", 1_104m),
      new CustomerStatistic("Jese Leos", "jese.leos@flowbite.com", "/images/dashboard/users/jese-leos.png", 8_983m),
      new CustomerStatistic("Bonnie Green", "bonnie.green@flowbite.com", "/images/dashboard/users/bonnie-green.png", 832m)
    };

        TransactionHeaders = new[]
        {
      "Transaction",
      "Date & Time",
      "Amount",
      "Reference number",
      "Payment method",
      "Status"
    };

        Transactions = new[]
        {
      new TransactionEntry("Payment from Bonnie Green", "Apr 23, 2021", "$2300", "0047568936", 475, PaymentBrand.Visa, TransactionStatus.Completed),
      new TransactionEntry("Payment refund to #00910", "Apr 23, 2021", "-$670", "0078568936", 924, PaymentBrand.Mastercard, TransactionStatus.Completed),
      new TransactionEntry("Payment failed from #087651", "Apr 18, 2021", "$234", "0088568934", 826, PaymentBrand.Mastercard, TransactionStatus.Cancelled),
      new TransactionEntry("Payment from Lana Byrd", "Apr 15, 2021", "$5000", "0018568911", 634, PaymentBrand.Mastercard, TransactionStatus.InProgress),
      new TransactionEntry("Payment from Jese Leos", "Apr 15, 2021", "$2300", "0045568939", 163, PaymentBrand.Visa, TransactionStatus.Completed),
      new TransactionEntry("Refund to THEMESBERG LLC", "Apr 11, 2021", "-$560", "0031568935", 443, PaymentBrand.Visa, TransactionStatus.InReview),
      new TransactionEntry("Payment from Lana Lysle", "Apr 6, 2021", "$1437", "0023568934", 223, PaymentBrand.Visa, TransactionStatus.InReview),
      new TransactionEntry("Payment to Joseph Mcfall", "Apr 1, 2021", "$980", "0057568935", 362, PaymentBrand.Mastercard, TransactionStatus.Completed),
      new TransactionEntry("Payment from Alphabet", "Mar 23, 2021", "$11,436", "00836143841", 772, PaymentBrand.Mastercard, TransactionStatus.InProgress),
      new TransactionEntry("Payment from Bonnie Green", "Mar 23, 2021", "$560", "0031568935", 123, PaymentBrand.Visa, TransactionStatus.Completed)
    };

        StatusFilters = new List<StatusFilterOption>
    {
      new("Completed", TransactionStatus.Completed, false, 56),
      new("Cancelled", TransactionStatus.Cancelled, true, 56),
      new("In progress", TransactionStatus.InProgress, false, 56),
      new("In review", TransactionStatus.InReview, true, 97)
    };

        DateRangeOptions = new List<DateRangeOption>
    {
      new("Yesterday", -1),
      new("Today", 0),
      new("Last 7 days", 7),
      new("Last 30 days", 30),
      new("Last 90 days", 90)
    };
        SelectedDateRange = DateRangeOptions.FirstOrDefault(option => option.Label == "Last 7 days") ?? DateRangeOptions.First();
    }

    private static IReadOnlyList<NumericPoint> CreatePoints(IReadOnlyList<string> categories, IReadOnlyList<decimal> values)
    {
        var count = Math.Min(categories.Count, values.Count);
        var points = new List<NumericPoint>(count);
        for (var index = 0; index < count; index++)
        {
            points.Add(new NumericPoint(categories[index], values[index]));
        }

        return points;
    }

    private void HandleThemeChanged(bool isDark)
    {
        _ = InvokeAsync(() =>
        {
            _isDarkMode = isDark;
            StateHasChanged();
            return Task.CompletedTask;
        });
    }

    public void Dispose()
    {
        ThemeService.ThemeChanged -= HandleThemeChanged;
    }

    private sealed record DeviceSummary(string Name, string Value, string ChangeText, string ChangeCss);
    private sealed record TransactionEntry(string Name, string Date, string Amount, string ReferenceNumber, int PaymentSuffix, PaymentBrand Brand, TransactionStatus Status);

    private sealed class StatusFilterOption
    {
        public StatusFilterOption(string label, TransactionStatus status, bool isSelected, int count)
        {
            Label = label;
            Status = status;
            IsSelected = isSelected;
            Count = count;
        }

        public string Label { get; }
        public TransactionStatus Status { get; }
        public bool IsSelected { get; set; }
        public int Count { get; }
    }

    private sealed record DateRangeOption(string Label, int DaysBack);

    private static string GetStatusLabel(TransactionStatus status) => status switch
    {
        TransactionStatus.Completed => "Completed",
        TransactionStatus.Cancelled => "Canceled",
        TransactionStatus.InReview => "In review",
        TransactionStatus.InProgress => "In progress",
        _ => "Unknown"
    };

    private static string GetStatusBadgeClasses(TransactionStatus status) => status switch
    {
        TransactionStatus.Completed => "inline-flex items-center rounded-full bg-emerald-100 px-2 py-1 text-xs font-semibold text-emerald-800 dark:bg-emerald-900/50 dark:text-emerald-300",
        TransactionStatus.Cancelled => "inline-flex items-center rounded-full bg-red-100 px-2 py-1 text-xs font-semibold text-red-800 dark:bg-red-900/50 dark:text-red-300",
        TransactionStatus.InReview => "inline-flex items-center rounded-full bg-amber-100 px-2 py-1 text-xs font-semibold text-amber-800 dark:bg-amber-900/50 dark:text-amber-300",
        TransactionStatus.InProgress => "inline-flex items-center rounded-full bg-purple-100 px-2 py-1 text-xs font-semibold text-purple-800 dark:bg-purple-900/50 dark:text-purple-300",
        _ => "inline-flex items-center rounded-full bg-gray-100 px-2 py-1 text-xs font-semibold text-gray-800 dark:bg-gray-800 dark:text-gray-300"
    };

    private static string GetDateRangeSummary(DateRangeOption option)
    {
        var (start, end) = CalculateDateRange(option);
        var format = "MMM d, yyyy";
        var startText = start.ToString(format, CultureInfo.InvariantCulture);
        var endText = end.ToString(format, CultureInfo.InvariantCulture);
        return start == end ? startText : $"{startText} - {endText}";
    }

    private Task SelectDateRange(DateRangeOption option)
    {
        SelectedDateRange = option;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private static (DateTime Start, DateTime End) CalculateDateRange(DateRangeOption option)
    {
        var today = DateTime.Today;
        return option.DaysBack switch
        {
            -1 =>
              (today.AddDays(-1), today.AddDays(-1)),
            0 =>
              (today, today),
            _ =>
              (today.AddDays(-option.DaysBack), today)
        };
    }

    private enum TransactionStatus
    {
        Completed,
        Cancelled,
        InReview,
        InProgress
    }

    private enum PaymentBrand
    {
        Visa,
        Mastercard
    }
}
