using System;
using System.Collections.Generic;
using ApexCharts;

namespace WebApp.Charts;

public static class DashboardChartOptions
{
  public const string FontFamily = "Inter, sans-serif";
  private const string AxisColorLight = "#4B5563";
  private const string AxisColorDark = "#9CA3AF";
  private const string ChartBorderLight = "#E5E7EB";
  private const string ChartBorderDark = "#1F2937";
  private const string PrimaryHighlight = "#1A56DB";
  private const string SecondaryHighlight = "#3B82F6";
  private const string AccentTeal = "#0EA5E9";
  private const string AccentCyan = "#38BDF8";
  private const string AccentNavy = "#1E3A8A";
  private const string AccentIndigo = "#312E81";

  private static readonly string[] RevenueCategoryValues =
  {
    "01 Feb",
    "02 Feb",
    "03 Feb",
    "04 Feb",
    "05 Feb",
    "06 Feb",
    "07 Feb"
  };

  private static readonly string[] FlowbitePaletteValues =
  {
    PrimaryHighlight,
    SecondaryHighlight,
    AccentCyan,
    AccentTeal,
    AccentNavy,
    AccentIndigo
  };

  private static readonly string[] LightBarBackgroundValues =
  {
    "#E5E7EB",
    "#E5E7EB",
    "#E5E7EB",
    "#E5E7EB",
    "#E5E7EB",
    "#E5E7EB",
    "#E5E7EB"
  };

  private static readonly string[] DarkBarBackgroundValues =
  {
    ChartBorderDark,
    ChartBorderDark,
    ChartBorderDark,
    ChartBorderDark,
    ChartBorderDark,
    ChartBorderDark,
    ChartBorderDark
  };

  public static IReadOnlyList<string> MainChartCategoryLabels { get; } = Array.AsReadOnly(RevenueCategoryValues);

  public static IReadOnlyList<string> DefaultPalette { get; } = Array.AsReadOnly(FlowbitePaletteValues);

  public static ApexChartBaseOptions CreateGlobalDefaults(bool isDarkMode)
  {
    var baseAxisColor = isDarkMode ? AxisColorDark : AxisColorLight;

    return new ApexChartBaseOptions
    {
      Colors = new List<string>(FlowbitePaletteValues),
      Chart = new Chart
      {
        FontFamily = FontFamily,
        ForeColor = baseAxisColor,
        Toolbar = new Toolbar
        {
          Show = false
        }
      },
      Tooltip = new Tooltip
      {
        Style = new TooltipStyle
        {
          FontFamily = FontFamily,
          FontSize = "14px"
        }
      },
      Legend = new Legend
      {
        FontFamily = FontFamily,
        FontSize = "14px",
        FontWeight = 500,
        HorizontalAlign = Align.Left,
        Labels = new LegendLabels
        {
          Colors = baseAxisColor
        },
        Markers = new LegendMarkers
        {
          Width = 12,
          Height = 12,
          Radius = 12,
          OffsetX = -2,
          OffsetY = -1
        },
        ItemMargin = new LegendItemMargin
        {
          Horizontal = 12
        }
      }
    };
  }

  public static ApexChartOptions<TItem> CreateRevenueAreaOptions<TItem>(bool isDarkMode)
    where TItem : class
  {
    var colors = GetMainChartColors(isDarkMode);

    return new ApexChartOptions<TItem>
    {
      Chart = new Chart
      {
        Height = 420,
        Type = ChartType.Area,
        FontFamily = FontFamily,
        ForeColor = colors.LabelColor,
        Toolbar = new Toolbar
        {
          Show = false
        }
      },
      Stroke = new Stroke
      {
        Curve = Curve.Smooth,
        Width = new Size(4)
      },
      Fill = new Fill
      {
        Type = FillType.Gradient,
        Gradient = new FillGradient
        {
          Shade = GradientShade.Light,
          Type = GradientType.Vertical,
          ShadeIntensity = 0.9,
          OpacityFrom = colors.OpacityFrom,
          OpacityTo = colors.OpacityTo,
          Stops = new List<double> { 0, 85, 100 }
        }
      },
      DataLabels = new DataLabels
      {
        Enabled = false
      },
      Tooltip = new Tooltip
      {
        Shared = true,
        Intersect = false,
        Style = new TooltipStyle
        {
          FontFamily = FontFamily,
          FontSize = "14px"
        }
      },
      Grid = new Grid
      {
        Show = true,
        BorderColor = colors.BorderColor,
        StrokeDashArray = 4,
        Padding = new Padding
        {
          Left = 25,
          Right = 15,
          Bottom = 15
        }
      },
      Markers = new Markers
      {
        Size = new Size(6),
        StrokeColors = isDarkMode ? "#111827" : "#ffffff",
        StrokeWidth = new Size(2),
        Hover = new MarkersHover
        {
          SizeOffset = 3
        }
      },
      Xaxis = new XAxis
      {
        Categories = new List<string>(MainChartCategoryLabels),
        Labels = new XAxisLabels
        {
          Style = new AxisLabelStyle
          {
            Colors = colors.LabelColor,
            FontFamily = FontFamily,
            FontSize = "14px",
            FontWeight = 500
          }
        },
        AxisBorder = new AxisBorder
        {
          Color = colors.BorderColor
        },
        AxisTicks = new AxisTicks
        {
          Color = colors.BorderColor
        },
        Crosshairs = new AxisCrosshairs
        {
          Show = true,
          Position = GridPosition.Back,
          Stroke = new CrosshairStroke
          {
            Color = colors.BorderColor,
            Width = 1,
            DashArray = 10
          }
        }
      },
      Yaxis = new List<YAxis>
      {
        new()
        {
          Labels = new YAxisLabels
          {
            Formatter = "function (value) { return '$' + value; }",
            Style = new AxisLabelStyle
            {
              Colors = colors.LabelColor,
              FontFamily = FontFamily,
              FontSize = "14px",
              FontWeight = 500
            }
          }
        }
      },
      Legend = new Legend
      {
        FontFamily = FontFamily,
        FontSize = "14px",
        FontWeight = 500,
        Labels = new LegendLabels
        {
          Colors = colors.LabelColor
        },
        Markers = new LegendMarkers
        {
          Width = 12,
          Height = 12,
          Radius = 12,
          OffsetX = -2,
          OffsetY = -1
        },
        ItemMargin = new LegendItemMargin
        {
          Horizontal = 14
        }
      },
      Responsive = new List<Responsive<TItem>>
      {
        new()
        {
          Breakpoint = 1024,
          Options = new ApexChartOptions<TItem>
          {
            Xaxis = new XAxis
            {
              Labels = new XAxisLabels
              {
                Show = false
              }
            }
          }
        }
      }
    };
  }

