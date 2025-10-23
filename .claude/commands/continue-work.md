# Continue Migration Work

**IMPORTANT**: You have zero memory of this project. Start by reading:

1. **MIGRATION_LOG.md** - Your memory from previous sessions (ALWAYS READ THIS FIRST)
2. **docs/IMPLEMENTATION_PLAN.md** - Full implementation plan with all 23 tasks

## After Reading

1. Determine current phase based on MIGRATION_LOG.md:
   - Check Phase 1 progress (0-5 tasks complete)
   - Check Phase 2 progress (0-13 tasks complete)
   - Check Phase 3 progress (0-5 tasks complete)

2. Recreate TodoWrite list showing ALL 23 tasks with current status:
   - Mark completed tasks with ✅
   - Mark in-progress task with 🚧 (if any)
   - Mark pending tasks with ⬜

3. Report comprehensive status:
   - Overall progress: X/23 tasks complete (X%)
   - What phase you're currently in
   - What's completed in current phase
   - What task is in progress (if any)
   - What's the next task to do
   - Any blockers noted in MIGRATION_LOG.md

4. Ask user if you should:
   - Continue with the next pending task, OR
   - Switch to a different task, OR
   - Review a specific completed component, OR
   - Skip to a different phase

## Sub-Agent Guidance (Use Autonomously)

Check MIGRATION_LOG.md for patterns on when sub-agents were useful in previous tasks.

**Use Explore agent** when:
- You need to find Svelte reference code for the current task
- Understanding implementation patterns across multiple files
- The current task involves researching component composition

**Use general-purpose agent** when:
- Complex multi-step research is needed
- Analyzing patterns across multiple completed and pending tasks
- Synthesizing learnings from previous work

**Do NOT use sub-agents** for:
- Direct component implementation
- Simple file reads
- Direct edits
- Status reporting

## After Completing Any Task

**YOU MUST UPDATE MIGRATION_LOG.md** with:

1. **Task status**: Update from ⬜ → 🚧 → ✅
2. **Completion date**: Add date
3. **Files created/modified**: Full paths
4. **Code patterns used**: Document what patterns you applied
5. **Issues encountered**: Problems and solutions
6. **Learnings**: What worked well, tips for future tasks
7. **Update Quick Status**: Increment appropriate phase counter
8. **Update Overall Progress**: Recalculate X/23 and percentage

## Important Reminders

- Read MIGRATION_LOG.md FIRST before doing anything else
- Update MIGRATION_LOG.md IMMEDIATELY after completing each task
- Mark tasks in TodoWrite as you complete them
- Check for blockers in MIGRATION_LOG.md (some tasks depend on others)
- Phase 3 Tasks 3.4-3.5 depend on Phase 1 Task 1.3 (StackedLayout.razor)
- All Phase 2 components depend on Tasks 2.1-2.3 (models + service)
- Refer to docs/SVELTE_TO_BLAZOR_GUIDE.md for complete code examples
- Refer to docs/ARCHITECTURE_COMPARISON.md for framework differences
- Reference Svelte source files for behavior and styling details

## Context Recommendations

Based on current phase, you should also read:

**If in Phase 1 (Layout)**:
- docs/ARCHITECTURE_COMPARISON.md (Layout section)
- Current layout files in src/WebApp/Layout/

**If in Phase 2 (Settings)**:
- docs/SVELTE_TO_BLAZOR_GUIDE.md (Settings Components section - has full code)
- Svelte Settings page: /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(sidebar)/settings/+page.svelte

**If in Phase 3 (Playground)**:
- docs/SVELTE_TO_BLAZOR_GUIDE.md (Playground section)
- Svelte Playground pages: /mnt/c/Users/tschavey/projects/themesberg/flowbite-svelte-admin-dashboard/src/routes/(no-sidebar)/playground/stacked/+page.svelte

Tell the user which files you're reading for context based on current phase.
