# Migrating Flowbite Svelte Charts to Blazor

## Current Svelte Chart Stack
- The Svelte dashboard depends on Flowbite’s chart wrapper, `@flowbite-svelte-plugins/chart`, which in turn bundles ApexCharts (`apexcharts` entry in `pnpm-lock.yaml`). This dependency is declared alongside `flowbite-svelte` in `package.json` (`/mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/package.json`).
- Chart widgets import the Flowbite wrapper and pass ApexCharts option objects. For example, `ChartWidget.svelte` pulls in `Chart` from `@flowbite-svelte-plugins/chart` and injects serialized options (`src/lib/ChartWidget.svelte`).
- Graph configuration files (e.g., `src/routes/utils/graphs/thickbars.ts`) author chart state with `ApexOptions` from `apexcharts`, confirming that every visualization is defined in terms of ApexCharts’ API even though the rendering wrapper is Flowbite-specific.
- Layout pages (such as `src/routes/utils/dashboard/Dashboard.svelte`) wire Flowbite `Chart` components with those option factories, often adjusting configuration for dark mode via helper utilities like `DarkChart.svelte`, which toggles Apex options when the `dark` class changes.

## Blazor-ApexCharts v6.0.2 Usage Notes
- **Core component** – `ApexChart<TItem>` (`src/Blazor-ApexCharts/ApexChart.razor(.cs)`) renders the chart placeholder and accepts per-instance `ApexChartOptions<TItem>`, title, axes configuration, and a rich set of event callbacks (data point selection, zoom, legend clicks, etc.). Each chart requires its own options object to avoid cross-instance mutation.
- **Series composition** – Series are provided as child components, typically `ApexPointSeries<TItem>` (`Series/ApexPointSeries.cs`). You bind strongly typed data via `Items`, set `SeriesType`, and supply lambda expressions for `XValue`, `YValue`, ordering, or aggregation. Additional series types (range, bubble, candlestick, box plot, etc.) live in the same folder.
- **Chart service** – Register `services.AddApexCharts()` to get an `IApexChartService` (`ChartService/ApexChartService.cs`). The service loads the required JavaScript, manages a registry of rendered charts, applies global options/locales, and can re-render or update all charts (e.g., when toggling dark mode or changing locale).
- **Options model** – The options schema mirrors ApexCharts’ JS API via classes under `Models/` (axes, data labels, tooltips, responsive breakpoints, etc.). Components serialize these objects through `ChartSerializer` before sending them to the JS interop layer.
- **Runtime control** – `ApexChart<TItem>` exposes `RenderAsync` and `UpdateOptionsAsync` to refresh charts after modifying data/appearance, and propagates hover data into an optional `ApexPointTooltip` render fragment (`ApexChartTooltip.razor`).
- **Static assets** – The package ships its JS assets under `_content/Blazor-ApexCharts/`. When the chart service initializes it loads `blazor-apexcharts.js` (see `Internal/JSLoader.cs`) and uses `IJSRuntime` to invoke functions on `blazor_apexchart`.

## Migration Plan

1. **Baseline audit**
   - Capture the list of Flowbite chart widgets and option factories used on the Svelte dashboard (line, bar, area, radial, etc.).
   - Classify each chart by series type, axis requirements, tooltip customization, and dark-mode behavior.
2. **Blazor scaffolding**
   - Add the nuget package to the csproj
   - Register `AddApexCharts()` in `Program.cs` with global options for palette/theme parity.
   - Create a reusable `DashboardChartOptions` helper in Blazor mirroring the Svelte `getChartOptions` utilities (light/dark aware).
3. **Component wrappers**
   - For each Svelte widget (e.g., `ChartWidget`, `ProductMetricCard`, `Traffic`), sketch a Blazor component that encapsulates card layout + `<ApexChart>` markup.
   - Port chart-specific configs by translating the corresponding `ApexOptions` object to `ApexChartOptions<TItem>` (axes, series colors, markers, tooltips).
4. **Data + series binding**
   - Map the dashboard view models to strongly typed `List<T>` collections for each chart.
   - Define `ApexPointSeries` (and other series components where needed) with `Items`, `XValue`, `YValue`, and `SeriesType` matching the Svelte datasets.
5. **Dark mode & interactivity**
   - Wire a theme observer (e.g., cascading parameter or JS interop) that toggles Apex global options through `IApexChartService.SetGlobalOptionsAsync` when the site theme changes.
   - Recreate event-driven UI (date range selectors, “Sales Report” links) and ensure tooltips follow the Blazor `ApexPointTooltip` pattern when custom markup is required.
