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


        public async static Task MainAsync()
        {
            HttpServer server = new HttpServer(routes => routes
                .MapGet("/", new TextResponse("Hello from the server!"))
                .MapGet("/HTML", new HtmlResponse(StartUp.HtmlForm))
                .MapGet("/Redirect", new RedirectResponse("https://softuni.org/"))
                .MapPost("/HTML", new TextResponse("", StartUp.AddFormDataAction)));

            await server.StartAsync();
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
