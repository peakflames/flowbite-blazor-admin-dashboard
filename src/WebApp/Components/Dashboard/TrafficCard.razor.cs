using System.Collections.Generic;
using System.Linq;
using ApexCharts;
using Microsoft.AspNetCore.Components;
using WebApp.Charts;
using WebApp.Domain.Dashboard;

namespace WebApp.Components.Dashboard;

public partial class TrafficCard : ComponentBase
{
  private ApexChart<NumericPoint>? _chart;
  private ApexChartOptions<NumericPoint> _options = DashboardChartOptions.CreateTrafficDonutOptions<NumericPoint>(false);

  [Parameter] public string Title { get; set; } = "Traffic by device";
  [Parameter] public string? Subtitle { get; set; }
  [Parameter] public string SeriesName { get; set; } = "Traffic";
  [Parameter] public IReadOnlyList<NumericPoint>? Slices { get; set; }
  [Parameter] public bool IsDarkMode { get; set; }
  [Parameter] public string CardClass { get; set; } = string.Empty;
  [Parameter] public RenderFragment? HeaderContent { get; set; }
  [Parameter] public RenderFragment? BodyContent { get; set; }
  [Parameter] public RenderFragment? FooterContent { get; set; }

  protected override void OnParametersSet()
  {
    var primarySlice = Slices?.OrderByDescending(slice => slice.Value).FirstOrDefault();
    var centerLabel = primarySlice?.Category ?? Subtitle;
    var centerValue = primarySlice is null ? null : $"{primarySlice.Value:0.#}%";

    _options = DashboardChartOptions.CreateTrafficDonutOptions<NumericPoint>(IsDarkMode, centerLabel, centerValue);
  }
}
