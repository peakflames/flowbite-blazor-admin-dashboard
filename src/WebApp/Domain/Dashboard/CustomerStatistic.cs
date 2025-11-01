namespace WebApp.Domain.Dashboard;

public sealed record CustomerStatistic(
  string Name,
  string Email,
  string ImageUrl,
  decimal Spend);
