
Recommended IDE actions
- In Visual Studio open the solution and use __Build Solution__ or __Rebuild__.
- Use an EditorConfig file (repo may include one) to enforce formatting rules.

Formatting and linting
- Run `dotnet format` to apply consistent formatting.
- Prefer small, readable changes and run tests before submitting a PR.

## API and documentation
- Update XML docs and `src/UlidType/README.md` for any public API changes.
- Add or update examples in `examples/` as needed.

## Tests
- Add unit tests for bug fixes and new features under `test/UlidType.Tests`.
- Tests should be deterministic where possible and runnable with `dotnet test`.

## Pull request checklist
- [ ] PR targets `main`
- [ ] Branch name follows convention
- [ ] Unit tests added/updated and passing
- [ ] Documentation updated if behavior or public API changed
- [ ] CI checks pass (build & tests)
- [ ] Include description of the change and motivation

## Continuous Integration / Releases
- The repository uses GitHub Actions (or similar) for CI. CI should build the project and run tests on PRs.
- Releases should follow semantic versioning where possible.

## Licensing & Intellectual Property
- Verify that your contribution can be licensed under the repository license.
- Do not include third-party code without proper license and attribution.

## Questions
If you are unsure about design choices or need guidance, open an issue and label it `discussion` or `help wanted`.

Thank you for contributing!