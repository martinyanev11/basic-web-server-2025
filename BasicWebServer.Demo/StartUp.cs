using BasicWebServer.Demo.Controllers;
using BasicWebServer.Server;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using System.Text;
using System.Web;

namespace BasicWebServer.Demo
{
    public class StartUp
    {
        private const string LoginForm = @"<form action='/Login' method='POST'>
   Username: <input type='text' name='Username'/>
   Password: <input type='text' name='Password'/>
   <input type='submit' value ='Log In' /> 
</form>";

        private const string Username = "user";

        private const string Password = "user123";

        public async static Task Main()
        {
            HttpServer server = new HttpServer(routes => routes
                .MapGet<HomeController>("/", c => c.Index())
                .MapGet<HomeController>("/Redirect", c => c.Redirect())
                .MapGet<HomeController>("/HTML", c => c.Html())
                .MapPost<HomeController>("/HTML", c => c.HtmlFormPost())
                .MapGet<HomeController>("/Content", c => c.Content())
                .MapPost<HomeController>("/Content", c => c.DownloadContent())
                .MapGet<HomeController>("/Cookies", c => c.Cookies())
                .MapGet<HomeController>("/Session", c => c.Session()));

            await server.StartAsync();
        }

        private static void LoginAction(Request request, Response response)
        {
            request.Session.Clear();

            string bodyText = string.Empty;

            bool usernameMatches = request.Form["Username"] == StartUp.Username;
            bool passwordMatches = request.Form["Password"] == StartUp.Password;

            if (usernameMatches && passwordMatches)
            {
                request.Session[Session.SessionUserKey] = "MyUserId";

                response.Cookies.Add(Session.SessionCookieName, request.Session.Id);

                bodyText = "<h3>Logged successfully!</h3>";
            }
            else
            {
                bodyText = StartUp.LoginForm;
            }

            response.Body = string.Empty;
            response.Body += bodyText;
        }

        private static void LogoutAction(Request request, Response response)
        {
            request.Session.Clear();

            response.Body = string.Empty;
            response.Body = "<h3>Logged out successfully!</h3>";
        }

        private static void GetUserDataAction(Request request, Response response)
        {
            response.Body = string.Empty;

            if (request.Session.ContainsKey(Session.SessionUserKey))
            {
                response.Body += $@"<h3>Currently logged-in user 
is with username '{Username}'";
            }
            else
            {
                response.Body += $@"<h3>You should first log in 
- <a href='/Login'>Login</a></h3>";

            }
        }
    }
}