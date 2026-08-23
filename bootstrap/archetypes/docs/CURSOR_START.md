# Cursor start notes

Copy into a new Cursor chat if the session has no repo memory.

```
START
Repo: https://github.com/RossSim/archetypes (public MIT)
Companion to https://github.com/RossSim/personality-engine. This is authoring data (preset catalogs and, later, a builder), not a Unity project and not a psychology-provider library.

Read docs/CHARTER.md first. Then docs/DESIGN.md and docs/ROADMAP.md. Catalogs come before MindPreset and PresetBuilder. No new IAffectProvider here. No IQ or g. No real-world race, ethnicity, or national rank presets. No LLM in this repo.

GitHub is public. Do not put private issue-tracker URLs, project keys, or ticket ids in this repository, GitHub pull requests, issues, commit messages, Releases, or release notes. Product docs live in this repository. Use the Atlassian MCP for Jira only; keep that tracker private.
END
```

Standing rules:

- GitHub is public; the issue tracker is private. Never commit or publish tracker URLs, project keys, or ticket ids — including PR titles, PR bodies, issues, commit messages, and GitHub Releases.
- Product docs live in this repository. The private tracker is Jira only. Do not search, create, or update wiki pages.
- This repository is self-contained. Do not name other private or internal projects, studios, or repos.
- Every preset knob cites a paper or is labeled project convention.
- No new `IAffectProvider` implementations here. Those stay in personality-engine.
