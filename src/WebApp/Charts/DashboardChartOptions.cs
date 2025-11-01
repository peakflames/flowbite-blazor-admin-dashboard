using System;
using System.Collections.Generic;
using ApexCharts;

namespace WebApp.Charts;

public static class DashboardChartOptions
{
  public const string FontFamily = "Inter, sans-serif";
  private const string AxisColorLight = "#4B5563";
  private const string AxisColorDark = "#9CA3AF";
  private const string ChartBorderLight = "#F3F4F6";
  private const string ChartBorderDark = "#374151";
  private const string PrimaryHighlight = "#EF562F";
  private const string SecondaryHighlight = "#FDBA8C";
  private const string AccentTeal = "#16BDCA";
  private const string AccentCyan = "#17B0BD";
  private const string AccentBlue = "#1A56DB";

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
    AccentBlue,
    AccentTeal,
    "#0E7490",
    "#312E81"
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

  public static ApexChartBaseOptions CreateGlobalDefaults()
  {
    return new ApexChartBaseOptions
    {
      Colors = new List<string>(FlowbitePaletteValues),
      Chart = new Chart
      {
        FontFamily = FontFamily,
        ForeColor = AxisColorLight,
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
        Labels = new LegendLabels
        {
          Colors = AxisColorLight
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
      Fill = new Fill
      {
        Type = FillType.Gradient,
        Gradient = new FillGradient
        {
          Shade = GradientShade.Light,
          Type = GradientType.Vertical,
          ShadeIntensity = 0.5,
          OpacityFrom = colors.OpacityFrom,
          OpacityTo = colors.OpacityTo,
          Stops = new List<double> { 0, 100 }
        }
      },
      DataLabels = new DataLabels
      {
        Enabled = false
      },
      Tooltip = new Tooltip
      {
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
        StrokeDashArray = 1,
        Padding = new Padding
        {
          Left = 35,
          Bottom = 15
        }
      },
      Markers = new Markers
      {
        Size = new Size(5),
        StrokeColors = "#ffffff",
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
        ItemMargin = new LegendItemMargin
        {
          Horizontal = 10
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

  public static ApexChartOptions<TItem> CreateTrafficDonutOptions<TItem>(bool isDarkMode)
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
      Colors = new List<string> { AccentTeal, SecondaryHighlight, AccentBlue },
      Stroke = new Stroke
      {
        Colors = new List<string> { isDarkMode ? "#1f2937" : "#ffffff" }
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
      ? new MainChartColors(ChartBorderDark, AxisColorDark, 0.0, 0.15)
      : new MainChartColors(ChartBorderLight, AxisColorLight, 0.45, 0.0);
  }

  private readonly record struct MainChartColors(string BorderColor, string LabelColor, double OpacityFrom, double OpacityTo);
}