  public static ApexChartOptions<TItem> CreateProductQuantityBarOptions<TItem>(bool isDarkMode = false)
    where TItem : class
  {
    var tooltip = CreateTooltip(shared: false, intersect: false);

    return new ApexChartOptions<TItem>
    {
      Chart = CreateBarChart(140, isDarkMode),
      Colors = new List<string> { PrimaryHighlight },
      PlotOptions = new PlotOptions
      {
        Bar = new PlotOptionsBar
        {
          ColumnWidth = "90%",
          BorderRadius = 3
        }
      },
      Tooltip = tooltip,
      States = CreateHoverDarkenState(),
      Stroke = new Stroke
      {
        Show = true,
        Width = new Size(5),
        Colors = new List<string> { "transparent" }
      },
      Grid = new Grid { Show = false },
      DataLabels = new DataLabels { Enabled = false },
      Legend = new Legend { Show = false },
      Xaxis = CreateHiddenCategoryAxis(),
      Yaxis = new List<YAxis> { new() { Show = false } },
      Fill = new Fill { Opacity = 1 }
    };
  }

  public static ApexChartOptions<TItem> CreateUserBarOptions<TItem>(bool isDarkMode, bool horizontal = false)
    where TItem : class
  {
    var tooltip = CreateTooltip(shared: true, intersect: false);
    tooltip.FollowCursor = false;

    return new ApexChartOptions<TItem>
    {
      Chart = CreateBarChart(140, isDarkMode),
      PlotOptions = new PlotOptions
      {
        Bar = new PlotOptionsBar
        {
          ColumnWidth = "25%",
          BorderRadius = 3,
          Horizontal = horizontal,
          Colors = new PlotOptionsBarColors
          {
            BackgroundBarColors = GetBackgroundBarColors(isDarkMode),
            BackgroundBarRadius = 3
          }
        }
      },
      Theme = new Theme
      {
        Monochrome = new ThemeMonochrome
        {
          Enabled = true,
          Color = PrimaryHighlight,
          ShadeIntensity = 0.65,
          ShadeTo = Mode.Dark
        }
      },
      Tooltip = tooltip,
      States = CreateHoverDarkenState(),
      Grid = new Grid { Show = false },
      DataLabels = new DataLabels { Enabled = false },
      Legend = new Legend { Show = false },
      Xaxis = CreateHiddenCategoryAxis(),
      Yaxis = new List<YAxis> { new() { Show = false } },
      Fill = new Fill { Opacity = 1 }
    };
  }

  public static ApexChartOptions<TItem> CreateCategorySalesBarOptions<TItem>(bool isDarkMode = false)
    where TItem : class
  {
    var tooltip = CreateTooltip(shared: true, intersect: false);

    return new ApexChartOptions<TItem>
    {
      Chart = CreateBarChart(420, isDarkMode),
      Colors = new List<string> { PrimaryHighlight, SecondaryHighlight, AccentCyan },
      PlotOptions = new PlotOptions
      {
        Bar = new PlotOptionsBar
        {
          ColumnWidth = "90%",
          BorderRadius = 3
        }
      },
      Tooltip = tooltip,
      States = CreateHoverDarkenState(),
      Stroke = new Stroke
      {
        Show = true,
        Width = new Size(5),
        Colors = new List<string> { "transparent" }
      },
      Grid = new Grid { Show = false },
      DataLabels = new DataLabels { Enabled = false },
      Legend = new Legend { Show = false },
      Xaxis = CreateHiddenCategoryAxis(),
      Yaxis = new List<YAxis> { new() { Show = false } },
      Fill = new Fill { Opacity = 1 }
    };
  }

