# Svelte to Blazor Implementation Guide

## Table of Contents
1. [Component Migration Patterns](#component-migration-patterns)
2. [Settings Page Components](#settings-page-components)
3. [Playground Page Components](#playground-page-components)
4. [Data Models and Services](#data-models-and-services)
5. [Common Patterns Reference](#common-patterns-reference)

---

## Component Migration Patterns

### Pattern 1: Simple Display Component

#### Svelte: EmptyCard.svelte
```svelte
<script lang="ts">
  import { Card } from 'flowbite-svelte';
  import type { EmptyCardProps } from './types';

  let { size, class: className, ...restProps }: Empty

CardProps = $props();
</script>

<Card {size} class={className} {...restProps}>
  <div class="rounded border border-dashed border-gray-200">Card header</div>
  <div class="h-full rounded border border-dashed border-gray-200">Card body</div>
  <div class="rounded border border-dashed border-gray-200">Card footer</div>
</Card>
```

#### Blazor: EmptyCard.razor
```razor
@using Flowbite.Components

<Card Size="@Size" class="@Class">
    <div class="rounded border border-dashed border-gray-200">Card header</div>
    <div class="h-full rounded border border-dashed border-gray-200">Card body</div>
    <div class="rounded border border-dashed border-gray-200">Card footer</div>
</Card>

@code {
    [Parameter]
    public CardSize? Size { get; set; }

    [Parameter]
    public string Class { get; set; } = "";

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
```

**Translation Notes**:
- `$props()` → `[Parameter]` properties
- `...restProps` → `[Parameter(CaptureUnmatchedValues = true)]`
- `class` reserved in C#, use `Class` property
- Flowbite Blazor uses enums for size (`CardSize`)

---

### Pattern 2: Component with Content Slots

#### Svelte: CardWidget.svelte
```svelte
<script lang="ts">
  import { Card, Heading } from 'flowbite-svelte';
  import type { Snippet } from 'svelte';
  import type { CardWidgetProps } from './types';

  let { children, title, subtitle, class: className = '' }: CardWidgetProps = $props();
</script>

<Card size="xl" class="shadow-sm {className}">
  <div class="mb-4">
    {#if title}
      <Heading tag="h3" class="text-xl font-semibold">{title}</Heading>
    {/if}
    {#if subtitle}
      <p class="text-sm text-gray-500 dark:text-gray-300">{subtitle}</p>
    {/if}
  </div>
  {@render children()}
</Card>
```

#### Blazor: CardWidget.razor
```razor
@using Flowbite.Components

<Card Size="CardSize.ExtraLarge" class="shadow-sm @Class">
    <div class="mb-4">
        @if (!string.IsNullOrEmpty(Title))
        {
            <Heading Tag="HeadingTag.H3" class="text-xl font-semibold">@Title</Heading>
        }
        @if (!string.IsNullOrEmpty(Subtitle))
        {
            <p class="text-sm text-gray-500 dark:text-gray-300">@Subtitle</p>
        }
    </div>
    @ChildContent
</Card>

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? Subtitle { get; set; }

    [Parameter]
    public string Class { get; set; } = "";
}
```

**Translation Notes**:
- `children: Snippet` → `ChildContent: RenderFragment`
- `{@render children()}` → `@ChildContent`
- String props nullable in C# (`string?`)
- Heading tag uses enum (`HeadingTag.H3`)

---

### Pattern 3: Component with Multiple Named Slots

#### Svelte: ActivityList.svelte
```svelte
<script lang="ts">
  import { Card, Heading } from 'flowbite-svelte';
  import type { Snippet } from 'svelte';
  import type { ActivityListProps } from './types';

  let { title, children, actions }: ActivityListProps = $props();
</script>

<Card size="xl" class="shadow-sm">
  <div class="mb-4 flex items-center justify-between">
    {#if title}
      <Heading tag="h3" class="text-xl font-semibold">{title}</Heading>
    {/if}
    {#if actions}
      {@render actions()}
    {/if}
  </div>
  {@render children()}
</Card>
```

#### Blazor: ActivityList.razor
```razor
@using Flowbite.Components

<Card Size="CardSize.ExtraLarge" class="shadow-sm">
    <div class="mb-4 flex items-center justify-between">
        @if (!string.IsNullOrEmpty(Title))
        {
            <Heading Tag="HeadingTag.H3" class="text-xl font-semibold">@Title</Heading>
        }
        @if (Actions != null)
        {
            @Actions
        }
    </div>
    @ChildContent
</Card>

@code {
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public RenderFragment? Actions { get; set; }
}
```

**Usage**:
```razor
<ActivityList Title="Recent Activity">
    <Actions>
        <Button Size="ButtonSize.Small">View All</Button>
    </Actions>
    <ChildContent>
        <ul>
            <li>Activity 1</li>
            <li>Activity 2</li>
        </ul>
    </ChildContent>
</ActivityList>
```

---

### Pattern 4: Component with Parameterized Slots

#### Svelte: NotificationCard.svelte (simplified)
```svelte
<script lang="ts">
  import type { Snippet } from 'svelte';

  interface Props {
    items: { title: string; active: boolean }[];
    itemSlot?: Snippet<[{ title: string; active: boolean }]>;
  }

  let { items, itemSlot }: Props = $props();
</script>

<ul>
  {#each items as item}
    <li>
      {#if itemSlot}
        {@render itemSlot(item)}
      {:else}
        {item.title}
      {/if}
    </li>
  {/each}
</ul>
```

#### Blazor: NotificationCard.razor
```razor
<ul>
    @foreach (var item in Items)
    {
        <li>
            @if (ItemTemplate != null)
            {
                @ItemTemplate(item)
            }
            else
            {
                @item.Title
            }
        </li>
    }
</ul>

@code {
    [Parameter]
    public List<NotificationItem> Items { get; set; } = new();

    [Parameter]
    public RenderFragment<NotificationItem>? ItemTemplate { get; set; }
}
```

**Usage**:
```razor
<NotificationCard Items="@notificationItems">
    <ItemTemplate Context="item">
        <strong>@item.Title</strong>
        <ToggleSwitch @bind-Checked="item.Active" />
    </ItemTemplate>
</NotificationCard>
```

---

## Settings Page Components

### Component 1: UserProfile

#### Svelte Implementation
**File**: `src/lib/UserProfile.svelte`
```svelte
<script lang="ts">
  import { Card, Avatar, Heading } from 'flowbite-svelte';
  import type { Snippet } from 'svelte';
  import type { UserProfileProps } from './types';

  let { children, src, title, subtitle }: UserProfileProps = $props();
</script>

<Card size="xl" class="shadow-sm">
  <div class="flex flex-col items-center pb-6">
    <Avatar size="lg" {src} alt="User avatar" class="mb-3" />
    {#if title}
      <Heading tag="h3" class="mb-1 text-xl font-medium text-gray-900 dark:text-white">
        {title}
      </Heading>
    {/if}
    {#if subtitle}
      <span class="text-sm text-gray-500 dark:text-gray-400">{subtitle}</span>
    {/if}
    {#if children}
      <div class="mt-4 w-full">
        {@render children()}
      </div>
    {/if}
  </div>
</Card>
```

#### Blazor Implementation
**File**: `src/WebApp/Components/Settings/UserProfile.razor`
```razor
@using Flowbite.Components

<Card Size="CardSize.ExtraLarge" class="shadow-sm">
    <div class="flex flex-col items-center pb-6">
        <Avatar Size="AvatarSize.Large" Image="@Src" Alt="User avatar" class="mb-3" Rounded="true" />
        @if (!string.IsNullOrEmpty(Title))
        {
            <Heading Tag="HeadingTag.H3" class="mb-1 text-xl font-medium text-gray-900 dark:text-white">
                @Title
            </Heading>
        }
        @if (!string.IsNullOrEmpty(Subtitle))
        {
            <span class="text-sm text-gray-500 dark:text-gray-400">@Subtitle</span>
        }
        @if (ChildContent != null)
        {
            <div class="mt-4 w-full">
                @ChildContent
            </div>
        }
    </div>
</Card>

@code {
    [Parameter]
    public string Src { get; set; } = "";

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? Subtitle { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
```

---

### Component 2: GeneralInfo

#### Svelte Implementation
**File**: `src/lib/GeneralInfo.svelte`
```svelte
<script lang="ts">
  import { Card, Heading, Label, Input, Textarea, Button } from 'flowbite-svelte';
  import type { GeneralInfoProps, InputField } from './types';

  let { inputs }: GeneralInfoProps = $props();
</script>

<Card size="xl" class="shadow-sm">
  <div class="mb-4">
    <Heading tag="h3" class="text-xl font-semibold">General information</Heading>
  </div>
  <form>
    <div class="grid grid-cols-6 gap-6">
      {#each inputs as input}
        <div class="col-span-6 sm:col-span-3">
          <Label for={input.label.toLowerCase()} class="mb-2 block">
            {input.label}
          </Label>
          {#if input.type === 'textarea'}
            <Textarea id={input.label.toLowerCase()} placeholder={input.placeholder} rows="4" />
          {:else}
            <Input
              type={input.type}
              id={input.label.toLowerCase()}
              placeholder={input.placeholder}
            />
          {/if}
        </div>
      {/each}
      <div class="col-span-6 sm:col-full">
        <Button type="submit">Save all</Button>
      </div>
    </div>
  </form>
</Card>
```

#### Blazor Implementation
**File**: `src/WebApp/Components/Settings/GeneralInfo.razor`
```razor
@using Flowbite.Components
@using WebApp.Domain

<Card Size="CardSize.ExtraLarge" class="shadow-sm">
    <div class="mb-4">
        <Heading Tag="HeadingTag.H3" class="text-xl font-semibold">General information</Heading>
    </div>
    <EditForm Model="@Model" OnValidSubmit="@HandleSubmit">
        <DataAnnotationsValidator />
        <div class="grid grid-cols-6 gap-6">
            @foreach (var field in InputFields)
            {
                <div class="col-span-6 sm:col-span-3">
                    <Label For="@field.Id" Value="@field.Label" class="mb-2 block" />
                    @if (field.Type == "textarea")
                    {
                        <Textarea Id="@field.Id"
                                 @bind-Value="@GetFieldValue(field.PropertyName)"
                                 Placeholder="@field.Placeholder"
                                 Rows="4" />
                    }
                    else
                    {
                        <TextInput TValue="string"
                                  Id="@field.Id"
                                  Type="@field.Type"
                                  @bind-Value="@GetFieldValue(field.PropertyName)"
                                  Placeholder="@field.Placeholder" />
                    }
                    <ValidationMessage For="@(() => GetFieldValue(field.PropertyName))" />
                </div>
            }
            <div class="col-span-6 sm:col-full">
                <Button Type="submit">Save all</Button>
            </div>
        </div>
    </EditForm>
</Card>

@code {
    [Parameter]
    public UserSettings Model { get; set; } = new();

    [Parameter]
    public EventCallback<UserSettings> OnSubmit { get; set; }

    private List<InputFieldDefinition> InputFields { get; set; } = new()
    {
        new() { Id = "first-name", Label = "First Name", PropertyName = nameof(UserSettings.FirstName), Type = "text", Placeholder = "Bonnie" },
        new() { Id = "last-name", Label = "Last Name", PropertyName = nameof(UserSettings.LastName), Type = "text", Placeholder = "Green" },
        new() { Id = "email", Label = "Email", PropertyName = nameof(UserSettings.Email), Type = "email", Placeholder = "bonnie.green@company.com" },
        new() { Id = "phone", Label = "Phone Number", PropertyName = nameof(UserSettings.Phone), Type = "tel", Placeholder = "+(12) 345 6789" },
        new() { Id = "birthday", Label = "Birthday", PropertyName = nameof(UserSettings.Birthday), Type = "date", Placeholder = "" },
        new() { Id = "organization", Label = "Organization", PropertyName = nameof(UserSettings.Organization), Type = "text", Placeholder = "Company Name" },
        new() { Id = "role", Label = "Role", PropertyName = nameof(UserSettings.Role), Type = "text", Placeholder = "React Developer" },
        new() { Id = "department", Label = "Department", PropertyName = nameof(UserSettings.Department), Type = "text", Placeholder = "Development" },
        new() { Id = "zip", Label = "Zip/postal code", PropertyName = nameof(UserSettings.ZipCode), Type = "text", Placeholder = "123456" },
        new() { Id = "country", Label = "Country", PropertyName = nameof(UserSettings.Country), Type = "text", Placeholder = "United States" },
        new() { Id = "city", Label = "City", PropertyName = nameof(UserSettings.City), Type = "text", Placeholder = "New York" },
        new() { Id = "address", Label = "Address", PropertyName = nameof(UserSettings.Address), Type = "textarea", Placeholder = "Enter your address" }
    };

    private string GetFieldValue(string propertyName)
    {
        var property = typeof(UserSettings).GetProperty(propertyName);
        return property?.GetValue(Model)?.ToString() ?? "";
    }

    private async Task HandleSubmit()
    {
        await OnSubmit.InvokeAsync(Model);
    }

    private class InputFieldDefinition
    {
        public string Id { get; set; } = "";
        public string Label { get; set; } = "";
        public string PropertyName { get; set; } = "";
        public string Type { get; set; } = "text";
        public string Placeholder { get; set; } = "";
    }
}
```

**Data Model**: `src/WebApp/Domain/UserSettings.cs`
```csharp
using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class UserSettings
{
    [Required]
    public string FirstName { get; set; } = "";

    [Required]
    public string LastName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Phone]
    public string Phone { get; set; } = "";

    public string? Birthday { get; set; }

    public string Organization { get; set; } = "";

    public string Role { get; set; } = "";

    public string Department { get; set; } = "";

    public string ZipCode { get; set; } = "";

    public string Country { get; set; } = "";

    public string City { get; set; } = "";

    public string Address { get; set; } = "";
}
```

---

### Component 3: LanguageTime

#### Svelte Implementation
**File**: `src/lib/LanguageTime.svelte`
```svelte
<script lang="ts">
  import { Card, Heading, Label, Select, Button } from 'flowbite-svelte';
  import type { Snippet } from 'svelte';
  import type { LanguageTimeProps } from './types';

  let {
    children,
    timezones = DEFAULT_TIMEZONES,
    languages = DEFAULT_LANGUAGES
  }: LanguageTimeProps = $props();

  let selectedLanguage = $state(languages[0].value);
  let selectedTimezone = $state(timezones[0].value);
</script>

<Card size="xl" class="shadow-sm">
  <Heading tag="h3" class="mb-4 text-xl font-semibold">Language & Time</Heading>
  <form>
    <div class="grid grid-cols-6 gap-6">
      <div class="col-span-6 sm:col-span-3">
        <Label for="language" class="mb-2 block">Select language</Label>
        <Select id="language" bind:value={selectedLanguage}>
          {#each languages as lang}
            <option value={lang.value}>{lang.name}</option>
          {/each}
        </Select>
      </div>
      <div class="col-span-6 sm:col-span-3">
        <Label for="timezone" class="mb-2 block">Time Zone</Label>
        <Select id="timezone" bind:value={selectedTimezone}>
          {#each timezones as tz}
            <option value={tz.value}>{tz.name}</option>
          {/each}
        </Select>
      </div>
      {#if children}
        <div class="col-span-6">
          {@render children()}
        </div>
      {/if}
    </div>
  </form>
</Card>
```

#### Blazor Implementation
**File**: `src/WebApp/Components/Settings/LanguageTime.razor`
```razor
@using Flowbite.Components
@using WebApp.Domain

<Card Size="CardSize.ExtraLarge" class="shadow-sm">
    <Heading Tag="HeadingTag.H3" class="mb-4 text-xl font-semibold">Language & Time</Heading>
    <form>
        <div class="grid grid-cols-6 gap-6">
            <div class="col-span-6 sm:col-span-3">
                <Label For="language" Value="Select language" class="mb-2 block" />
                <Select Id="language" @bind-Value="SelectedLanguage">
                    @foreach (var lang in Languages)
                    {
                        <option value="@lang.Value">@lang.Name</option>
                    }
                </Select>
            </div>
            <div class="col-span-6 sm:col-span-3">
                <Label For="timezone" Value="Time Zone" class="mb-2 block" />
                <Select Id="timezone" @bind-Value="SelectedTimezone">
                    @foreach (var tz in Timezones)
                    {
                        <option value="@tz.Value">@tz.Name</option>
                    }
                </Select>
            </div>
            @if (ChildContent != null)
            {
                <div class="col-span-6">
                    @ChildContent
                </div>
            }
        </div>
    </form>
</Card>

@code {
    [Parameter]
    public List<SelectOption> Languages { get; set; } = new()
    {
        new() { Name = "English (US)", Value = "en-US" },
        new() { Name = "Español", Value = "es" },
        new() { Name = "Français", Value = "fr" },
        new() { Name = "Deutsch", Value = "de" },
        new() { Name = "Italiano", Value = "it" },
        new() { Name = "中文", Value = "zh" },
        new() { Name = "日本語", Value = "ja" }
    };

    [Parameter]
    public List<SelectOption> Timezones { get; set; } = new()
    {
        new() { Name = "Pacific/Honolulu (GMT-10:00)", Value = "Pacific/Honolulu" },
        new() { Name = "America/Los_Angeles (GMT-07:00)", Value = "America/Los_Angeles" },
        new() { Name = "America/Denver (GMT-06:00)", Value = "America/Denver" },
        new() { Name = "America/Chicago (GMT-05:00)", Value = "America/Chicago" },
        new() { Name = "America/New_York (GMT-04:00)", Value = "America/New_York" },
        new() { Name = "Europe/London (GMT+01:00)", Value = "Europe/London" },
        new() { Name = "Asia/Tokyo (GMT+09:00)", Value = "Asia/Tokyo" }
    };

    [Parameter]
    public string SelectedLanguage { get; set; } = "en-US";

    [Parameter]
    public EventCallback<string> SelectedLanguageChanged { get; set; }

    [Parameter]
    public string SelectedTimezone { get; set; } = "America/Los_Angeles";

    [Parameter]
    public EventCallback<string> SelectedTimezoneChanged { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
```

**Data Model**: `src/WebApp/Domain/SelectOption.cs`
```csharp
namespace WebApp.Domain;

public class SelectOption
{
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
}
```

---

### Component 4: Sessions

#### Svelte Implementation
**File**: `src/lib/Sessions.svelte`
```svelte
<script lang="ts">
  import { Card, Heading, Button, A } from 'flowbite-svelte';
  import type { SessionProps, SessionType } from './types';

  let { seeMorehref = '#', sessions }: SessionProps = $props();
</script>

<Card size="xl" class="shadow-sm">
  <div class="mb-4 flex items-center justify-between">
    <Heading tag="h3" class="text-xl font-semibold">Sessions</Heading>
    <A href={seeMorehref} class="text-sm font-medium">See all</A>
  </div>
  <div class="space-y-4">
    {#each sessions as session}
      <div class="flex items-center justify-between border-b pb-4 last:border-0">
        <div class="flex items-center gap-4">
          {#if session.IconOption}
            <svelte:component this={session.IconOption.icon}
              size={session.IconOption.size ?? 'lg'}
              class={session.IconOption.iconClass} />
          {/if}
          <div>
            <div class="font-semibold text-gray-900 dark:text-white">
              {session.device}
            </div>
            <div class="text-sm text-gray-500 dark:text-gray-400">
              {session.ipaddress}
            </div>
          </div>
        </div>
        <Button href={session.href} color="light" size="xs">
          {session.btnName ?? 'Revoke'}
        </Button>
      </div>
    {/each}
  </div>
</Card>
```

#### Blazor Implementation
**File**: `src/WebApp/Components/Settings/Sessions.razor`
```razor
@using Flowbite.Components
@using Flowbite.Icons
@using WebApp.Domain

<Card Size="CardSize.ExtraLarge" class="shadow-sm">
    <div class="mb-4 flex items-center justify-between">
        <Heading Tag="HeadingTag.H3" class="text-xl font-semibold">Sessions</Heading>
        <a href="@SeeMoreHref" class="text-sm font-medium text-primary-600 hover:underline dark:text-primary-500">
            See all
        </a>
    </div>
    <div class="space-y-4">
        @foreach (var session in SessionList)
        {
            <div class="flex items-center justify-between border-b pb-4 last:border-0">
                <div class="flex items-center gap-4">
                    @if (session.Icon != null)
                    {
                        <DynamicComponent Type="@session.Icon"
                                        Parameters="@(new Dictionary<string, object> {
                                            { "Size", session.IconSize ?? "lg" },
                                            { "Class", session.IconClass ?? "" }
                                        })" />
                    }
                    <div>
                        <div class="font-semibold text-gray-900 dark:text-white">
                            @session.Device
                        </div>
                        <div class="text-sm text-gray-500 dark:text-gray-400">
                            @session.IpAddress
                        </div>
                    </div>
                </div>
                <Button Href="@session.ActionHref"
                       Color="ButtonColor.Light"
                       Size="ButtonSize.ExtraSmall">
                    @session.ActionButtonText
                </Button>
            </div>
        }
    </div>
</Card>

@code {
    [Parameter]
    public List<SessionInfo> SessionList { get; set; } = new();

    [Parameter]
    public string SeeMoreHref { get; set; } = "#";
}
```

**Data Model**: `src/WebApp/Domain/SessionInfo.cs`
```csharp
namespace WebApp.Domain;

public class SessionInfo
{
    public string Device { get; set; } = "";
    public string IpAddress { get; set; } = "";
    public string ActionHref { get; set; } = "#";
    public string ActionButtonText { get; set; } = "Revoke";
    public Type? Icon { get; set; }
    public string? IconSize { get; set; }
    public string? IconClass { get; set; }
}
```

---

### Component 5: NotificationCard

#### Blazor Implementation
**File**: `src/WebApp/Components/Settings/NotificationCard.razor`
```razor
@using Flowbite.Components
@using WebApp.Domain

<Card Size="CardSize.ExtraLarge" class="shadow-sm @Class">
    @if (!string.IsNullOrEmpty(Title))
    {
        <div class="mb-4">
            <Heading Tag="HeadingTag.H3" class="text-xl font-semibold">@Title</Heading>
            @if (!string.IsNullOrEmpty(Subtitle))
            {
                <p class="text-sm text-gray-500 dark:text-gray-300">@Subtitle</p>
            }
        </div>
    }
    <div class="space-y-4">
        @foreach (var item in Items)
        {
            <div class="flex items-center justify-between">
                <div>
                    <div class="font-medium text-gray-900 dark:text-white">
                        @item.Title
                    </div>
                    <div class="text-sm text-gray-500 dark:text-gray-400">
                        @item.Subtitle
                    </div>
                </div>
                <ToggleSwitch @bind-Checked="item.Active"
                             OnChange="@(() => HandleToggleChange(item))" />
            </div>
        }
    </div>
</Card>

@code {
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? Subtitle { get; set; }

    [Parameter]
    public List<NotificationItem> Items { get; set; } = new();

    [Parameter]
    public string Class { get; set; } = "";

    [Parameter]
    public EventCallback<NotificationItem> OnToggleChanged { get; set; }

    private async Task HandleToggleChange(NotificationItem item)
    {
        await OnToggleChanged.InvokeAsync(item);
    }
}
```

**Data Model**: `src/WebApp/Domain/NotificationItem.cs`
```csharp
namespace WebApp.Domain;

public class NotificationItem
{
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";
    public bool Active { get; set; }
}
```

---

## Playground Page Components

### Playground Component

#### Blazor Implementation
**File**: `src/WebApp/Components/Playground/Playground.razor`
```razor
@using Flowbite.Components
@using WebApp.Components.Shared

<main>
    <div class="grid grid-cols-1 pt-2 xl:grid-cols-3 xl:gap-4 xl:px-0 dark:bg-gray-900">
        <div class="col-span-full mb-4 xl:mb-2">
            @if (Breadcrumb != null)
            {
                @Breadcrumb
            }
            <Heading Tag="HeadingTag.H1" class="text-xl font-semibold sm:text-2xl">
                @Title
            </Heading>
        </div>
        <div class="col-span-full xl:col-auto">
            <EmptyCard Size="CardSize.ExtraLarge" Class="mb-4 h-80 w-full space-y-6 p-4" />
            <EmptyCard Size="CardSize.ExtraLarge" Class="mb-4 h-80 w-full space-y-6 p-4" />
        </div>
        <div class="col-span-2">
            <EmptyCard Class="mb-4 h-80 max-w-none space-y-6 p-4" />
            <EmptyCard Class="mb-4 h-80 w-full max-w-none space-y-6 p-4" />
        </div>
    </div>
    <div class="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
        @for (int i = 0; i < 4; i++)
        {
            <EmptyCard Size="CardSize.ExtraLarge" Class="h-60 w-full space-y-6 sm:p-6" />
        }
    </div>
</main>

@code {
    [Parameter]
    public RenderFragment? Breadcrumb { get; set; }

    [Parameter]
    public string Title { get; set; } = "Create something awesome here";
}
```

---

## Data Models and Services

### SettingsService

**File**: `src/WebApp/Services/SettingsService.cs`
```csharp
using WebApp.Domain;

namespace WebApp.Services;

public class SettingsService
{
    private UserSettings _settings = new();
    private List<NotificationItem> _notifications = new();
    private List<SessionInfo> _sessions = new();

    public event Action? OnChange;

    public UserSettings Settings => _settings;
    public List<NotificationItem> Notifications => _notifications;
    public List<SessionInfo> Sessions => _sessions;

    public SettingsService()
    {
        InitializeDefaultData();
    }

    public void UpdateSettings(UserSettings settings)
    {
        _settings = settings;
        NotifyStateChanged();
    }

    public void UpdateNotification(NotificationItem item)
    {
        var existing = _notifications.FirstOrDefault(n => n.Title == item.Title);
        if (existing != null)
        {
            existing.Active = item.Active;
            NotifyStateChanged();
        }
    }

    public void RevokeSession(SessionInfo session)
    {
        _sessions.Remove(session);
        NotifyStateChanged();
    }

    private void InitializeDefaultData()
    {
        _settings = new UserSettings
        {
            FirstName = "Bonnie",
            LastName = "Green",
            Email = "bonnie.green@company.com",
            Phone = "+(12) 345 6789",
            Organization = "Themesberg LLC",
            Role = "React Developer",
            Department = "Development",
            Country = "United States",
            City = "San Francisco",
            ZipCode = "94103",
            Address = "123 Main St"
        };

        _notifications = new List<NotificationItem>
        {
            new() { Title = "Email notifications", Subtitle = "Get notified via email", Active = true },
            new() { Title = "Push notifications", Subtitle = "Get notified on your device", Active = false },
            new() { Title = "SMS notifications", Subtitle = "Get notified via SMS", Active = true },
            new() { Title = "In-app notifications", Subtitle = "Get notified in the app", Active = true }
        };

        _sessions = new List<SessionInfo>
        {
            new()
            {
                Device = "Windows 10 - Chrome",
                IpAddress = "192.168.1.1",
                Icon = typeof(WindowsIcon),
                ActionHref = "#"
            },
            new()
            {
                Device = "iPhone 12 - Safari",
                IpAddress = "192.168.1.2",
                Icon = typeof(AppleIcon),
                ActionHref = "#"
            }
        };
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
```

**Registration**: `src/WebApp/Program.cs`
```csharp
services.AddScoped<SettingsService>();
```

---

## Common Patterns Reference

### Pattern: Event Handling

#### Svelte
```svelte
<button onclick={() => handleClick('data')}>Click</button>

<script>
  function handleClick(data: string) {
    console.log(data);
  }
</script>
```

#### Blazor
```razor
<button @onclick="@(() => HandleClick("data"))">Click</button>

@code {
    private void HandleClick(string data)
    {
        Console.WriteLine(data);
    }
}
```

### Pattern: Conditional Rendering

#### Svelte
```svelte
{#if condition}
  <p>True</p>
{:else}
  <p>False</p>
{/if}
```

#### Blazor
```razor
@if (condition)
{
    <p>True</p>
}
else
{
    <p>False</p>
}
```

### Pattern: List Rendering

#### Svelte
```svelte
{#each items as item, index}
  <div>{index}: {item.name}</div>
{/each}
```

#### Blazor
```razor
@foreach (var (item, index) in items.Select((item, i) => (item, i)))
{
    <div>@index: @item.Name</div>
}
```

### Pattern: Two-Way Binding

#### Svelte
```svelte
<input bind:value={text} />
<p>{text}</p>
```

#### Blazor
```razor
<input @bind="Text" @bind:event="oninput" />
<p>@Text</p>

@code {
    private string Text { get; set; } = "";
}
```

### Pattern: Component Communication

#### Svelte (Event Dispatch)
```svelte
<!-- Child -->
<script>
  import { createEventDispatcher } from 'svelte';
  const dispatch = createEventDispatcher();

  function notify() {
    dispatch('message', { text: 'Hello' });
  }
</script>

<!-- Parent -->
<Child on:message={(e) => console.log(e.detail.text)} />
```

#### Blazor (EventCallback)
```razor
@* Child *@
<button @onclick="Notify">Notify</button>

@code {
    [Parameter]
    public EventCallback<string> OnMessage { get; set; }

    private Task Notify()
    {
        return OnMessage.InvokeAsync("Hello");
    }
}

@* Parent *@
<Child OnMessage="@HandleMessage" />

@code {
    private void HandleMessage(string text)
    {
        Console.WriteLine(text);
    }
}
```

---

## Summary

This guide provides concrete implementation patterns for migrating Svelte components to Blazor. Key takeaways:

1. **Snippets → RenderFragments**: Named content slots translate directly
2. **$props() → [Parameter]**: Props become parameters with attributes
3. **$state → private fields**: Local state managed manually
4. **Services for global state**: Replace Svelte stores with DI services
5. **EditForm for validation**: Blazor has built-in form validation
6. **EventCallbacks for communication**: Replace event dispatching

The next document will provide a detailed implementation plan with task breakdown and sequencing.
