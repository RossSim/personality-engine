using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PersonalityEngine.Samples.AlmaTimeline;

internal static class AlmaTimelinePage
{
    public static string Render(IReadOnlyList<AlmaTimeline.Frame> frames)
    {
        var payload = new
        {
            duration = AlmaTimeline.DurationSeconds,
            joyAt = AlmaTimeline.JoyAtSecond,
            metrics = AlmaTimeline.Metrics.Select(m => new { m.Key, m.Label, m.Color }),
            frames = frames.Select(f => new { t = f.Second, values = f.Values })
        };
        var json = JsonSerializer.Serialize(payload, JsonOptions);
        return Template.Replace("__DATA__", json);
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private static readonly string Template = """
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>Alma composition — 10s timeline</title>
  <style>
    :root { color-scheme: light; }
    body { font: 14px/1.4 system-ui, sans-serif; margin: 24px; color: #1a1a1a; background: #fff; }
    h1 { font-size: 18px; font-weight: 650; margin: 0 0 4px; }
    .caption { color: #555; margin: 0 0 16px; }
    #chart { width: 100%; max-width: 920px; height: auto; display: block; }
    .legend { display: flex; flex-wrap: wrap; gap: 8px 16px; margin: 12px 0 20px; max-width: 920px; }
    .legend span { display: inline-flex; align-items: center; gap: 6px; }
    .swatch { width: 12px; height: 12px; border-radius: 2px; }
    table { border-collapse: collapse; font-variant-numeric: tabular-nums; font-size: 12px; }
    th, td { border: 1px solid #ddd; padding: 4px 6px; text-align: right; }
    th:first-child, td:first-child { text-align: left; white-space: nowrap; }
    th { background: #f4f4f4; font-weight: 600; }
    td.pending { color: #bbb; }
    .clock { margin: 0 0 8px; font-weight: 600; }
  </style>
</head>
<body>
  <h1>Alma composition — 10 second run</h1>
  <p class="caption">Gebhard example traits. 1s ticks. Host tags joy at t=1s, then emotion and mood overlay decay. Y is channel value (−1 to 1); X is time in seconds.</p>
  <p class="clock">t = <span id="clock">0</span>s</p>
  <svg id="chart" viewBox="0 0 920 400" role="img" aria-label="Line chart of affect channels over 10 seconds">
    <g id="grid"></g>
    <g id="series"></g>
  </svg>
  <div class="legend" id="legend"></div>
  <table id="table"></table>
  <script>
    const DATA = __DATA__;
    const W = 920, H = 400, L = 48, R = 16, T = 16, B = 36;
    const plotW = W - L - R, plotH = H - T - B;
    const xMax = DATA.duration, yMin = -1, yMax = 1;
    const x = t => L + (t / xMax) * plotW;
    const y = v => T + (1 - (v - yMin) / (yMax - yMin)) * plotH;

    const grid = document.getElementById("grid");
    const series = document.getElementById("series");
    const legend = document.getElementById("legend");
    const table = document.getElementById("table");
    const clock = document.getElementById("clock");

    function line(x1, y1, x2, y2, stroke, width) {
      const el = document.createElementNS("http://www.w3.org/2000/svg", "line");
      el.setAttribute("x1", x1); el.setAttribute("y1", y1);
      el.setAttribute("x2", x2); el.setAttribute("y2", y2);
      el.setAttribute("stroke", stroke);
      el.setAttribute("stroke-width", width);
      return el;
    }
    function text(content, tx, ty, anchor) {
      const el = document.createElementNS("http://www.w3.org/2000/svg", "text");
      el.textContent = content;
      el.setAttribute("x", tx);
      el.setAttribute("y", ty);
      el.setAttribute("font-size", "11");
      el.setAttribute("fill", "#555");
      el.setAttribute("text-anchor", anchor);
      return el;
    }

    grid.appendChild(line(L, y(0), W - R, y(0), "#bbb", 1));
    for (let t = 0; t <= xMax; t++) {
      grid.appendChild(line(x(t), T, x(t), H - B, "#eee", 1));
      grid.appendChild(text(String(t), x(t), H - 12, "middle"));
    }
    for (let v = yMin; v <= yMax; v += 0.5) {
      grid.appendChild(line(L, y(v), W - R, y(v), "#eee", 1));
      grid.appendChild(text(String(v), L - 8, y(v) + 4, "end"));
    }
    grid.appendChild(text("time (s)", L + plotW / 2, H - 2, "middle"));
    const yLabel = text("value", 14, T + plotH / 2, "middle");
    yLabel.setAttribute("transform", `rotate(-90 14 ${T + plotH / 2})`);
    grid.appendChild(yLabel);

    DATA.metrics.forEach(m => {
      const item = document.createElement("span");
      item.innerHTML = `<i class="swatch" style="background:${m.color}"></i>${m.label}`;
      item.title = m.key;
      legend.appendChild(item);
    });

    const thead = table.createTHead();
    const head = thead.insertRow();
    const h0 = document.createElement("th");
    h0.textContent = "Metric";
    head.appendChild(h0);
    for (let t = 0; t <= xMax; t++) {
      const th = document.createElement("th");
      th.textContent = t + "s";
      head.appendChild(th);
    }
    const tbody = table.createTBody();
    DATA.metrics.forEach((m, mi) => {
      const row = tbody.insertRow();
      row.dataset.metric = String(mi);
      const name = row.insertCell();
      name.textContent = m.label;
      name.style.color = m.color;
      for (let t = 0; t <= xMax; t++) row.insertCell();
    });

    function fmt(v) {
      if (v == null) return "—";
      return Number(v).toLocaleString("en-US", { minimumFractionDigits: 0, maximumFractionDigits: 3 });
    }

    function polylines(values, color) {
      const segs = [];
      let cur = [];
      values.forEach((v, i) => {
        if (v == null) {
          if (cur.length) { segs.push(cur); cur = []; }
        } else {
          cur.push(x(i) + "," + y(v));
        }
      });
      if (cur.length) segs.push(cur);
      return segs.map(pts => {
        const el = document.createElementNS("http://www.w3.org/2000/svg", "polyline");
        el.setAttribute("fill", "none");
        el.setAttribute("stroke", color);
        el.setAttribute("stroke-width", "2");
        el.setAttribute("points", pts.join(" "));
        return el;
      });
    }

    function draw(shown) {
      const lastT = shown - 1;
      clock.textContent = String(Math.max(0, lastT));
      series.replaceChildren();
      DATA.metrics.forEach((m, mi) => {
        const values = DATA.frames.slice(0, shown).map(f => f.values[mi]);
        polylines(values, m.color).forEach(el => series.appendChild(el));
      });
      DATA.metrics.forEach((_, mi) => {
        const cells = tbody.rows[mi].cells;
        for (let t = 0; t <= xMax; t++) {
          const cell = cells[t + 1];
          if (t < shown) {
            cell.textContent = fmt(DATA.frames[t].values[mi]);
            cell.className = "";
          } else {
            cell.textContent = "";
            cell.className = "pending";
          }
        }
      });
    }

    let shown = 1;
    draw(shown);
    const timer = setInterval(() => {
      shown += 1;
      draw(shown);
      if (shown > DATA.duration) clearInterval(timer);
    }, 1000);
  </script>
</body>
</html>
""";
}
