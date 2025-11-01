using System.Collections.Generic;

namespace WebApp.Domain.Dashboard;

public sealed record ChartSeriesDefinition<TPoint>(string Name, IReadOnlyList<TPoint> Points, string? Color = null)
  where TPoint : class;
