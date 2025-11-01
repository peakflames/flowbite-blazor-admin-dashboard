using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using WebApp.Domain.Dashboard;

namespace WebApp.Components.Dashboard;

public partial class StatisticsCard : ComponentBase
{
  private StatisticsTab _activeTab = StatisticsTab.Products;

  [Parameter] public string Title { get; set; } = "Statistics this month";
  [Parameter] public IReadOnlyList<ProductStatistic>? Products { get; set; }
  [Parameter] public IReadOnlyList<CustomerStatistic>? Customers { get; set; }
  [Parameter] public string PeriodLabel { get; set; } = "Last 7 days";
  [Parameter] public string ReportHref { get; set; } = "#";
  [Parameter] public string CardClass { get; set; } = string.Empty;

  protected override void OnParametersSet()
  {
    if ((_activeTab == StatisticsTab.Products && (Products?.Count ?? 0) == 0) &&
        (Customers?.Count ?? 0) > 0)
    {
      _activeTab = StatisticsTab.Customers;
    }
    else if ((_activeTab == StatisticsTab.Customers && (Customers?.Count ?? 0) == 0) &&
             (Products?.Count ?? 0) > 0)
    {
      _activeTab = StatisticsTab.Products;
    }
  }

  private void SetTab(StatisticsTab tab)
  {
    if (_activeTab != tab)
    {
      _activeTab = tab;
    }
  }

  private static string GetChangeClass(decimal changePercent)
  {
    var isPositive = changePercent >= 0;
    return isPositive
      ? "inline-flex items-center font-semibold text-emerald-500 dark:text-emerald-400"
      : "inline-flex items-center font-semibold text-red-500 dark:text-red-400";
  }

  private static string GetTabButtonClass(StatisticsTab tab, StatisticsTab activeTab) =>
    tab == activeTab
      ? "w-full rounded-t-lg bg-gray-100 p-4 text-center text-sm font-medium text-primary-600 shadow-inner dark:bg-gray-800 dark:text-primary-400"
      : "w-full rounded-t-lg p-4 text-center text-sm font-medium text-gray-500 transition hover:bg-gray-50 hover:text-gray-600 dark:text-gray-400 dark:hover:bg-gray-800 dark:hover:text-gray-200";

  private string GetTabButtonClass(StatisticsTab tab) => GetTabButtonClass(tab, _activeTab);

  private static string FormatChange(decimal changePercent)
  {
    var rounded = Math.Round(changePercent, 2, MidpointRounding.AwayFromZero);
    return rounded >= 0 ? $"+{rounded:0.##}%" : $"{rounded:0.##}%";
  }

  private static string FormatCurrency(decimal amount) =>
    amount.ToString("$#,0", CultureInfo.InvariantCulture);

  private enum StatisticsTab
  {
    Products,
    Customers
  }
}
