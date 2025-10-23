# Implementation Plan: Settings & Playground Pages Migration

## Overview

This document provides a detailed, task-by-task implementation plan for migrating the Settings page and Playground pages from the Flowbite Svelte Admin Dashboard to the Flowbite Blazor starter project.

**Scope**:
- Phase 1: Layout Refinements (prerequisites)
- Phase 2: Settings Page Implementation
- Phase 3: Playground Pages Implementation

**Estimated Total Time**: 16-24 hours (based on experienced Blazor developer)

---

## Phase 1: Layout Refinements

### Overview
Before implementing the Settings and Playground pages, enhance the existing `MainLayout` to support multiple layout patterns and improve responsive behavior.

### Prerequisite Reading
- `ARCHITECTURE_COMPARISON.md` - Section 4: Routing and Layouts
- Svelte layouts: `/mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(sidebar)/+layout.svelte`

---

### Task 1.1: Create Layout Base Classes
**Estimated Time**: 1 hour

**Objective**: Create abstract base layout component for shared functionality.

**Files to Create**:
- `src/WebApp/Layout/LayoutBase.razor`
- `src/WebApp/Layout/LayoutBase.razor.cs`

**Implementation**:

```razor
@* LayoutBase.razor *@
@inherits LayoutComponentBase

<div class="@GetLayoutClasses()">
    @RenderLayout()
</div>

@code {
    protected virtual string GetLayoutClasses() => "";

    protected virtual RenderFragment RenderLayout() => builder =>
    {
        builder.AddContent(0, Body);
    };
}
```

```csharp
// LayoutBase.razor.cs
using Microsoft.AspNetCore.Components;

namespace WebApp.Layout;

public partial class LayoutBase : LayoutComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    protected bool IsMobileMenuOpen { get; set; } = false;

    protected void ToggleMobileMenu()
    {
        IsMobileMenuOpen = !IsMobileMenuOpen;
        StateHasChanged();
    }

    protected override void OnInitialized()
    {
        Navigation.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        IsMobileMenuOpen = false;
        StateHasChanged();
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
    }
}
```

**Testing**:
- ✓ Base layout compiles without errors
- ✓ Navigation events properly close mobile menu

---

### Task 1.2: Refactor MainLayout for Extensibility
**Estimated Time**: 2 hours

**Objective**: Refactor `MainLayout` to extend `LayoutBase` and support layout variants.

