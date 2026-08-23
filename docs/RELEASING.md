# Releasing

Every published cut of Personality Engine has a **version**, **changelog notes**, and a **GitHub Release** with downloadable assets.

## Version

The package and assembly version live in `src/PersonalityEngine.Core/PersonalityEngine.Core.csproj` (`Version`). Use SemVer: `MAJOR.MINOR.PATCH`.

- **PATCH**: fixes, docs, packaging
- **MINOR**: new providers, layers, or compatible API
- **MAJOR**: breaking snapshot keys, event kinds, or composition contracts

The README “Current library version” line must match `Version`.

## Release notes

Add a new section at the top of `CHANGELOG.md` (below `[Unreleased]`):

```markdown
## [X.Y.Z] - YYYY-MM-DD

### Added
### Changed
### Fixed
```

Move items out of `[Unreleased]`. Update the compare links at the bottom of that file. Those notes are the GitHub Release body. Do **not** include private issue-tracker URLs, project keys, or ticket ids in the changelog, the GitHub Release, or the git tag message.

## Cut a release

1. Bump `Version` and the README version line.
2. Write the `CHANGELOG.md` section.
3. Commit on `main` (why, not what).
4. Tag and push:

   ```bash
   git tag -a vX.Y.Z -m "Personality Engine vX.Y.Z"
   git push origin main
   git push origin vX.Y.Z
   ```

5. Pushing `v*` runs [`.github/workflows/release.yml`](../.github/workflows/release.yml): test, pack `.nupkg` + DLL zip, create the GitHub Release with the changelog section as notes, and push `PersonalityEngine.Core` to [GitHub Packages](https://github.com/RossSim/personality-engine/packages) (`nuget.pkg.github.com`). Pull requests and `main` already run `dotnet test` and the console samples; the tag run is the last gate.

If the workflow cannot run, pack locally and attach the same assets:

```bash
dotnet test
dotnet pack src/PersonalityEngine.Core/PersonalityEngine.Core.csproj -c Release -o dist
dotnet build src/PersonalityEngine.Core/PersonalityEngine.Core.csproj -c Release
```

Do not commit `dist/`, `bin/`, or `*.nupkg`.
