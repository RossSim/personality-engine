using System.Net;
using System.Text;
using System.Text.Json;

namespace PersonalityEngine.Samples.AlmaTimeline;

internal static class AlmaTimelineServer
{
    public const string Prefix = "http://127.0.0.1:8765/";

    public static void Listen()
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add(Prefix);
        listener.Start();
        Console.WriteLine($"Alma timeline host: {Prefix}");
        Console.WriteLine("Ctrl+C to stop.");

        while (true)
        {
            var ctx = listener.GetContext();
            try
            {
                Handle(ctx);
            }
            catch (Exception ex)
            {
                ctx.Response.StatusCode = 500;
                WriteText(ctx.Response, "text/plain", ex.Message);
            }
        }
    }

    private static void Handle(HttpListenerContext ctx)
    {
        var path = ctx.Request.Url?.AbsolutePath ?? "/";
        if (ctx.Request.HttpMethod == "GET" && (path is "/" or "/index.html"))
        {
            WriteText(ctx.Response, "text/html; charset=utf-8", AlmaTimeline.ToHtml());
            return;
        }

        if (ctx.Request.HttpMethod == "POST" && path == "/run")
        {
            using var reader = new StreamReader(ctx.Request.InputStream, ctx.Request.ContentEncoding);
            var body = reader.ReadToEnd();
            var request = Parse(body);
            var frames = AlmaTimeline.Run(request);
            var json = JsonSerializer.Serialize(
                new { frames = frames.Select(f => new { t = f.Second, values = f.Values }) },
                JsonOptions);
            WriteText(ctx.Response, "application/json; charset=utf-8", json);
            return;
        }

        ctx.Response.StatusCode = 404;
        WriteText(ctx.Response, "text/plain", "Not found");
    }

    private static AlmaTimeline.TimelineRequest Parse(string body)
    {
        var dto = JsonSerializer.Deserialize<RunDto>(body, JsonOptions) ?? new RunDto();
        var pulses = new List<AlmaTimeline.OccPulse>();
        if (dto.Pulses is not null)
        {
            foreach (var pulse in dto.Pulses)
            {
                if (string.IsNullOrWhiteSpace(pulse.Kind))
                    continue;
                pulses.Add(new AlmaTimeline.OccPulse(pulse.Kind, pulse.Intensity));
            }
        }

        return new AlmaTimeline.TimelineRequest(pulses, dto.Stagger, dto.FirstAt ?? AlmaTimeline.JoyAtSecond);
    }

    private static void WriteText(HttpListenerResponse response, string type, string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        response.ContentType = type;
        response.ContentEncoding = Encoding.UTF8;
        response.ContentLength64 = bytes.Length;
        response.OutputStream.Write(bytes);
        response.OutputStream.Close();
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    private sealed class RunDto
    {
        public PulseDto[]? Pulses { get; set; }
        public int Stagger { get; set; }
        public int? FirstAt { get; set; }
    }

    private sealed class PulseDto
    {
        public string? Kind { get; set; }
        public float Intensity { get; set; } = 1f;
    }
}
