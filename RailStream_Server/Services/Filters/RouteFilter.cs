using RailStream_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Services.Filters
{
    static class RouteFilter
    {
        static public List<Route> FilterRouteByName(List<Route> routes, string nameFilter)
        {
            return routes.FindAll((route) => { return route.RouteName.ToLower().IndexOf(nameFilter.ToLower()) > -1; });
        }
        static public List<Route> FilterRouteByDestination(List<Route> routes, string destination)
        {
            return routes.FindAll((route) => { return route.Destination.ToLower().IndexOf(destination.ToLower()) > -1; });
        }
        static public List<Route> FilterRouteByDeparturePlace(List<Route> routes, string departurePlace)
        {
            return routes.FindAll((route) => { return route.DeparturePlace.ToLower().IndexOf(departurePlace.ToLower()) > -1; });
        }
        static public List<Route> FilterRouteByDepartureDate(List<Route> routes, DateTime departureDate)
        {
            return routes.FindAll((route) => { return route.DepartureDate.ToShortDateString() == departureDate.ToShortDateString(); });
        }
    }
}
