using System.Text.RegularExpressions;

namespace TestApi.CustomConstraints;

public class MonthsCustomConstraint : IRouteConstraint
{
    public bool Match(
        HttpContext? httpContext,
        IRouter? route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection
    )
    {
        if (!values.ContainsKey(routeKey))
        {
            return false;
        }

        string? monthValue = Convert.ToString(values[routeKey]);

        Regex regex = new Regex("^(apr|jul|oct|jan)$");
        if (regex.IsMatch(monthValue)) {
            return true;
        }

        return false;
    }
}