6. **Validation**
   - Compare rendered charts against the Svelte originals (colors, legend order, spacing).
   - Add automated smoke tests or snapshot captures where feasible to guard regressions.


## Baseline Audit Findings

| Svelte chart usage | Options source | Apex series type & data shape | Axes & legend | Tooltip customization | Dark-mode behavior | Notes for Blazor port |
| --- | --- | --- | --- | --- | --- | --- |
| `ChartWidget.svelte` main revenue card | `src/lib/chart_options.ts` + runtime `series` injection | Area series with string categories (`['01 Feb', … '07 Feb']`) and numeric Y values; two series assigned in `Dashboard.svelte` | Both axes visible with custom font styles; legend shown | Uses default Apex tooltip with custom font family/size | `getChartOptions(dark)` swaps grid/axis colors and gradient opacity | Create reusable `DashboardChartOptions` builder returning `ApexChartOptions<T>`; call `chart.UpdateOptionsAsync` when theme flips |
| `ProductMetricCard` (“New products”) | `src/routes/utils/graphs/thickbars.ts` | Single `bar` series of `{ x: string; y: number }` points with explicit color | Axes, grid, legend disabled; relies on column width/border radius | Tooltip `shared=false`, custom font styling | No dark-mode branch | Convert to `ApexPointSeries<T>`; ensure options isolate per instance to avoid shared mutation |
| `ProductMetricCard` (“Users” vertical) | `src/routes/utils/graphs/users.ts` (`dark` parameter) | Single monochrome `bar` series over numeric array + `labels` | Axes hidden, background bars emulate progress | Tooltip `shared=true` | Switches background bar colors (`#E5E7EB` ↔ `#374151`) based on `dark` | Implement theme-aware options factory and wrap with Blazor component to rebuild on theme change |
| `ProductMetricCard` (“Users” horizontal) | Same as above but sets `plotOptions.bar.horizontal = true` inline | Same data & series as vertical | Horizontal orientation hides axes; same visuals | Tooltip `shared=true` | Inherits `dark` toggle above | Need cloning helper before toggling `horizontal` to prevent cross-instance mutation |
| `CategorySalesReport` card | `src/routes/utils/graphs/thinmultibars.ts` | Three grouped `bar` series with `{ x, y }` points, unique colors per series | Axes hidden; legend hidden (color communicates series) | Tooltip shared; default content | No theme variation | Map to multiple `ApexPointSeries<T>`; ensure Flowbite palette preserved |
| `Traffic` donut widget | `src/routes/utils/graphs/traffic.ts` | Donut chart with numeric series `[70, 5, 25]` + labels | Legend off; relies on slice colors | Tooltip overrides `x.formatter` (label) and `y.formatter` (percent) | `stroke.colors` toggles between white and dark gray | Requires pie/donut options and service-driven global stroke update on theme change |
| `DarkChart.svelte` utility | Accepts `configFunc(dark)` returning `ApexOptions` | Pass-through to Flowbite Chart | N/A | N/A | Listens for global `dark` DOM event and recomputes options | Replace with CascadingParameter/JS interop that calls `IApexChartService.SetGlobalOptionsAsync` + component-level `UpdateOptionsAsync` |

Additional factories (`thinfillbars.ts`, `thinstackbars.ts`) provide monochrome sparkline/column variants used on documentation pages; keep parity if those widgets are recreated later.

## Blazor Scaffolding Notes
- `WebApp.csproj` references `Blazor-ApexCharts` v6.0.2 so the chart primitives are available in the WASM project.
- `Program.cs` registers `AddApexCharts()` with `DashboardChartOptions.CreateGlobalDefaults()` to preload the Flowbite palette, Inter font, and tooltip styling.
- `src/WebApp/Charts/DashboardChartOptions.cs` mirrors Svelte’s `getChartOptions`, returning immutable `ApexChartOptions<T>` objects per request so charts don’t share mutable state.
- `_Imports.razor` includes `@using ApexCharts`; existing QuickGrid pages now alias their `Align` enum to avoid namespace clashes.

