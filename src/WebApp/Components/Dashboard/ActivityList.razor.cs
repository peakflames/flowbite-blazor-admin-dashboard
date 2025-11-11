using Microsoft.AspNetCore.Components;

namespace WebApp.Components.Dashboard;

public partial class ActivityList : ComponentBase
{
  [Parameter] public string Title { get; set; } = "Latest Activity";
  [Parameter] public RenderFragment? ChildContent { get; set; }
  [Parameter] public RenderFragment? ActionContent { get; set; }
  [Parameter] public string CardClass { get; set; } = "p-4 sm:p-6";
}
