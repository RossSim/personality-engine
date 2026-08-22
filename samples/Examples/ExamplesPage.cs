using System.Text.Encodings.Web;
using System.Text.Json;
using PersonalityEngine.Providers.Peterson;

namespace PersonalityEngine.Samples.Examples;

internal static class ExamplesPage
{
    public static string Render(GameExamples.ExampleBundle bundle)
    {
        var json = JsonSerializer.Serialize(ToDto(bundle), JsonOptions);
        return Template.Replace("__CONFIG__", json);
    }

    private static object ToDto(GameExamples.ExampleBundle bundle) => new
    {
        raid = new
        {
            tagged = "HostEvents.Threat(0.8)",
            notices = "Same raid, three Picks: aid, freeze, flee.",
            people = bundle.Raid.People.Select(p => new
            {
                p.Name,
                p.Blurb,
                before = RaidDto(p.Before),
                after = RaidDto(p.After)
            })
        },
        kind = VisitDto(bundle.Kind, "HostEvents.Like + Gratitude + NeedMet", "Haggle and greet stay on top."),
        cruel = VisitDto(bundle.Cruel, "HostEvents.Dislike + Anger + Harm", "Refuse and call-guard take over."),
        scale = new
        {
            notices = "One shrine fire. Four grains of mind. Crowd walkers skipped.",
            frames = bundle.Scale.Frames.Select(f => new
            {
                f.Caption,
                nation = new
                {
                    f.Nation.Chaos,
                    f.Nation.Order,
                    f.Nation.Pick,
                    explore = Score(f.Nation, PetersonMeaningWeighter.Explore),
                    defend = Score(f.Nation, PetersonMeaningWeighter.Defend),
                    integrate = Score(f.Nation, PetersonMeaningWeighter.Integrate),
                    withdraw = Score(f.Nation, PetersonMeaningWeighter.Withdraw)
                },
                village = new { f.Village.Pleasure, f.Village.Arousal },
                priest = new { f.Priest.Anger, f.Priest.Liking }
            })
        }
    };

    private static object VisitDto(GameExamples.VisitStory story, string tagged, string notices) => new
    {
        story.Cruel,
        tagged,
        notices,
        frames = story.Frames.Select(f => new
        {
            f.Visit,
            f.Liking,
            f.Pleasure,
            f.Anger,
            f.Gratitude,
            f.Approach,
            f.Avoid,
            f.Pick
        })
    };

    private static object RaidDto(GameExamples.RaidFrame f) => new
    {
        f.Fear,
        f.Pleasure,
        f.Arousal,
        f.Freeze,
        f.Flee,
        f.Aid,
        f.Pick
    };

