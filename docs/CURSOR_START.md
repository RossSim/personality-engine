# Cursor start notes

Copy into a new Cursor chat if the session has no repo memory.

```
START
Repo: https://github.com/textide/personality-engine (public MIT)
Jira: https://prayingforradar.atlassian.net/jira/software/projects/PE/summary
Project key: PE. Charter: PE-1. Coding epic: PE-3.
This is a standalone C# library (netstandard2.1), not Unity and not AviationStuff Assets/Systems.

Read docs/CHARTER.md first. The engine is modular middleware: composable, citable providers. OCEAN, PAD, and OCC are the default v0.1 composition (Gebhard ALMA 2005 wiring), not the ceiling. Hosts may add layers, replace a layer’s provider, or supplement a layer with more sources/methods. No LLM in the core.

Tickets: PE-2 docs, PE-4 citation map, PE-5 repo skeleton + first Gebhard OCEAN→PAD test.
END
```

Standing rules:

- Seal Done epics; do not parent new work onto them.
- Every provider cites a paper; project numbers are labeled project convention.
- Prefer small stories under PE-3 over growing PE-1.
