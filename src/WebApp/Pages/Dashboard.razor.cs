using System;
using System.Collections.Generic;
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

  protected override async Task OnInitializedAsync()
  {
    _isDarkMode = await ThemeService.InitializeAsync();
    ThemeService.ThemeChanged += HandleThemeChanged;
    BuildSeries();
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
}