    private static float Score(GameExamples.NationSnap nation, string id) =>
        nation.Scores.TryGetValue(id, out var value) ? value : 0f;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static readonly string Template = """
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>Personality Engine — game examples</title>
  <style>
    :root { color-scheme: light; }
    body { font: 14px/1.45 system-ui, sans-serif; margin: 24px; color: #1a1a1a; background: #fff; }
    h1 { font-size: 18px; font-weight: 650; margin: 0 0 4px; }
    h2 { font-size: 16px; margin: 0 0 8px; }
    .caption { color: #555; margin: 0 0 16px; max-width: 920px; }
    .tabs { display: flex; gap: 8px; flex-wrap: wrap; margin: 0 0 16px; }
    .tabs button, .toolbar button { font: inherit; padding: 6px 12px; }
    .tabs button[aria-selected="true"] { font-weight: 650; }
    .panel { display: none; max-width: 960px; }
    .panel.on { display: block; }
    .toolbar { display: flex; flex-wrap: wrap; align-items: center; gap: 10px; margin: 0 0 12px; }
    .people { display: grid; grid-template-columns: 1fr 1fr 1fr; gap: 12px; }
    .card { border: 1px solid #ddd; padding: 12px; }
    .card h3 { margin: 0 0 6px; font-size: 14px; }
    .muted { color: #555; font-size: 12px; margin: 0 0 8px; }
    .bar { height: 10px; background: #eee; margin: 4px 0 8px; }
    .bar > span { display: block; height: 10px; background: #1565c0; }
    .pick { font-weight: 650; }
    .nums { font-variant-numeric: tabular-nums; font-size: 12px; }
    svg { width: 100%; max-width: 640px; height: 160px; display: block; margin: 8px 0 16px; }
    .scale { display: grid; gap: 8px; }
    .band { border: 1px solid #ddd; padding: 10px 12px; }
    .band.active { border-color: #1565c0; }
    .indent { margin-left: 16px; }
    .indent2 { margin-left: 32px; }
    .row2 { display: grid; grid-template-columns: 1fr 1fr; gap: 8px; }
    code { font-size: 12px; }
  </style>
</head>
<body>
  <h1>Personality Engine — three game stories</h1>
  <p class="caption">Real ticks from this library, replayed in the browser. The host still tagged what happened and still Picked. Serve with <code>dotnet run --project samples/Examples -- --serve</code>.</p>
  <div class="tabs" role="tablist">
    <button type="button" id="tab-raid" aria-selected="true">1. Three minds</button>
    <button type="button" id="tab-visits">2. Kind vs cruel</button>
    <button type="button" id="tab-scale">3. Person to nation</button>
  </div>
  <section class="panel on" id="panel-raid"></section>
  <section class="panel" id="panel-visits"></section>
  <section class="panel" id="panel-scale"></section>
  <script>
    const DATA = __CONFIG__;
    const $ = (id) => document.getElementById(id);
    let raidAfter = false, raidTimer = 0;
    let visitCruel = false, visitStep = 0, visitTimer = 0;
    let scaleStep = 0, scaleTimer = 0;

    function pct(v) { return Math.max(0, Math.min(1, v)) * 100; }
    function n(v) { return Number(v).toFixed(2); }

    function bar(label, value) {
      return `<div class="nums">${label} ${n(value)}</div><div class="bar"><span style="width:${pct(value)}%"></span></div>`;
    }

    function showTab(name) {
      ["raid","visits","scale"].forEach((k) => {
        $("tab-" + k).setAttribute("aria-selected", k === name ? "true" : "false");
        $("panel-" + k).classList.toggle("on", k === name);
      });
    }

    function raidFrame(person) { return raidAfter ? person.after : person.before; }

    function renderRaid() {
      const people = DATA.raid.people.map((p) => {
        const f = raidFrame(p);
        return `<article class="card"><h3>${p.name} · <span class="pick">${f.pick}</span></h3>
          <p class="muted">${p.blurb}</p>
          <div class="nums">fear ${n(f.fear)} · pleasure ${n(f.pleasure)} · arousal ${n(f.arousal)}</div>
          ${bar("freeze", f.freeze)}${bar("flee", f.flee)}${bar("aid", f.aid)}</article>`;
      }).join("");
      $("panel-raid").innerHTML = `
        <h2>Three civilians, one raid</h2>
        <p>Same <code>${DATA.raid.tagged}</code>. Three OCEAN seeds. Freeze, flee, and aid were already animated.</p>
        <p><strong>Player notices:</strong> ${DATA.raid.notices}</p>
        <div class="toolbar">
          <button type="button" id="raid-play">${raidTimer ? "Pause" : "Play"}</button>
          <span class="muted">${raidAfter ? "After Threat" : "Quiet street"}</span>
        </div>
        <div class="people">${people}</div>`;
      $("raid-play").onclick = () => {
        if (raidTimer) { clearInterval(raidTimer); raidTimer = 0; renderRaid(); return; }
        raidAfter = false; renderRaid();
        raidTimer = setInterval(() => { raidAfter = !raidAfter; renderRaid(); }, 1400);
      };
    }

    function visitData() { return visitCruel ? DATA.cruel : DATA.kind; }

    function polyline(values, y0, y1) {
      const w = 640, h = 160, p = 16;
      return values.map((v, i) => {
        const x = p + i * ((w - 2 * p) / Math.max(1, values.length - 1));
        const t = (v - y0) / (y1 - y0);
        const y = h - p - t * (h - 2 * p);
        return `${x},${y}`;
      }).join(" ");
    }

    function renderVisits() {
      const story = visitData();
      const f = story.frames[visitStep];
      const lik = story.frames.map((x) => x.liking);
      const pls = story.frames.map((x) => x.pleasure);
      $("panel-visits").innerHTML = `
        <h2>Eight visits to Bram the shopkeeper</h2>
        <p>Slow channel: pairwise liking. Fast channel: gratitude or anger, which decays between visits.</p>
        <p><strong>Host tagged:</strong> <code>${story.tagged}</code></p>
        <p><strong>Player notices:</strong> ${story.notices}</p>
        <div class="toolbar">
          <button type="button" id="visit-kind">Kind</button>
          <button type="button" id="visit-cruel">Cruel</button>
          <button type="button" id="visit-play">${visitTimer ? "Pause" : "Play"}</button>
          <span class="muted">Visit ${f.visit} · pick ${f.pick}</span>
        </div>
        <div class="nums">liking ${n(f.liking)} · pleasure ${n(f.pleasure)} · anger ${n(f.anger)} · gratitude ${n(f.gratitude)}</div>
        ${bar("approach:player", f.approach)}${bar("avoid:player", f.avoid)}
        <svg viewBox="0 0 640 160" role="img" aria-label="Liking and pleasure across visits">
          <polyline fill="none" stroke="#2e7d32" stroke-width="2" points="${polyline(lik, -1, 1)}"></polyline>
          <polyline fill="none" stroke="#1565c0" stroke-width="2" points="${polyline(pls, -0.2, 0.8)}"></polyline>
        </svg>
        <p class="muted">Green: liking (−1..1). Blue: mood pleasure. Gap between visits is 2s of idle decay.</p>`;
      $("visit-kind").onclick = () => { visitCruel = false; visitStep = 0; renderVisits(); };
      $("visit-cruel").onclick = () => { visitCruel = true; visitStep = 0; renderVisits(); };
      $("visit-play").onclick = () => {
        if (visitTimer) { clearInterval(visitTimer); visitTimer = 0; renderVisits(); return; }
        visitStep = 0; renderVisits();
        visitTimer = setInterval(() => {
          visitStep = (visitStep + 1) % story.frames.length;
          renderVisits();
        }, 900);
      };
    }

    function renderScale() {
      const frames = DATA.scale.frames;
      const f = frames[scaleStep];
      const active = (i) => i <= scaleStep ? " active" : "";
      $("panel-scale").innerHTML = `
        <h2>One event, four scales of mind</h2>
        <p>A shrine burns. Nation meaning, village mood, and a named priest each get their own instance. Crowd walkers are skipped.</p>
        <p><strong>Player notices:</strong> ${DATA.scale.notices}</p>
        <div class="toolbar">
          <button type="button" id="scale-play">${scaleTimer ? "Pause" : "Play"}</button>
          <span class="muted">${f.caption}</span>
        </div>
        <div class="scale">
          <div class="band${active(1)}"><strong>Nation — meaning</strong>
            <div class="nums">chaos ${n(f.nation.chaos)} · order ${n(f.nation.order)} · pick ${f.nation.pick}</div>
            ${bar("explore", f.nation.explore)}${bar("defend", f.nation.defend)}${bar("integrate", f.nation.integrate)}${bar("withdraw", f.nation.withdraw)}
          </div>
          <div class="indent"><div class="band${active(2)}"><strong>Village — mood</strong>
            <div class="nums">pleasure ${n(f.village.pleasure)} · arousal ${n(f.village.arousal)}</div>
          </div></div>
          <div class="indent2"><div class="row2">
            <div class="band${active(3)}"><strong>Priest (named)</strong>
              <div class="nums">anger ${n(f.priest.anger)} · liking ${n(f.priest.liking)}</div>
            </div>
            <div class="band"><strong>Crowd walkers</strong>
              <p class="muted" style="margin:0">No instance. Personality-plus-mood seed only, and not in this pulse.</p>
            </div>
          </div></div>
        </div>`;
      $("scale-play").onclick = () => {
        if (scaleTimer) { clearInterval(scaleTimer); scaleTimer = 0; renderScale(); return; }
        scaleStep = 0; renderScale();
        scaleTimer = setInterval(() => {
          scaleStep = (scaleStep + 1) % frames.length;
          renderScale();
        }, 1400);
      };
    }

    $("tab-raid").onclick = () => showTab("raid");
    $("tab-visits").onclick = () => showTab("visits");
    $("tab-scale").onclick = () => showTab("scale");
    renderRaid();
    renderVisits();
    renderScale();
  </script>
</body>
</html>
""";
}
