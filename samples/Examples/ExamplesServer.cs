using System.Net;
using System.Text;

namespace PersonalityEngine.Samples.Examples;

internal static class ExamplesServer
{
    public const string Prefix = "http://127.0.0.1:8766/";

    public static void Listen()
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add(Prefix);
        listener.Start();
        Console.WriteLine($"Examples host: {Prefix}");
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
            WriteText(ctx.Response, "text/html; charset=utf-8", GameExamples.ToHtml());
            return;
        }

        ctx.Response.StatusCode = 404;
        WriteText(ctx.Response, "text/plain", "Not found");
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
}
