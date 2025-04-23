using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.AuthorizationFilters;

public class TokenAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        bool hasAuthKey = context.HttpContext.Request.Cookies.TryGetValue("Auth-Key", out string? authKey);

        if (!hasAuthKey || string.IsNullOrEmpty(authKey) || authKey != "A100")
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
