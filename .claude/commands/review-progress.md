# Review Migration Progress

**IMPORTANT**: You have zero memory of this project. Start by reading:

1. **MIGRATION_LOG.md** - Your memory from previous sessions (ALWAYS READ THIS FIRST)
2. **docs/IMPLEMENTATION_PLAN.md** - Full implementation plan with all 23 tasks

## After Reading

Generate a comprehensive status report including:

### 1. Overall Progress Summary

```
Overall Progress: X/23 tasks complete (X%)
Estimated time remaining: X hours (based on Implementation Plan estimates)

Phase 1 (Layout): X/5 tasks complete (X%)
Phase 2 (Settings): X/13 tasks complete (X%)
Phase 3 (Playground): X/5 tasks complete (X%)
```

### 2. Detailed Phase Status

For each phase, list:

**Phase 1: Layout Refinements**
- ✅ Task 1.1: Create LayoutBase.razor (Completed: [date])
- 🚧 Task 1.2: Refactor MainLayout.razor (In Progress)
- ⬜ Task 1.3: Create StackedLayout.razor (Not started)
- etc.

**Phase 2: Settings Page**
- [Same format for all 13 tasks]

**Phase 3: Playground Pages**
- [Same format for all 5 tasks]

### 3. Recently Completed Work

List the last 3-5 completed tasks with:
- Task name
- Completion date
- Key learnings (from MIGRATION_LOG.md)

### 4. Current Status

- What task is currently in progress (if any)
- What's the next task to do
- Any blockers or dependencies

### 5. Blockers & Issues

From MIGRATION_LOG.md section "Blockers & Incomplete Work":
- List any blocked tasks
- List any incomplete work
- List any open questions

### 6. Key Patterns Discovered

From MIGRATION_LOG.md section "Pattern Library":
- List the patterns that have been established
- Note which components use which patterns
- Highlight any patterns that worked particularly well

### 7. Common Issues & Solutions

From MIGRATION_LOG.md section "Common Issues & Solutions":
- List problems encountered
- List solutions that worked
- Tips for avoiding similar issues

### 8. Testing Status

From MIGRATION_LOG.md section "Testing Notes":
- List components that have been tested
- List components that need testing
- Note any test failures or issues

### 9. Recommendations

Based on the progress, provide:
- What should be done next
- Any suggested changes to task order
- Areas that might need extra attention
- Estimated time to completion

## Important Notes

- **This is a READ-ONLY command** - Do NOT make any changes
- Do NOT update MIGRATION_LOG.md
- Do NOT use TodoWrite
- Do NOT create or edit any files
- Just report the status based on MIGRATION_LOG.md

## Sub-Agent Guidance

**Do NOT use sub-agents for this command** - This is a simple status report based on MIGRATION_LOG.md.

## Output Format

Present the report in a clear, formatted way using markdown:
- Use headings (##, ###)
- Use bullet lists for tasks
- Use emoji status indicators (✅ 🚧 ⬜)
- Use code blocks for any code snippets or file paths
- Use bold for emphasis on important points
- Use tables if helpful for comparing phases

## After Presenting Report

Ask the user:
1. Do you want to continue with the next task?
2. Do you want to dive deeper into any specific phase or component?
3. Do you want to address any blockers?
4. Do you want to review a specific completed component?
5. Any other action?

Do NOT automatically proceed with any work - wait for user direction.
