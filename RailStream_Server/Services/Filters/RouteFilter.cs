using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using RailStream_Server.Models;

namespace RailStream_Server.Services.Filters
{
    static class RouteFilter
    {
        public static List<Route> RouteFilterByDepaturePlace(List<Route> routes, string fromStation)
        {
            if (fromStation == "") return routes;
            else return routes.Where(t => t.DeparturePlace.ToLower().Contains(fromStation.ToLower())).ToList();
        }
            
        public static List<Route> RouteFilterByDestination(List<Route> routes, string toStation)
        {
            if (toStation == "") return routes;
            else return routes.Where(t => t.Destination.ToLower().Contains(toStation.ToLower())).ToList();
        }
            
        public static List<Route> RouteFilterByDepatureDate(List<Route> routes, DateTime? startDate)
        {
            if (startDate == null) return routes;
            else return routes.Where(t => t.DepartureDate == startDate).ToList();
        }
            
    }
}
