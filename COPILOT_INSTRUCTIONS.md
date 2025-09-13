# Project Guidance for Copilot & Contributors

## Style Sources (Do Not Duplicate Here)
Refer to:
- .editorconfig (authoritative code style + analyzers)
- Directory.Packages.props (centralized package versions)
- Directory.Build.props / .targets (shared build config)
- Each project’s *.csproj
- Per project: usings.cs (global usings)

Keep this file focused on *intent* and *preferences* so Copilot infers patterns.

## General Coding Conventions
- Use file-scoped namespaces.
- Prefer `readonly record struct` for small immutable value objects.
- Prefer `internal` over `public` unless part of an intentional API surface.
- Use expression-bodied members when trivial and not harming readability.
- Use `sealed` by default for classes unless extensibility is required.
- Prefer `var` when the type is obvious from the right-hand side; otherwise be explicit.
- Always honor nullable reference types (treat warnings as design feedback).
- Avoid static mutable state.
- Use dependency injection over service locator.
- Prefer guard clauses at method start (throw early, no nested pyramid).
- Prefer pattern matching (`is`, `switch expressions`) over `if`/`else` chains when semantic.
- Do not use curly braces for single-line blocks unless improving readability.
- It's OK to use #region / #endregion for logical grouping in larger files.

## Domain / DDD
- Aggregates enforce invariants; do not leak internal collections—expose read-only views.
- Use value objects where identity = value equality.
- Keep repositories aggregate-root–centric.
- One transaction = one aggregate unless explicitly extending via AllowedAggregateRoots (rare; justify in PR).
- Invariants validated via `IValidatable`; do not duplicate validation in services unless policy-specific.

## EF Core
- Avoid lazy loading (explicit or eager only).
- Keep DbContext lifetime scoped; no static context.
- No raw SQL unless unavoidable—prefer LINQ and query filters.
- Configure concurrency tokens where appropriate (RowVersion or concurrency columns).
- Interceptors (like DddInterceptor) should remain side‑effect minimal and deterministic.
- Do not call `.Result` / `.Wait()` on async database calls.

## Async
- Suffix async methods with `Async`.
- Pass `CancellationToken` through all async call chains.
- Avoid fire-and-forget except for explicitly scheduled background operations (document rationale).
- Use `ValueTask` only when it measurably reduces allocations (e.g. hot paths already returning cached results).

## Error Handling / Logging
- Throw domain-specific exceptions for business rule violations; not generic `InvalidOperationException` unless internal invariant.
- Avoid swallowing exceptions – log or rethrow.
- Do not log sensitive data (PII, secrets).
- Prefer `Try...` patterns over broad exception-based control flow where appropriate.

## Testing Conventions
- Framework: xUnit.
- Assertions: FluentAssertions (never Assert.* unless framework-specific).
- Mocks/Doubles: NSubstitute.
- Test naming:
  - Async: `MethodName_WhenCondition_ShouldOutcome_Async`
  - Sync: `MethodName_WhenCondition_ShouldOutcome`
- Use Arrange / Act / Assert with clear spacing.
- Minimize deep object graphs—use builders or inline records.
- Prefer one logical assertion per test (grouped FluentAssertions chain counts as one).
- Avoid testing implementation details (interactions only when behavior requires).
- Use `Trait` attributes (e.g. `[Trait("Category","Integration")]`) for slower or external tests.
- Deterministic time: inject clock abstractions (never rely on `DateTime.UtcNow` directly in tests).
- Deterministic IDs: inject or override ID providers when needed.

## Test Data Patterns
- Use a Test Data Builder (`EntityBuilder`) instead of repetitive object scaffolding.
- For complex aggregates, expose fluent methods (e.g. `.WithStatus(...)`, `.WithLineItems(params ...)`).
- Avoid shared mutable fixtures; prefer inline creation or per-test fixture classes.

## Mocking Guidelines
- Only mock external collaborators (I/O, time, random, repository, bus).
- Do not mock value objects.
- Default to strictness by verifying only meaningful interactions.

## Performance / Allocation
- Avoid premature optimization; but:
  - Use `AsNoTracking()` for read-only queries.
  - Avoid unnecessary `ToList()` materialization inside query pipelines.
  - Avoid large object graphs serialization unless required.

## Naming
- Events: past tense (e.g. `OrderPlacedEvent`).
- Commands: imperative (e.g. `PlaceOrderCommand`).
- Handlers: suffix with `Handler`.
- Repositories: `<AggregateRoot>Repository`.

## Git / PR Hygiene
- One logical concern per PR.
- PR description: What / Why / How / Risk / Rollback.
- Commit messages: `<scope>: <imperative summary>`
  - Example: `ddd: enforce single tenant boundary in interceptor`

## Security
- Never embed secrets—use user secrets or environment variables.
- Validate all external inputs at boundaries (controllers, message consumers).

## Copilot Prompting Hints
When asking Copilot for code:
- Specify: context (aggregate, EF entity, test scenario).
- Specify: required patterns (e.g. “value object with equality & validation”).
- For tests: mention desired doubles, e.g. “use NSubstitute for IClock”.

## Anti-Patterns (Avoid)
- Service classes that just forward to repository.
- Anemic domain models (add invariants to entities).
- Static utility god classes.
- Catch-all exception wrappers that rethrow without context.
- Copy/paste mapping logic (centralize with mappers or projection expressions).

## Interceptor-Specific Guidance
- DddInterceptor changes should include tests covering: audit, invariants, boundary violations, allowed extensions, soft delete semantics.
- Keep `ActionParameters` minimal—avoid adding transient computed values if derivable at call site.
- Order: Tenant -> Aggregate -> Audit -> Complete -> Validate (do not reorder without justification).

## Tooling / Analyzer Suggestions
- Add analyzers for: async usage, nullability misuse, concurrency tokens.
- Treat warnings as build errors for new code paths where feasible.

## Documentation
- Public surface: XML docs for all externally-consumed APIs.
- limit to 128 characters per line.
- Internal code: only document non-obvious intent or domain rationale (avoid “what” duplication).
- Put the XML start and end element tags on their own lines, unless the internal text is short enough to fit the entire element
  on one line.

## Future Enhancements (Track Separately)
- Evaluate using `IClock` abstraction to remove direct time providers.
- Introduce domain events dispatch hook post-commit.
- Expand multi-tenant tests to include row-level security simulation.

---
(End of instructions – keep additions lean and purposeful; remove anything that drifts from active practice.)