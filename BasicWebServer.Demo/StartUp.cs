using BasicWebServer.Server;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;

namespace BasicWebServer.Demo
{
    public class StartUp
    {
        private const string HtmlForm = @"<form action='/HTML' method='Post'>
   Name: <input type='text' name='Name'/>
   Age: <input type='number' name ='Age'/>
<input type='submit' value ='Save' />
</form>";

        private const string DownloadForm = @"<form action='/Content' method='POST'>
   <input type='submit' value ='Download Sites Content' /> 
</form>";

        private const string FileName = "content.txt";

        public async static Task Main()
        {
            await DownloadSitesAsTextFileAsync(StartUp.FileName,
                new string[] { "https://judge.softuni.org/", "https://softuni.org/" });

            HttpServer server = new HttpServer(routes => routes
                .MapGet("/", new TextResponse("Hello from the server!"))
                .MapGet("/HTML", new HtmlResponse(StartUp.HtmlForm))
                .MapGet("/Redirect", new RedirectResponse("https://softuni.org/"))
                .MapPost("/HTML", new TextResponse("", StartUp.AddFormDataAction))
                .MapGet("/Content", new HtmlResponse(StartUp.DownloadForm))
                .MapPost("/Content", new TextFileResponse(StartUp.FileName)));

            await server.StartAsync();
        }

        private static async Task<string> DownloadWebSiteContentAsync(string url)
        {
            var httpClient = new HttpClient();

            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);

                var html = await response.Content.ReadAsStringAsync();

                return html.Substring(0, 2000);
            }
        }

        private static async Task DownloadSitesAsTextFileAsync
            (string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();

            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContentAsync(url));
            }

            string[] responses = await Task.WhenAll(downloads);

            string responsesString = string.Join
                (Environment.NewLine + new string('-', 100), responses);

            await File.WriteAllTextAsync(fileName, responsesString);
        }

        private static void AddFormDataAction(Request request, Response response)
        {
            response.Body = string.Empty;

            foreach (var kvp in request.Form)
            {
                response.Body += $"{kvp.Key} - {kvp.Value}";
                response.Body += Environment.NewLine;
            }
        }
    }
}