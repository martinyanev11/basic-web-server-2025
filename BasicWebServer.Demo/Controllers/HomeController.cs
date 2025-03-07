namespace BasicWebServer.Demo.Controllers
{
    using BasicWebServer.Server.Controllers;
    using BasicWebServer.Server.HTTP;
    using System.Text;
    using System.Web;

    public class HomeController : Controller
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

        public HomeController(Request request) : base(request)
        {
        }

        public Response Index() => Text("Hello from the server!");
        public Response Redirect() => Redirect("https://softuni.org/");
        public Response Html() => Html(HtmlForm);
        public Response HtmlFormPost()
        {
            string formData = string.Empty;

            foreach (var kvp in this.Request.Form)
            {
                formData += $"{kvp.Key} - {kvp.Value}";
                formData += Environment.NewLine;
            }

            return Text(formData);
        }
        public Response Content() => Html(DownloadForm);
        public Response DownloadContent()
        {
            DownloadSitesAsTextFileAsync(FileName,
                new string[] { "https://judge.softuni.org/", "https://softuni.org/" })
                .Wait();

            return File(FileName);
        }
        public Response Cookies()
        {
            if (this.Request.Cookies.Any(c => c.Name != 
            BasicWebServer.Server.HTTP.Session.SessionCookieName))
            {
                StringBuilder cookieText = new StringBuilder();

                cookieText.AppendLine("<h1>Cookies</h1>");

                cookieText.Append(@"<table border='1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in this.Request.Cookies)
                {
                    cookieText.Append("<tr>");

                    cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");

                    cookieText.Append("</tr>");
                }

                cookieText.Append("</table>");

                return Html(cookieText.ToString());
            }

            var cookies = new CookieCollection
            {
                { "My-Cookie", "My-Value" },
                { "My-Second-Cookie", "My-Second-Value" }
            };

            return Html("<h1>No cookies yet!</h1>", cookies);
        }
        public Response Session()
        {
            string currentDateKey = "CurrentDate";
            bool sessionsExists = this.Request.Session
                .ContainsKey(currentDateKey);

            if (sessionsExists)
            {
                var currentDate = this.Request.Session[currentDateKey];
                return Text($"Store date: {currentDate}!");
            }

            return Text("Current date stored!");
        }

        #region Helper methods
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

            await System.IO.File.WriteAllTextAsync(fileName, responsesString);
        }
        #endregion
    }
}
