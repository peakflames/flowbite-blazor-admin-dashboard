# Phase 3: Playground Pages Implementation

**IMPORTANT**: You have zero memory of this project. Start by reading these files in order:

1. **MIGRATION_LOG.md** - Your memory from previous sessions (ALWAYS READ THIS FIRST)
2. **docs/IMPLEMENTATION_PLAN.md** (Phase 3 section only - tasks 3.1 through 3.5)
3. **docs/SVELTE_TO_BLAZOR_GUIDE.md** (Playground Components section)
4. **docs/ARCHITECTURE_COMPARISON.md** (Layout System section)
5. **CLAUDE.md** (Project overview)
6. Svelte Playground reference:
   - /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(no-sidebar)/playground/stacked/+page.svelte
   - /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(sidebar)/playground/sidebar/+page.svelte
   - /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/lib/EmptyCard.svelte
   - /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/lib/Playground.svelte

## After Reading

1. Create a TodoWrite list with all Phase 3 tasks (5 tasks from Implementation Plan):
   - Task 3.1: Create EmptyCard.razor
   - Task 3.2: Create Playground.razor Component
   - Task 3.3: Create PlaygroundSidebar.razor Page
   - Task 3.4: Create PlaygroundStacked.razor Page
   - Task 3.5: Add Playground Navigation Links

2. Check MIGRATION_LOG.md for current progress on Phase 3

3. Report what's completed and what's next

4. Ask which task to start with (or suggest starting with Task 3.1 if nothing is complete)

## Prerequisites

**IMPORTANT**: Phase 3 depends on Phase 1 completion!

- Task 3.4 (PlaygroundStacked.razor) requires StackedLayout.razor from Task 1.3
- If StackedLayout.razor doesn't exist yet, you must either:
  1. Complete Task 1.3 first, OR
  2. Create a placeholder StackedLayout.razor for Task 3.4

Check MIGRATION_LOG.md to see if Task 1.3 is complete. Report this to the user.

## Sub-Agent Guidance (Use Autonomously)

**Use Explore agent** (thoroughness: "quick") when:
- Finding Svelte Playground component patterns
- Understanding how Svelte structures the 6x6 grid
- Looking for how EmptyCard is styled in Svelte

**Use general-purpose agent** when:
- Need to understand layout differences between sidebar and stacked Playground pages
- Analyzing how Svelte handles the Playground grid across different layout contexts

**Do NOT use sub-agents** for:
- Direct component implementation (user wants to see the work)
- Simple file reads of Playground components
- Direct edits

## After Completing Each Task

**YOU MUST UPDATE MIGRATION_LOG.md** with:

1. **Task status**: Change from ⬜ Not started → 🚧 In Progress → ✅ Complete
2. **Completion date**: Add the date when completed
3. **Files created/modified**: List all files with full paths
4. **For components, document**:
   - Parameters defined
   - RenderFragments used
   - Grid/layout patterns (e.g., "grid-cols-2 md:grid-cols-3 gap-3.5")
   - Loop patterns (@for, @foreach)
5. **Code patterns used**: Document patterns used
6. **Issues encountered**: Any problems and solutions
7. **Learnings**: What worked well
8. **Update Quick Status**: Increment Phase 3 progress counter

## Important Reminders

- Read MIGRATION_LOG.md FIRST before doing anything
- Update MIGRATION_LOG.md IMMEDIATELY after completing each task
- Mark tasks in TodoWrite as completed as you finish them
- Playground components are simpler than Settings - mostly layout/styling
- EmptyCard is a simple presentational component
- Playground.razor is a 6x6 grid of EmptyCards (36 total cards)
- PlaygroundSidebar uses MainLayout (default)
- PlaygroundStacked uses StackedLayout (requires @layout directive)
- Grid pattern: responsive grid with gap-3.5 spacing
- Components go in: src/WebApp/Components/
- Pages go in: src/WebApp/Pages/

## Component Implementation Order (Recommended)

1. **Task 3.1**: EmptyCard.razor (foundation component)
2. **Task 3.2**: Playground.razor (uses EmptyCard)
3. **Task 3.3**: PlaygroundSidebar.razor (uses Playground, easier first)
4. **Task 3.4**: PlaygroundStacked.razor (requires StackedLayout from Phase 1)
5. **Task 3.5**: Add navigation links (final step)