## Dashboard Components
- `src/WebApp/Components/Dashboard/ChartWidget.razor` renders the revenue area chart with typed series metadata and reuses `CreateRevenueAreaOptions`.
- `src/WebApp/Components/Dashboard/ProductMetricCard.razor` supports both quantity and monochrome user bars via `ProductMetricChartType` and the new bar option factories.
- `src/WebApp/Components/Dashboard/CategorySalesReport.razor` outputs the multi-series grouped bar report fed by `ChartSeriesDefinition<NumericPoint>`.
- `src/WebApp/Components/Dashboard/TrafficCard.razor` maps donut slices to `CreateTrafficDonutOptions` and exposes header/body/footer slots for device stats.
- `src/WebApp/Domain/Dashboard/ChartSeriesDefinition.cs` and `NumericPoint.cs` provide simple immutable data records shared across all chart components.
- `src/WebApp/Pages/Dashboard.razor(.cs)` wires the new components together, translating the original Svelte option factories into typed `ChartSeriesDefinition<NumericPoint>` collections and rendering them with Flowbite cards.

## Theme Synchronization
- `src/WebApp/Services/ThemeService.cs` imports `/js/themeObserver.js` and broadcasts `ThemeChanged` events after watching the document class list for Flowbite’s dark-mode toggles.
- `src/WebApp/wwwroot/js/themeObserver.js` hosts the MutationObserver that keeps `ThemeService` informed about the `dark` class on `<html>`.
- `src/WebApp/Components/Dashboard/ThemeObserver.razor` lives in `MainLayout` to initialize the observer, and invokes `IApexChartService.SetGlobalOptionsAsync` with `DashboardChartOptions.CreateGlobalDefaults(isDark)` so every chart inherits palette updates.
- Per-page consumers (e.g., `Pages/Dashboard.razor(.cs)`) subscribe to `ThemeService` to re-evaluate chart option factories when the theme flips, keeping local chart instances in sync with the global Apex configuration.

## QA & Verification Plan
- Launch `dotnet watch --non-interactive` from `src/WebApp` and browse to `/dashboard`; compare colors, spacing, and legend ordering against <https://flowbite-svelte.com/admin-dashboard/dashboard>.
- Toggle the theme switch in the navbar and confirm that all charts update palettes (area gradient, monochrome bars, donut stroke) without a full page reload.
- Exercise responsive breakpoints (1024px and 430px) to ensure the area chart hides x-axis labels and the donut chart height matches the Svelte layout.
- Execute the Playwright smoke suite once the dashboard route is wired into the automation harness; capture baseline screenshots for future regressions. (Latest capture: `docs/references/dashboard-20251102.png`)
- Record any visual or behavioral gaps back in this document before marking the checklist item as complete.

### Playwright Baseline (2025‑11‑02)
- Stored new dark-theme dashboard capture at `docs/references/dashboard-20251102.png` (blue palette revenue + donut center labels).

### QA Comparison Notes (2025‑11‑01)
- Layout parity: Blazor dashboard currently renders only the four primary chart cards; Svelte reference includes additional widgets (chat, stats tabs, insights carousel, transactions table). These need to be ported or consciously omitted with documentation.
- Chart palette / styling:
  - Revenue area chart now renders the intentional blue gradient with smoothed strokes and custom legend markers. Remaining difference: Svelte reference uses orange/red theme.
  - Product/User bar cards pick up the blue accent palette. Verify spacing/column width against reference once additional widgets land.
  - Category sales grouped bars use blue palette but still need spacing review versus Svelte.
  - Donut chart now shows a center device/value label; segmented legend pills from Svelte remain unimplemented.
- Typography/layout: Card padding and typography weights differ (e.g., Svelte area card includes subtitle and change pill aligned left). Align Tailwind classes to match.
- Card padding: Blazor cards have more interior padding than the Svelte widgets; trim spacing to match the tighter reference layout.
- Call-to-action footer content: Svelte cards include “Date range selector”, “More” links, and icon buttons that are absent (or placeholder text) in Blazor port.
- Dark-mode behavior validated; palette flips with navbar toggle. Need to ensure light theme also matches.
- Action Items: Port remaining dashboard components (Stats, Activity, Chat, Insights, Transactions), refine chart options per widget, add missing legends/tooltips, and align card footers with Flowbite Svelte markup.

## To-Do Checklist
- [x] Inventory every Flowbite chart instance and its option factory.
- [x] Add Blazor-ApexCharts service registration with default palette/theme.
- [x] Implement Blazor card + chart components, translating options chart-by-chart.
- [x] Bind strongly typed data series for all dashboard visualizations.
- [x] Hook dark-mode and locale interactions through `IApexChartService`.
- [ ] QA the Blazor dashboard against the Svelte reference and document gaps.

## Verication
- Use a seperate background process of `dotnet watch --non-interactive` is run and render changes fast due to it hot reload capability.
- use playwright-mcp to verify the rendered data.
- Note that the running svelte application is found at `https://flowbite-svelte.com/admin-dashboard/dashboard`
