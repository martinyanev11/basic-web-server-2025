﻿namespace BasicWebServer.Server.Controllers
{
    using BasicWebServer.Server.HTTP;
    using BasicWebServer.Server.Routing;

    public static class RoutingTableExtension
    {
        public static IRoutingTable MapGet<TController>(
            this IRoutingTable routingTable, 
            string path, 
            Func<TController, Response> controllerFunction) where TController : Controller
        {
            return routingTable.MapGet(path, request => controllerFunction(
                CreateController<TController>(request)));
        }

        public static IRoutingTable MapPost<TController>(
            this IRoutingTable routingTable,
            string path,
            Func<TController, Response> controllerFunction) where TController : Controller
         => routingTable.MapPost(path, request => controllerFunction(
                CreateController<TController>(request)));

        private static TController CreateController<TController>(Request request)
        => (TController)Activator.CreateInstance(typeof(TController), new[] {request});
    }
}
