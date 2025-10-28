using WebApp.Domain;

namespace WebApp.Services;

/// <summary>
/// Blazor service for managing Settings page state.
/// Uses event-based notifications for state changes.
/// Components subscribe to OnChange and call StateHasChanged() manually.
/// </summary>
public class SettingsService
{
    private UserSettings _settings = new();
    private List<SessionInfo> _sessions = new();

    /// <summary>
    /// Event fired when any settings state changes.
    /// Components should subscribe to this and call StateHasChanged().
    /// </summary>
    public event Action? OnChange;

    /// <summary>
    /// Current user settings (read-only access)
    /// </summary>
    public UserSettings Settings => _settings;

    /// <summary>
    /// Current active sessions (read-only access)
    /// </summary>
    public List<SessionInfo> Sessions => _sessions;

    public SettingsService()
    {
        InitializeDefaultData();
    }

    /// <summary>
    /// Save updated user settings
    /// </summary>
    public async Task SaveSettingsAsync(UserSettings settings)
    {
        // Simulate API call
        await Task.Delay(500);

        _settings = settings;
        NotifyStateChanged();
    }

    /// <summary>
    /// Update a notification item's active state
    /// </summary>
    public void UpdateNotification(NotificationItem item)
    {
        // In real app, would save to backend
        // For now, just notify of change
        NotifyStateChanged();
    }

    /// <summary>
    /// Revoke/remove a session
    /// </summary>
    public async Task RevokeSessionAsync(SessionInfo session)
    {
        // Simulate API call
        await Task.Delay(300);

        _sessions.Remove(session);
        NotifyStateChanged();
    }

    /// <summary>
    /// Get social accounts data
    /// </summary>
    public async Task<List<SocialAccount>> GetSocialAccountsAsync()
    {
        // Simulate API call
        await Task.Delay(100);

        return new List<SocialAccount>
        {
            new()
            {
                Platform = "Facebook",
                Username = "bonnie.green",
                IsConnected = true
                // Icon will be set by component using Flowbite icons
            },
            new()
            {
                Platform = "Twitter",
                Username = "@bonniegreen",
                IsConnected = true
            },
            new()
            {
                Platform = "GitHub",
                Username = "bonniegreen",
                IsConnected = false
            },
            new()
            {
                Platform = "LinkedIn",
                Username = "bonnie-green",
                IsConnected = false
            }
        };
    }

    /// <summary>
    /// Get user accounts data
    /// </summary>
    public async Task<List<UserAccount>> GetUserAccountsAsync()
    {
        // Simulate API call
        await Task.Delay(100);

        return new List<UserAccount>
        {
            new()
            {
                Name = "Neil Sims",
                Avatar = "https://flowbite-admin-dashboard.vercel.app/images/users/neil-sims.png",
                Country = "United States",
                Status = "Active"
            },
            new()
            {
                Name = "Bonnie Green",
                Avatar = "https://flowbite-admin-dashboard.vercel.app/images/users/bonnie-green.png",
                Country = "Australia",
                Status = "Active"
            },
            new()
            {
                Name = "Michael Gough",
                Avatar = "https://flowbite-admin-dashboard.vercel.app/images/users/michael-gough.png",
                Country = "United Kingdom",
                Status = "Inactive"
            }
        };
    }

    /// <summary>
    /// Toggle social account connection status
    /// </summary>
    public async Task ToggleSocialAccountAsync(SocialAccount account)
    {
        // Simulate API call
        await Task.Delay(300);

        account.IsConnected = !account.IsConnected;
        NotifyStateChanged();
    }

    /// <summary>
    /// Initialize default data for demonstration
    /// </summary>
    private void InitializeDefaultData()
    {
        _settings = new UserSettings
        {
            FirstName = "Bonnie",
            LastName = "Green",
            Email = "bonnie.green@company.com",
            Phone = "+(12) 345 6789",
            Birthday = "1990-08-15",
            Organization = "Themesberg LLC",
            Role = "Blazor Developer",
            Department = "Development",
            Country = "United States",
            City = "San Francisco",
            ZipCode = "94103",
            Address = "123 Main Street, Apt 4B"
        };

        _sessions = new List<SessionInfo>
        {
            new()
            {
                Device = "Chrome on macOS",
                IpAddress = "California 123.123.123.123",
                ActionHref = "#",
                ActionButtonText = "Revoke"
                // Icon will be set by component
            },
            new()
            {
                Device = "Safari on iPhone",
                IpAddress = "Rome 24.456.355.98",
                ActionHref = "#",
                ActionButtonText = "Revoke"
                // Icon will be set by component
            }
        };
    }

    /// <summary>
    /// Notify all subscribers that state has changed.
    /// Components will receive this event and call StateHasChanged().
    /// </summary>
    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}