**Files to Modify**:
- `src/WebApp/Layout/MainLayout.razor`
- `src/WebApp/Layout/MainLayout.razor.cs` (create if doesn't exist)

**Implementation**:

```razor
@* MainLayout.razor *@
@inherits LayoutBase

<div class="antialiased bg-gray-50 dark:bg-gray-900">
    <AppNavBar OnMenuToggle="@ToggleMobileMenu" IsMobileMenuOpen="@IsMobileMenuOpen" />

    <AppSidebar IsOpen="@(!IsMobileMenuOpen)" />

    <main class="p-4 md:ml-64 h-auto pt-20">
        @Body
    </main>

    <AppFooter />
</div>

<ToastHost />
```

**Key Changes**:
1. Inherit from `LayoutBase` instead of `LayoutComponentBase`
2. Use `ToggleMobileMenu` from base class
3. Pass `IsMobileMenuOpen` state to navbar
4. Ensure sidebar responds to state

**Testing**:
- ✓ Mobile menu toggle works
- ✓ Sidebar shows/hides on mobile
- ✓ Desktop layout unaffected
- ✓ Navigation closes mobile menu

---

### Task 1.3: Create StackedLayout (No Sidebar)
**Estimated Time**: 1.5 hours

**Objective**: Create alternate layout without sidebar for playground pages.

**Files to Create**:
- `src/WebApp/Layout/StackedLayout.razor`
- `src/WebApp/Layout/StackedLayout.razor.cs`

**Implementation**:

```razor
@* StackedLayout.razor *@
@inherits LayoutBase

<div class="antialiased bg-gray-50 dark:bg-gray-900 min-h-screen">
    <AppNavBar OnMenuToggle="@ToggleMobileMenu"
               IsMobileMenuOpen="@IsMobileMenuOpen"
               ShowSidebarToggle="false" />

    <main class="p-4 h-auto pt-20 max-w-screen-2xl mx-auto">
        @Body
    </main>

    <AppFooter />
</div>

<ToastHost />
```

**Additional Changes**:
Update `AppNavBar.razor` to support optional sidebar toggle:

```razor
@* Add parameter *@
@code {
    [Parameter]
    public bool ShowSidebarToggle { get; set; } = true;
}

@* Conditional rendering *@
@if (ShowSidebarToggle)
{
    <button @onclick="OnMenuToggle" ...>
        <HamburgerIcon />
    </button>
}
```

**Testing**:
- ✓ Stacked layout renders without sidebar
- ✓ Full-width content area
- ✓ Navbar shows without menu toggle
- ✓ Max-width constraint applied

---

### Task 1.4: Enhance Dark Mode Consistency
**Estimated Time**: 1 hour

**Objective**: Ensure dark mode styling is consistent across all layout elements.

**Files to Modify**:
- `src/WebApp/Layout/AppNavBar.razor`
- `src/WebApp/Layout/AppSidebar.razor`
- `src/WebApp/Layout/AppFooter.razor`

**Implementation Checklist**:

1. **AppNavBar**:
   - ✓ Background: `bg-white dark:bg-gray-800`
   - ✓ Border: `border-gray-200 dark:border-gray-700`
   - ✓ Text: `text-gray-900 dark:text-white`

2. **AppSidebar**:
   - ✓ Background: `bg-white dark:bg-gray-800`
   - ✓ Item hover: `hover:bg-gray-100 dark:hover:bg-gray-700`
   - ✓ Active item: `bg-gray-100 dark:bg-gray-700`

3. **AppFooter**:
   - ✓ Background: `bg-white dark:bg-gray-800`
   - ✓ Links: `text-gray-500 dark:text-gray-400`

**Testing**:
- ✓ Toggle dark mode - all elements update
- ✓ No flashing/inconsistent colors
- ✓ localStorage persists preference

---

### Task 1.5: Improve Sidebar Responsive Behavior
**Estimated Time**: 1.5 hours

**Objective**: Enhance sidebar collapse/expand on different breakpoints.

**Files to Modify**:
- `src/WebApp/Layout/AppSidebar.razor`

**Current Behavior**: Sidebar always visible on desktop
**Target Behavior**: Overlay on mobile, fixed on desktop, smooth transitions

**Implementation**:

```razor
@* AppSidebar.razor *@
<aside class="@GetSidebarClasses()"
       aria-label="Sidebar">
    <div class="h-full px-3 py-4 overflow-y-auto bg-white dark:bg-gray-800">
        @* Sidebar content *@
    </div>
</aside>

@* Add backdrop for mobile *@
@if (IsOpen && IsMobile())
{
    <div class="fixed inset-0 z-30 bg-gray-900/50 dark:bg-gray-900/80"
         @onclick="@(() => OnClose.InvokeAsync())"></div>
}

@code {
    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public EventCallback OnClose { get; set; }

    private string GetSidebarClasses()
    {
        var classes = new List<string>
        {
            "fixed top-0 left-0 z-40 w-64 h-screen pt-20 transition-transform",
            "bg-white border-r border-gray-200 dark:bg-gray-800 dark:border-gray-700"
        };

        if (!IsOpen)
        {
            classes.Add("-translate-x-full md:translate-x-0");
        }

        return string.Join(" ", classes);
    }

    private bool IsMobile()
    {
        // Use JSInterop or media query check
        return true; // Simplified for example
    }
}
```

**Testing**:
- ✓ Mobile: sidebar slides in/out
- ✓ Desktop: sidebar always visible
- ✓ Backdrop closes sidebar on mobile
- ✓ Smooth transitions

---

## Phase 2: Settings Page Implementation

### Overview
Implement the Settings page with 8 reusable components, matching the Svelte version functionality.

### Reference
- Svelte Settings: `/mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(sidebar)/settings/+page.svelte`
- Implementation Guide: `SVELTE_TO_BLAZOR_GUIDE.md` - Section 2

---

### Task 2.1: Create Domain Models
**Estimated Time**: 1 hour

**Objective**: Define all data models for Settings page.

**Files to Create**:
- `src/WebApp/Domain/UserSettings.cs`
- `src/WebApp/Domain/NotificationItem.cs`
- `src/WebApp/Domain/SessionInfo.cs`
- `src/WebApp/Domain/SelectOption.cs`
- `src/WebApp/Domain/SocialAccount.cs`
- `src/WebApp/Domain/UserAccount.cs`

**Implementation**: See `SVELTE_TO_BLAZOR_GUIDE.md` for complete model definitions.

**Additional Models**:

```csharp
// SocialAccount.cs
namespace WebApp.Domain;

public class SocialAccount
{
    public string Platform { get; set; } = "";
    public string Username { get; set; } = "";
    public bool IsConnected { get; set; }
    public Type? Icon { get; set; }
}

// UserAccount.cs
namespace WebApp.Domain;

public class UserAccount
{
    public string Name { get; set; } = "";
    public string Avatar { get; set; } = "";
    public string Country { get; set; } = "";
    public string Status { get; set; } = "";
}
```

**Testing**:
- ✓ All models compile
- ✓ DataAnnotations attributes present
- ✓ Nullable reference types correct

---

### Task 2.2: Create SettingsService
**Estimated Time**: 1.5 hours

**Objective**: Service to manage settings state and data.

**File to Create**:
- `src/WebApp/Services/SettingsService.cs`

**Implementation**: See `SVELTE_TO_BLAZOR_GUIDE.md` for complete implementation.

**Additional Methods**:

```csharp
public class SettingsService
{
    // ... existing code ...

    public async Task SaveSettingsAsync(UserSettings settings)
    {
        // Simulate API call
        await Task.Delay(500);
        _settings = settings;
        NotifyStateChanged();
    }

    public async Task<List<SocialAccount>> GetSocialAccountsAsync()
    {
        await Task.Delay(100);
        return new List<SocialAccount>
        {
            new() { Platform = "Facebook", Username = "bonnie.green", IsConnected = true, Icon = typeof(FacebookIcon) },
            new() { Platform = "Twitter", Username = "@bonniegreen", IsConnected = true, Icon = typeof(TwitterIcon) },
            new() { Platform = "GitHub", Username = "bonniegreen", IsConnected = false, Icon = typeof(GithubIcon) },
            new() { Platform = "LinkedIn", Username = "bonnie-green", IsConnected = false, Icon = typeof(LinkedinIcon) }
        };
    }

    public async Task<List<UserAccount>> GetUserAccountsAsync()
    {
        await Task.Delay(100);
        return new List<UserAccount>
        {
            new() { Name = "Neil Sims", Avatar = "/images/users/neil-sims.png", Country = "United States", Status = "Active" },
            new() { Name = "Bonnie Green", Avatar = "/images/users/bonnie-green.png", Country = "Australia", Status = "Active" }
        };
    }
}
```

**Register Service**:
```csharp
// Program.cs
services.AddScoped<SettingsService>();
```

**Testing**:
- ✓ Service instantiates
- ✓ Events fire on state change
- ✓ Default data loads
- ✓ Save operations work

---

### Task 2.3: Create Shared Components Directory
**Estimated Time**: 15 minutes

**Objective**: Organize Settings components.

**Directories to Create**:
```
src/WebApp/Components/
├── Settings/
│   ├── UserProfile.razor
│   ├── GeneralInfo.razor
│   ├── PasswordInfo.razor
│   ├── LanguageTime.razor
│   ├── SocialAccounts.razor
│   ├── Accounts.razor
│   ├── Sessions.razor
│   └── NotificationCard.razor
└── Shared/
    ├── CardWidget.razor
    └── EmptyCard.razor
```

---

### Task 2.4: Implement UserProfile Component
**Estimated Time**: 45 minutes

**Objective**: Display user avatar and profile information.

**File**: `src/WebApp/Components/Settings/UserProfile.razor`

**Implementation**: Complete code in `SVELTE_TO_BLAZOR_GUIDE.md` - Component 1.

**Testing**:
- ✓ Avatar displays correctly
- ✓ Title and subtitle render
- ✓ ChildContent slot works
- ✓ Dark mode styling correct

---

### Task 2.5: Implement GeneralInfo Component
**Estimated Time**: 2 hours

**Objective**: Form for user's general information with validation.

**File**: `src/WebApp/Components/Settings/GeneralInfo.razor`

**Implementation**: Complete code in `SVELTE_TO_BLAZOR_GUIDE.md` - Component 2.

**Key Features**:
- EditForm with DataAnnotationsValidator
- 12 input fields (dynamic rendering)
- Validation messages
- Submit handler

**Testing**:
- ✓ All fields render correctly
- ✓ Validation works (required, email, phone)
- ✓ Form submits with valid data
- ✓ Validation messages display
- ✓ Textarea renders for Address field

---

### Task 2.6: Implement PasswordInfo Component
**Estimated Time**: 1 hour

**Objective**: Password change form with validation.

**File**: `src/WebApp/Components/Settings/PasswordInfo.razor`

**Implementation**:

```razor
@using Flowbite.Components
@using System.ComponentModel.DataAnnotations

<Card Size="CardSize.ExtraLarge" class="shadow-sm">
    <div class="mb-4">
        <Heading Tag="HeadingTag.H3" class="text-xl font-semibold">Password information</Heading>
    </div>
    <EditForm Model="@Model" OnValidSubmit="@HandleSubmit">
        <DataAnnotationsValidator />
        <div class="grid grid-cols-6 gap-6">
            <div class="col-span-6 sm:col-span-3">
                <Label For="current-password" Value="Current password" class="mb-2 block" />
                <TextInput TValue="string"
                          Id="current-password"
                          Type="password"
                          @bind-Value="@Model.CurrentPassword"
                          Placeholder="••••••••" />
                <ValidationMessage For="@(() => Model.CurrentPassword)" />
            </div>
            <div class="col-span-6 sm:col-span-3"></div>
            <div class="col-span-6 sm:col-span-3">
                <Label For="new-password" Value="New password" class="mb-2 block" />
                <TextInput TValue="string"
                          Id="new-password"
                          Type="password"
                          @bind-Value="@Model.NewPassword"
                          Placeholder="••••••••" />
                <ValidationMessage For="@(() => Model.NewPassword)" />
            </div>
            <div class="col-span-6 sm:col-span-3">
                <Label For="confirm-password" Value="Confirm password" class="mb-2 block" />
                <TextInput TValue="string"
                          Id="confirm-password"
                          Type="password"
                          @bind-Value="@Model.ConfirmPassword"
                          Placeholder="••••••••" />
                <ValidationMessage For="@(() => Model.ConfirmPassword)" />
            </div>
            <div class="col-span-6 sm:col-full">
                <Button Type="submit">Save all</Button>
            </div>
        </div>
    </EditForm>
</Card>

@code {
    [Parameter]
    public EventCallback<PasswordChangeModel> OnSubmit { get; set; }

    private PasswordChangeModel Model { get; set; } = new();

    private async Task HandleSubmit()
    {
        await OnSubmit.InvokeAsync(Model);
        Model = new(); // Reset form
    }

    public class PasswordChangeModel
    {
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string CurrentPassword { get; set; } = "";

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string NewPassword { get; set; } = "";

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = "";
    }
}
```

**Testing**:
- ✓ All password fields render
- ✓ MinLength validation works
- ✓ Compare validation works
- ✓ Form resets after submit

---

### Task 2.7: Implement LanguageTime Component
**Estimated Time**: 1 hour

**Objective**: Language and timezone selection.

**File**: `src/WebApp/Components/Settings/LanguageTime.razor`

**Implementation**: Complete code in `SVELTE_TO_BLAZOR_GUIDE.md` - Component 3.

**Testing**:
- ✓ Language dropdown populated
- ✓ Timezone dropdown populated
- ✓ Two-way binding works
- ✓ ChildContent slot renders

---

### Task 2.8: Implement SocialAccounts Component
**Estimated Time**: 1 hour

**Objective**: Social media account connections.

**File**: `src/WebApp/Components/Settings/SocialAccounts.razor`

**Implementation**:

```razor
@using Flowbite.Components
@using Flowbite.Icons
@using WebApp.Domain

<Card Size="CardSize.ExtraLarge" class="shadow-sm">
    <Heading Tag="HeadingTag.H3" class="mb-4 text-xl font-semibold">Social accounts</Heading>
    <div class="space-y-4">
        @foreach (var account in Accounts)
        {
            <div class="flex items-center justify-between border-b pb-4 last:border-0">
                <div class="flex items-center gap-4">
                    @if (account.Icon != null)
                    {
                        <DynamicComponent Type="@account.Icon"
                                        Parameters="@(new Dictionary<string, object> {
                                            { "Class", "w-8 h-8" }
                                        })" />
                    }
                    <div>
                        <div class="font-semibold text-gray-900 dark:text-white">
                            @account.Platform
                        </div>
                        <div class="text-sm text-gray-500 dark:text-gray-400">
                            @(account.IsConnected ? account.Username : "Not connected")
                        </div>
                    </div>
                </div>
                <Button Color="@(account.IsConnected ? ButtonColor.Light : ButtonColor.Primary)"
                       Size="ButtonSize.ExtraSmall"
                       OnClick="@(() => ToggleConnection(account))">
                    @(account.IsConnected ? "Disconnect" : "Connect")
                </Button>
            </div>
        }
    </div>
</Card>

@code {
    [Parameter]
    public List<SocialAccount> Accounts { get; set; } = new();

    [Parameter]
    public EventCallback<SocialAccount> OnToggle { get; set; }

    private async Task ToggleConnection(SocialAccount account)
    {
        account.IsConnected = !account.IsConnected;
        await OnToggle.InvokeAsync(account);
    }
}
```

**Testing**:
- ✓ All platforms render
- ✓ Icons display
- ✓ Connect/disconnect toggle works
- ✓ Event callback fires

---

### Task 2.9: Implement Accounts Component
**Estimated Time**: 1 hour

**Objective**: Other user accounts list.

**File**: `src/WebApp/Components/Settings/Accounts.razor`

**Implementation**:

```razor
@using Flowbite.Components
@using WebApp.Domain

<Card Size="CardSize.ExtraLarge" class="shadow-sm">
    @if (!string.IsNullOrEmpty(Title))
    {
        <Heading Tag="HeadingTag.H3" class="mb-4 text-xl font-semibold">@Title</Heading>
    }
    <ul class="divide-y divide-gray-200 dark:divide-gray-700">
        @foreach (var user in Users)
        {
            <li class="py-4 flex items-center justify-between">
                <div class="flex items-center gap-4">
                    <Avatar Size="AvatarSize.Medium"
                           Image="@user.Avatar"
                           Alt="@user.Name"
                           Rounded="true" />
                    <div>
                        <div class="font-semibold text-gray-900 dark:text-white">
                            @user.Name
                        </div>
                        <div class="text-sm text-gray-500 dark:text-gray-400">
                            @user.Country
                        </div>
                    </div>
                </div>
                @if (ChildContent != null)
                {
                    @ChildContent(user)
                }
            </li>
        }
    </ul>
</Card>

@code {
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public List<UserAccount> Users { get; set; } = new();

    [Parameter]
    public RenderFragment<UserAccount>? ChildContent { get; set; }
}
```

**Testing**:
- ✓ User list renders
- ✓ Avatars display
- ✓ ChildContent template works

---

### Task 2.10: Implement Sessions Component
**Estimated Time**: 1 hour

**Objective**: Active device sessions.

**File**: `src/WebApp/Components/Settings/Sessions.razor`

**Implementation**: Complete code in `SVELTE_TO_BLAZOR_GUIDE.md` - Component 4.

**Testing**:
- ✓ Session list renders
- ✓ Icons display correctly
- ✓ Revoke button works
- ✓ "See all" link renders

---

### Task 2.11: Implement NotificationCard Component
**Estimated Time**: 1 hour

**Objective**: Notification preferences with toggles.

**File**: `src/WebApp/Components/Settings/NotificationCard.razor`

**Implementation**: Complete code in `SVELTE_TO_BLAZOR_GUIDE.md` - Component 5.

**Testing**:
- ✓ All notification items render
- ✓ ToggleSwitch binds correctly
- ✓ OnToggleChanged callback fires
- ✓ State persists during session

---

### Task 2.12: Create Settings Page
**Estimated Time**: 2 hours

**Objective**: Assemble all components into Settings page.

**File**: `src/WebApp/Pages/Settings.razor`

**Implementation**:

```razor
@page "/settings"
@using WebApp.Components.Settings
@using WebApp.Domain
@using WebApp.Services
@inject SettingsService SettingsService
@implements IDisposable

<div class="grid grid-cols-1 px-4 pt-6 dark:bg-gray-900">
    <div class="mb-4">
        <Breadcrumb class="mb-5">
            <BreadcrumbItem Href="/">Home</BreadcrumbItem>
            <BreadcrumbItem Href="/settings">Settings</BreadcrumbItem>
        </Breadcrumb>
        <Heading Tag="HeadingTag.H1" class="text-xl font-semibold sm:text-2xl">
            User settings
        </Heading>
    </div>

    <div class="grid grid-cols-1 space-y-2 xl:grid-cols-3 xl:gap-3.5">
        @* Left Column *@
        <div class="space-y-4 xl:col-auto">
            <UserProfile Src="@userAvatar"
                        Title="@($"{_settings.FirstName} {_settings.LastName}")"
                        Subtitle="@_settings.Email" />

            <LanguageTime @bind-SelectedLanguage="_selectedLanguage"
                         @bind-SelectedTimezone="_selectedTimezone" />

            <SocialAccounts Accounts="@_socialAccounts"
                           OnToggle="@HandleSocialToggle" />

            <Accounts Title="Other accounts"
                     Users="@_userAccounts">
                <ChildContent Context="user">
                    <Button Color="ButtonColor.Light" Size="ButtonSize.ExtraSmall">
                        View
                    </Button>
                </ChildContent>
            </Accounts>
        </div>

        @* Right Column *@
        <div class="space-y-4 xl:col-span-2">
            <GeneralInfo Model="@_settings"
                        OnSubmit="@HandleSettingsSave" />

            <PasswordInfo OnSubmit="@HandlePasswordChange" />

            <Sessions SessionList="@_sessions"
                     SeeMoreHref="/settings/sessions" />
        </div>

        @* Full Width *@
        <div class="col-span-full space-y-4">
            <NotificationCard Title="Email notifications"
                             Subtitle="Select the types of email notifications you'd like to receive"
                             Items="@_emailNotifications"
                             OnToggleChanged="@HandleNotificationToggle" />

            <NotificationCard Title="Push notifications"
                             Subtitle="Select the types of push notifications you'd like to receive"
                             Items="@_pushNotifications"
                             OnToggleChanged="@HandleNotificationToggle" />
        </div>
    </div>
</div>

@code {
    private UserSettings _settings = new();
    private List<SocialAccount> _socialAccounts = new();
    private List<UserAccount> _userAccounts = new();
    private List<SessionInfo> _sessions = new();
    private List<NotificationItem> _emailNotifications = new();
    private List<NotificationItem> _pushNotifications = new();

    private string _selectedLanguage = "en-US";
    private string _selectedTimezone = "America/Los_Angeles";
    private string userAvatar = "/images/users/bonnie-green.png";

    protected override async Task OnInitializedAsync()
    {
        SettingsService.OnChange += StateHasChanged;

        _settings = SettingsService.Settings;
        _sessions = SettingsService.Sessions;

        _socialAccounts = await SettingsService.GetSocialAccountsAsync();
        _userAccounts = await SettingsService.GetUserAccountsAsync();

        _emailNotifications = new List<NotificationItem>
        {
            new() { Title = "Rating reminders", Subtitle = "Send email reminders about ratings", Active = true },
            new() { Title = "Item update notifications", Subtitle = "Notifications about item updates", Active = false },
            new() { Title = "Item comment notifications", Subtitle = "Notifications about item comments", Active = true },
            new() { Title = "Buyer review notifications", Subtitle = "Notifications about buyer reviews", Active = false }
        };

        _pushNotifications = new List<NotificationItem>
        {
            new() { Title = "Company news", Subtitle = "Get notified about company news", Active = true },
            new() { Title = "Account activity", Subtitle = "Get notified about account activity", Active = true },
            new() { Title = "Meetups near you", Subtitle = "Notifications about meetups", Active = false },
            new() { Title = "New messages", Subtitle = "Get notified about new messages", Active = true }
        };
    }

    private async Task HandleSettingsSave(UserSettings settings)
    {
        await SettingsService.SaveSettingsAsync(settings);
        // Show toast notification
    }

    private async Task HandlePasswordChange(PasswordInfo.PasswordChangeModel model)
    {
        // Implement password change logic
        await Task.Delay(100);
        // Show success toast
    }

    private void HandleSocialToggle(SocialAccount account)
    {
        // Handle social account toggle
    }

    private void HandleNotificationToggle(NotificationItem item)
    {
        SettingsService.UpdateNotification(item);
    }

    public void Dispose()
    {
        SettingsService.OnChange -= StateHasChanged;
    }
}
```

**Testing**:
- ✓ Page loads without errors
- ✓ All components render
- ✓ Layout responsive at all breakpoints
- ✓ State changes reflect in UI
- ✓ Form submissions work
- ✓ Service integration works

---

### Task 2.13: Add Settings to Navigation
**Estimated Time**: 15 minutes

**Objective**: Add Settings link to sidebar navigation.

**File**: `src/WebApp/Layout/AppSidebar.razor`

**Implementation**:
```razor
<SidebarItem Href="/settings" Icon="@(new CogIcon())">
    Settings
</SidebarItem>
```

**Testing**:
- ✓ Settings link appears in sidebar
- ✓ Navigation to /settings works
- ✓ Active state highlights correctly

---

## Phase 3: Playground Pages Implementation

### Overview
Implement two Playground pages demonstrating different layout patterns.

---

### Task 3.1: Create EmptyCard Component
**Estimated Time**: 30 minutes

**Objective**: Placeholder card for wireframing.

**File**: `src/WebApp/Components/Shared/EmptyCard.razor`

**Implementation**: See `SVELTE_TO_BLAZOR_GUIDE.md` - Pattern 1.

**Testing**:
- ✓ Card renders with dashed borders
- ✓ Size parameter works
- ✓ Class parameter applies

---

### Task 3.2: Create Playground Component
**Estimated Time**: 1 hour

**Objective**: Reusable playground layout component.

**File**: `src/WebApp/Components/Playground/Playground.razor`

**Implementation**: Complete code in `SVELTE_TO_BLAZOR_GUIDE.md` - Playground Component.

**Testing**:
- ✓ Grid layout renders correctly
- ✓ Breadcrumb slot works
- ✓ Title displays
- ✓ Empty cards positioned correctly
- ✓ Responsive breakpoints work

---

### Task 3.3: Create Sidebar Playground Page
**Estimated Time**: 30 minutes

**Objective**: Playground page with sidebar layout.

**File**: `src/WebApp/Pages/PlaygroundSidebar.razor`

**Implementation**:

```razor
@page "/playground/sidebar"
@layout MainLayout
@using WebApp.Components.Playground

<div id="main-content" class="relative mx-auto h-full w-full overflow-y-auto bg-gray-50 p-4 dark:bg-gray-900">
    <Playground Title="Create something awesome here" />
</div>
```

**Testing**:
- ✓ Page loads with MainLayout (sidebar visible)
- ✓ Playground component renders
- ✓ Full-width container
- ✓ Dark mode styling correct

---

### Task 3.4: Create Stacked Playground Page
**Estimated Time**: 45 minutes

**Objective**: Playground page with stacked layout (no sidebar).

**File**: `src/WebApp/Pages/PlaygroundStacked.razor`

**Implementation**:

```razor
@page "/playground/stacked"
@layout StackedLayout
@using WebApp.Components.Playground
@using Flowbite.Components

<div id="main-content" class="relative mx-auto h-full w-full max-w-screen-2xl overflow-y-auto bg-gray-50 p-4 dark:bg-gray-900">
    <Playground Title="Create something awesome here">
        <Breadcrumb>
            <Breadcrumb class="mb-5">
                <BreadcrumbItem Href="/">Home</BreadcrumbItem>
                <BreadcrumbItem Href="/playground">Playground</BreadcrumbItem>
                <BreadcrumbItem>Stacked</BreadcrumbItem>
            </Breadcrumb>
        </Breadcrumb>
    </Playground>
</div>
```

**Testing**:
- ✓ Page loads with StackedLayout (no sidebar)
- ✓ Breadcrumb renders in slot
- ✓ Max-width constraint applied
- ✓ Full-width on mobile

---

### Task 3.5: Add Playground Links to Navigation
**Estimated Time**: 15 minutes

**Objective**: Add Playground section to sidebar.

**File**: `src/WebApp/Layout/AppSidebar.razor`

**Implementation**:

```razor
<SidebarCollapse Label="Playground" Icon="@(new WandMagicSparklesIcon())">
    <SidebarItem Href="/playground/sidebar" Icon="@(new GridIcon())">
        Sidebar Layout
    </SidebarItem>
    <SidebarItem Href="/playground/stacked" Icon="@(new LayersIcon())">
        Stacked Layout
    </SidebarItem>
</SidebarCollapse>
```

**Testing**:
- ✓ Playground dropdown in sidebar
- ✓ Both links navigate correctly
- ✓ Icons display
- ✓ Active state works

---

## Testing Checklist

### Phase 1: Layouts
- [ ] MainLayout responsive behavior
- [ ] StackedLayout renders without sidebar
- [ ] Mobile menu toggle works
- [ ] Dark mode consistent across layouts
- [ ] Sidebar transitions smooth
- [ ] Navigation closes mobile menu

### Phase 2: Settings Page
- [ ] All 8 components render correctly
- [ ] Forms submit and validate
- [ ] Two-way binding works (toggles, selects)
- [ ] Service state management functional
- [ ] Responsive at xs, sm, md, lg, xl breakpoints
- [ ] Dark mode styling correct
- [ ] Navigation to/from Settings works

### Phase 3: Playground Pages
- [ ] Sidebar playground uses MainLayout
- [ ] Stacked playground uses StackedLayout
- [ ] EmptyCard component renders
- [ ] Breadcrumb slot works in stacked
- [ ] Grid layouts responsive
- [ ] Navigation between playgrounds works

---

## Deployment Checklist

Before considering complete:

1. **Code Quality**:
   - [ ] No compiler warnings
   - [ ] No unused using statements
   - [ ] Consistent naming conventions
   - [ ] Code comments for complex logic

2. **Functionality**:
   - [ ] All pages load without errors
   - [ ] Forms validate correctly
   - [ ] State persists during session
   - [ ] Events fire appropriately

3. **Responsiveness**:
   - [ ] Test on mobile (< 640px)
   - [ ] Test on tablet (640px - 1024px)
   - [ ] Test on desktop (> 1024px)
   - [ ] Test on ultra-wide (> 1536px)

4. **Accessibility**:
   - [ ] ARIA labels present
   - [ ] Keyboard navigation works
   - [ ] Focus indicators visible
   - [ ] Color contrast sufficient

5. **Performance**:
   - [ ] Initial load time acceptable
   - [ ] No console errors
   - [ ] Images optimized
   - [ ] CSS minified in production

6. **Documentation**:
   - [ ] Update CLAUDE.md with new pages
   - [ ] Document any custom patterns
   - [ ] Add component usage examples
   - [ ] Update README if needed

---

## Estimated Timeline

| Phase | Tasks | Estimated Hours |
|-------|-------|-----------------|
| **Phase 1: Layouts** | 5 tasks | 7-9 hours |
| **Phase 2: Settings** | 13 tasks | 15-18 hours |
| **Phase 3: Playground** | 5 tasks | 3-4 hours |
| **Testing & Polish** | - | 2-3 hours |
| **Total** | 23 tasks | **27-34 hours** |

**For an experienced Blazor developer**: 20-24 hours
**For a developer new to Flowbite Blazor**: 30-40 hours

---

## Success Criteria

The implementation is complete when:

1. ✅ All 23 tasks completed
2. ✅ Settings page matches Svelte version functionality
3. ✅ Both Playground pages render correctly
4. ✅ All tests in checklist pass
5. ✅ Code reviewed and approved
6. ✅ Documentation updated
7. ✅ Works in latest Chrome, Firefox, Safari, Edge
8. ✅ No breaking changes to existing pages

---

## Troubleshooting Guide

### Issue: Components not rendering
**Solution**: Check `_Imports.razor` has correct using statements

### Issue: Flowbite components missing props
**Solution**: Check Flowbite Blazor version (alpha limitations)

### Issue: Dark mode not working
**Solution**: Verify `dark` class on `<html>` element

### Issue: Layout not responsive
**Solution**: Check Tailwind breakpoint classes (md:, lg:, xl:)

### Issue: Forms not validating
**Solution**: Ensure `DataAnnotationsValidator` in `EditForm`

### Issue: State not updating
**Solution**: Call `StateHasChanged()` after state mutation

### Issue: Services not injecting
**Solution**: Check service registration in `Program.cs`

### Issue: Icons not displaying
**Solution**: Ensure `Flowbite.Icons` or `Flowbite.Icons.Extended` imported

---

## Next Steps

After completing this implementation:

1. **Add more pages**: Dashboard, CRUD pages, Authentication
2. **Integrate charts**: Add ApexCharts or similar library
3. **Add real data**: Connect to backend API
4. **Implement authentication**: Add Identity or Auth0
5. **Add tests**: Unit tests for services, integration tests for pages
6. **Optimize performance**: Lazy loading, code splitting
7. **Enhance accessibility**: WCAG compliance audit
8. **Create component library**: Package reusable components

---

## Reference Documentation

- [Blazor Components Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/)
- [Flowbite Blazor GitHub](https://github.com/peakflames/flowbite-blazor)
- [Tailwind CSS v3 Documentation](https://v3.tailwindcss.com/)
- [Flowbite Components](https://flowbite.com/docs/components/)
- Original Svelte Dashboard: `/mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard`
- Implementation Guide: `docs/SVELTE_TO_BLAZOR_GUIDE.md`
- Architecture Comparison: `docs/ARCHITECTURE_COMPARISON.md`

---

## Contact and Support

For questions or issues during implementation:
1. Review this plan and referenced documentation
2. Check `ARCHITECTURE_COMPARISON.md` for paradigm differences
3. Check `SVELTE_TO_BLAZOR_GUIDE.md` for code patterns
4. Consult Flowbite Blazor documentation at `/mnt/c/Users/tschavey/projects/themesberg/flowbite-blazor/src/DemoApp/wwwroot/llms-ctx.md`
5. Raise issues with the development team

Good luck with the implementation! 🚀