  public static ApexChartOptions<TItem> CreateTrafficDonutOptions<TItem>(bool isDarkMode, string? centerLabel = null, string? centerValue = null)
    where TItem : class
  {
    var tooltip = CreateTooltip(shared: true, intersect: false);
    tooltip.FollowCursor = false;
    tooltip.FillSeriesColor = false;
    tooltip.InverseOrder = true;
    tooltip.X = new TooltipX
    {
      Show = true,
      Formatter = "function (value) { return value; }"
    };
    tooltip.Y = new TooltipY
    {
      Formatter = "function (value) { return value + '%'; }"
    };

    var resolvedLabel = string.IsNullOrWhiteSpace(centerLabel) ? "Top device" : centerLabel;
    var resolvedValue = string.IsNullOrWhiteSpace(centerValue) ? string.Empty : centerValue;
    var jsLabel = resolvedLabel.Replace("'", "\\'");
    var jsValue = resolvedValue.Replace("'", "\\'");
    var labelColor = isDarkMode ? "#F9FAFB" : "#111827";

    return new ApexChartOptions<TItem>
    {
      Chart = new Chart
      {
        Type = ChartType.Donut,
        Height = 400,
        FontFamily = FontFamily,
        ForeColor = isDarkMode ? AxisColorDark : AxisColorLight,
        Toolbar = new Toolbar { Show = false }
      },
      Colors = new List<string> { PrimaryHighlight, AccentTeal, SecondaryHighlight },
      PlotOptions = new PlotOptions
      {
        Pie = new PlotOptionsPie
        {
          Donut = new PlotOptionsDonut
          {
            Size = "70%",
            Labels = new DonutLabels
            {
              Show = true,
              Name = new DonutLabelName
              {
                Show = true,
                OffsetY = 10,
                Color = labelColor,
                FontFamily = FontFamily,
                FontSize = "16px",
                FontWeight = 500,
                Formatter = $"function (w) {{ return '{jsLabel}'; }}"
              },
              Value = new DonutLabelValue
              {
                Show = true,
                OffsetY = -8,
                Color = labelColor,
                FontFamily = FontFamily,
                FontSize = "28px",
                FontWeight = 700,
                Formatter = $"function (w) {{ return '{jsValue}'; }}"
              },
              Total = new DonutLabelTotal
              {
                Show = false
              }
            }
          }
        }
      },
      Stroke = new Stroke
      {
        Colors = new List<string> { isDarkMode ? "#111827" : "#ffffff" },
        Width = new Size(2)
      },
      States = CreateHoverDarkenState(),
      Tooltip = tooltip,
      Grid = new Grid { Show = false },
      DataLabels = new DataLabels { Enabled = false },
      Legend = new Legend { Show = false },
      Responsive = new List<Responsive<TItem>>
      {
        new()
        {
          Breakpoint = 430,
          Options = new ApexChartOptions<TItem>
          {
            Chart = new Chart
            {
              Height = 300
            }
          }
        }
      }
    };
  }

  private static Chart CreateBarChart(int height, bool isDarkMode)
  {
    return new Chart
    {
      Type = ChartType.Bar,
      Height = height,
      FontFamily = FontFamily,
      ForeColor = isDarkMode ? AxisColorDark : AxisColorLight,
      Toolbar = new Toolbar { Show = false }
    };
  }

  private static XAxis CreateHiddenCategoryAxis()
  {
    return new XAxis
    {
      Floating = false,
      Labels = new XAxisLabels { Show = false },
      AxisBorder = new AxisBorder { Show = false },
      AxisTicks = new AxisTicks { Show = false }
    };
  }

  private static Tooltip CreateTooltip(bool shared, bool intersect)
  {
    return new Tooltip
    {
      Shared = shared,
      Intersect = intersect,
      Style = new TooltipStyle
      {
        FontFamily = FontFamily,
        FontSize = "14px"
      }
    };
  }

  private static States CreateHoverDarkenState()
  {
    return new States
    {
      Hover = new StatesHover
      {
        Filter = new StatesFilter
        {
          Type = StatesFilterType.darken
        }
      }
    };
  }

  private static List<string> GetBackgroundBarColors(bool isDarkMode)
  {
    return new List<string>(isDarkMode ? DarkBarBackgroundValues : LightBarBackgroundValues);
  }

  private static MainChartColors GetMainChartColors(bool isDarkMode)
  {
    return isDarkMode
      ? new MainChartColors(ChartBorderDark, AxisColorDark, 0.35, 0.05)
      : new MainChartColors(ChartBorderLight, AxisColorLight, 0.6, 0.1);
  }

  private readonly record struct MainChartColors(string BorderColor, string LabelColor, double OpacityFrom, double OpacityTo);
}
