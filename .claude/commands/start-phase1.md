# Phase 1: Layout Refinements

**IMPORTANT**: You have zero memory of this project. Start by reading these files in order:

1. **MIGRATION_LOG.md** - Your memory from previous sessions (ALWAYS READ THIS FIRST)
2. **docs/IMPLEMENTATION_PLAN.md** (Phase 1 section only - focus on tasks 1.1 through 1.5)
3. **docs/ARCHITECTURE_COMPARISON.md** (Layout System section)
4. **CLAUDE.md** (Project overview and architecture)
5. Current layout files:
   - src/WebApp/Layout/MainLayout.razor
   - src/WebApp/Layout/AppNavBar.razor
   - src/WebApp/Layout/AppSidebar.razor
   - src/WebApp/Layout/AppFooter.razor

## After Reading

1. Create a TodoWrite list with all Phase 1 tasks (5 tasks from Implementation Plan):
   - Task 1.1: Create LayoutBase.razor
   - Task 1.2: Refactor MainLayout.razor
   - Task 1.3: Create StackedLayout.razor
   - Task 1.4: Dark Mode Consistency
   - Task 1.5: Sidebar Responsive Behavior

2. Check MIGRATION_LOG.md to see current progress on Phase 1

3. Report what's completed and what's next

4. Ask which task to start with (or suggest starting with Task 1.1 if nothing is complete)

## Sub-Agent Guidance (Use Autonomously)

**Use Explore agent** (thoroughness: "medium") when:
- You need to find Svelte layout patterns in: /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(sidebar)/+layout.svelte
- You need to understand how Svelte handles responsive sidebar behavior across multiple files
- You need to see how dark mode is implemented consistently in Svelte layouts

**Use general-purpose agent** when:
- Complex research like "how does Svelte handle layout inheritance patterns across the app"
- Need to analyze multiple layout files and synthesize findings

**Do NOT use sub-agents** for:
- Direct component implementation (user wants to see the work)
- Simple file reads of known paths
- Direct edits

## After Completing Each Task

**YOU MUST UPDATE MIGRATION_LOG.md** with:

1. **Task status**: Change from ⬜ Not started → 🚧 In Progress → ✅ Complete
2. **Completion date**: Add the date when completed
3. **Files created/modified**: List all files touched
4. **Code patterns used**: Document any patterns (e.g., "LayoutComponentBase inheritance", "StateHasChanged() usage")
5. **Issues encountered**: Any problems and how they were solved
6. **Learnings**: What worked well, what to remember for future tasks
7. **Update Quick Status**: Increment Phase 1 progress counter

## Important Reminders

- Read MIGRATION_LOG.md FIRST before doing anything
- Update MIGRATION_LOG.md IMMEDIATELY after completing each task
- Mark tasks in TodoWrite as completed as you finish them
- Layout files use Tailwind CSS v3.4.15 (not v4 like Svelte)
- Ensure dark: classes are present for dark mode support
- Responsive breakpoints: sm:, md:, lg:, xl:, 2xl:
- Sidebar should be hidden on mobile (<768px), visible on desktop (≥768px)
