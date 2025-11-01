using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace WebApp.Services;

public sealed class ThemeService : IAsyncDisposable
{
  private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
  private DotNetObjectReference<ThemeService>? _dotNetRef;
  private bool _isInitialized;
  private bool _isDarkMode;

  public ThemeService(IJSRuntime jsRuntime)
  {
    ArgumentNullException.ThrowIfNull(jsRuntime);
    _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/themeObserver.js").AsTask());
  }

  public event Action<bool>? ThemeChanged;

  public bool IsDarkMode => _isDarkMode;

  public async Task<bool> InitializeAsync()
  {
    if (_isInitialized)
    {
      return _isDarkMode;
    }

    var module = await _moduleTask.Value;
    _dotNetRef ??= DotNetObjectReference.Create(this);
    _isDarkMode = await module.InvokeAsync<bool>("start", _dotNetRef);
    _isInitialized = true;
    return _isDarkMode;
  }

  [JSInvokable(nameof(NotifyThemeChanged))]
  public Task NotifyThemeChanged(bool isDark)
  {
    if (_isDarkMode == isDark)
    {
      return Task.CompletedTask;
    }

    _isDarkMode = isDark;
    ThemeChanged?.Invoke(isDark);
    return Task.CompletedTask;
  }

  public async ValueTask DisposeAsync()
  {
    if (_isInitialized && _moduleTask.IsValueCreated)
    {
      try
      {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("stop");
      }
      catch
      {
        // ignored - shutdown
      }
    }

    _dotNetRef?.Dispose();
    _dotNetRef = null;
    _isInitialized = false;
  }
}
