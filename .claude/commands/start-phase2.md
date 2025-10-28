# Phase 2: Settings Page Implementation

**IMPORTANT**: You have zero memory of this project. Start by reading these files in order:

1. **MIGRATION_LOG.md** - Your memory from previous sessions (ALWAYS READ THIS FIRST)
2. **docs/IMPLEMENTATION_PLAN.md** (Phase 2 section only - tasks 2.1 through 2.13)
3. **docs/SVELTE_TO_BLAZOR_GUIDE.md** (Settings Components section - this has complete code examples)
4. **docs/ARCHITECTURE_COMPARISON.md** (State Management and Component Composition sections)
5. **CLAUDE.md** (Project overview)
6. Svelte Settings reference:
   - /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(sidebar)/settings/+page.svelte
   - /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/lib/types.ts (for type definitions)

## After Reading

1. Create a TodoWrite list with all Phase 2 tasks (13 tasks from Implementation Plan):
   - Task 2.1: Create Domain Models
   - Task 2.2: Create SettingsService.cs
   - Task 2.3: Register SettingsService
   - Task 2.4: Create UserProfile.razor
   - Task 2.5: Create GeneralInfo.razor
   - Task 2.6: Create PasswordInfo.razor
   - Task 2.7: Create LanguageTime.razor
   - Task 2.8: Create SocialAccounts.razor
   - Task 2.9: Create Accounts.razor
   - Task 2.10: Create Sessions.razor
   - Task 2.11: Create NotificationCard.razor
   - Task 2.12: Create Settings.razor Page
   - Task 2.13: Add Settings Navigation Link

2. Check MIGRATION_LOG.md for current progress on Phase 2

3. Report what's completed and what's next

4. List all 8 Settings components that need migration

5. Ask which task/component to start with (or suggest starting with Task 2.1 if nothing is complete)

## Sub-Agent Guidance (Use Autonomously)

**Use Explore agent** (thoroughness: "medium") when:
- Finding all Svelte Settings components: /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/lib/
- Understanding how Settings components are used in the Svelte Settings page
- Searching for form validation patterns across Svelte components
- Finding all uses of Snippet pattern in Settings components

**Use general-purpose agent** when:
- Complex research like "how does Svelte handle form validation across all Settings components"
- Multi-step analysis of state management flow in Svelte Settings
- Synthesizing patterns from multiple Settings component implementations

**Do NOT use sub-agents** for:
- Direct component implementation (user wants to see the work)
- Simple file reads of known component paths
- Direct edits to create components
- Single-file analysis

## After Completing Each Task

**YOU MUST UPDATE MIGRATION_LOG.md** with:

1. **Task status**: Change from ⬜ Not started → 🚧 In Progress → ✅ Complete
2. **Completion date**: Add the date when completed
3. **Files created/modified**: List all files with full paths
4. **For components, document**:
   - Parameters defined (with [Parameter] attribute)
   - RenderFragments used (for child content/slots)
   - EventCallbacks defined (for events)
   - Service injections (@inject directives)
   - Form validation patterns (if applicable)
5. **Code patterns used**: Document patterns (e.g., "EditForm + DataAnnotationsValidator", "Service event subscription in OnInitialized")
6. **Issues encountered**: Any problems and solutions
7. **Learnings**: What worked well, tips for similar components
8. **Update Quick Status**: Increment Phase 2 progress counter
9. **Add to Pattern Library section**: If you discover a reusable pattern, add it to the Pattern Library in MIGRATION_LOG.md

## Important Reminders

- Read MIGRATION_LOG.md FIRST before doing anything
- Update MIGRATION_LOG.md IMMEDIATELY after completing each task
- Mark tasks in TodoWrite as completed as you finish them
- The SVELTE_TO_BLAZOR_GUIDE.md has COMPLETE working code examples - reference these!
- Settings components go in: src/WebApp/Components/Settings/
- Domain models go in: src/WebApp/Domain/
- Services go in: src/WebApp/Services/
- Register services in src/WebApp/Program.cs within the ConfigureServices() function
- Use EditForm with DataAnnotationsValidator for forms
- Use EventCallback<T> for component events
- Subscribe to service events in OnInitialized(), unsubscribe in Dispose()
- Always call StateHasChanged() when responding to service events

## Component Implementation Order (Recommended)

If starting fresh, follow this order (dependencies listed):

1. **Task 2.1-2.3**: Domain models + SettingsService (foundation)
2. **Task 2.4**: UserProfile.razor (simple, no forms)
3. **Task 2.7**: LanguageTime.razor (dropdowns, no validation)
4. **Task 2.5**: GeneralInfo.razor (form with validation - most complex)
5. **Task 2.6**: PasswordInfo.razor (form with validation)
6. **Task 2.8**: SocialAccounts.razor (list rendering)
7. **Task 2.9**: Accounts.razor (toggles)
8. **Task 2.10**: Sessions.razor (list with actions)
9. **Task 2.11**: NotificationCard.razor (toggles)
10. **Task 2.12**: Settings.razor page (assembly)
11. **Task 2.13**: Add navigation link

This order builds from simple to complex, allowing you to learn patterns incrementally.
