# Cursor start notes

Copy into a new Cursor chat if the session has no repo memory.

```
START
Repo: https://github.com/RossSim/personality-engine (public MIT)
This is a standalone C# library (netstandard2.1), not a Unity project. This repository is self-contained; do not name other private or internal projects.

Read docs/CHARTER.md first. For how games should *use* the engine at design time (not the C# API), read docs/APPLICATIONS.md. The engine is modular middleware: composable, citable providers. OCEAN, PAD, and OCC are the default composition (Gebhard ALMA 2005 wiring), not the ceiling. Hosts may add layers, replace a layer’s provider, or supplement a layer with more sources/methods. No LLM in the core. Current published library is 0.5.1 (fortune-of-others OCC, optional dyad liking, social Utility-AI tint).

GitHub is public. Do not put private issue-tracker URLs, project keys, or ticket ids in this repository, GitHub pull requests, issues, commit messages, Releases, or release notes. Product docs live in this repository. Use the Atlassian MCP for Jira only; keep that tracker private.
END
```

Standing rules:

- GitHub is public; the issue tracker is private. Never commit or publish Jira/Atlassian links or ticket ids — including PR titles, PR bodies, issues, commit messages, and GitHub Releases.
- Product docs live in this repository. The private tracker is Jira only. Do not search, create, or update wiki pages.
- This repository is self-contained. Do not name other private or internal projects, studios, or repos.
- Every provider cites a paper; project numbers are labeled project convention.
- Versions live in the Core csproj + CHANGELOG.md; cut a GitHub Release (`vX.Y.Z` tag) for each public cut. See docs/RELEASING.md.
