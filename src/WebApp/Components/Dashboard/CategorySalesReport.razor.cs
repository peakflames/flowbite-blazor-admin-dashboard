using System.Collections.Generic;
using ApexCharts;
using Microsoft.AspNetCore.Components;
using WebApp.Charts;
using WebApp.Domain.Dashboard;

namespace WebApp.Components.Dashboard;

public partial class CategorySalesReport : ComponentBase
{
  private ApexChart<NumericPoint>? _chart;
  private ApexChartOptions<NumericPoint> _options = DashboardChartOptions.CreateCategorySalesBarOptions<NumericPoint>();

  [Parameter] public string Title { get; set; } = string.Empty;
  [Parameter] public string? Subtitle { get; set; }
  [Parameter] public decimal? ChangeValue { get; set; }
  [Parameter] public string? ChangeCaption { get; set; }
  [Parameter] public IReadOnlyList<ChartSeriesDefinition<NumericPoint>>? Series { get; set; }
  [Parameter] public bool IsDarkMode { get; set; }
  [Parameter] public string CardClass { get; set; } = string.Empty;
  [Parameter] public RenderFragment? HeaderContent { get; set; }
  [Parameter] public RenderFragment? FooterContent { get; set; }

  private bool ShowChangeIndicator => ChangeValue.HasValue;

  private string FormattedChange => ChangeValue switch
  {
    > 0 => $"+{ChangeValue.Value:0.##}%",
    < 0 => $"{ChangeValue.Value:0.##}%",
    0 => "0%",
    _ => string.Empty
  };

  private string ChangeCss => ChangeValue switch
  {
    > 0 => "text-emerald-500 dark:text-emerald-400",
    < 0 => "text-red-500 dark:text-red-400",
    0 => "text-gray-500 dark:text-gray-400",
    _ => "text-gray-500 dark:text-gray-400"
  };

  protected override void OnParametersSet()
  {
    _options = DashboardChartOptions.CreateCategorySalesBarOptions<NumericPoint>(IsDarkMode);
  }
}
