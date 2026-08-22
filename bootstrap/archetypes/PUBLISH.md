# Publish to RossSim/archetypes

This folder is a **bootstrap copy** of the Archetypes skeleton. The live repo is https://github.com/RossSim/archetypes.

## Desktop Cursor (GitHub-authorized)

From a parent directory (not inside personality-engine):

```bash
git clone https://github.com/RossSim/archetypes.git
cd archetypes
```

Copy everything from `personality-engine/bootstrap/archetypes/` into this repo root (merge with existing `LICENSE` and `README.md` — the bootstrap versions replace the live copies). Omit `PUBLISH.md` from the live repo; it is only for this folder.

```bash
# Example if personality-engine is cloned beside archetypes:
cp -a ../personality-engine/bootstrap/archetypes/. .
rm -f PUBLISH.md
git add -A
git status
git commit -m "Add catalog-first charter, Cursor start notes, and public hygiene"
git push origin main
```

Or open `archetypes` in Cursor and ask the agent to sync from `personality-engine/bootstrap/archetypes/`.

## Cloud agent

Grant the Cursor GitHub App **write** access to `RossSim/archetypes` (same as personality-engine).
