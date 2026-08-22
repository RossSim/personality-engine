#!/usr/bin/env bash
# Fail if this public repo contains private issue-tracker URLs or ticket ids.
set -euo pipefail

root="$(git rev-parse --show-toplevel 2>/dev/null || true)"
if [[ -n "${root}" ]]; then
  cd "${root}"
fi

pattern='atlassian\.net|atlassian\.com|jira\.com|/browse/[A-Z]{2,}-[0-9]+|\b(AR|PE|AV)-[0-9]+\b'

matches="$(git grep -I -n -E "${pattern}" -- . ':!.github/workflows/public-hygiene.yml' ':!scripts/check-public-hygiene.sh' || true)"

if [[ -n "${matches}" ]]; then
  echo "Private issue-tracker URLs or ticket ids must not appear in this public repository:"
  echo "${matches}"
  exit 1
fi

echo "Public hygiene: no private issue-tracker URLs or ticket ids in the tree."
