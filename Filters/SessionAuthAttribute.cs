using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GroanZone.Filters
{
    public class SessionAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var http = context.HttpContext;

            var endpoint = http.GetEndpoint();
            var allowAnon = context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>();
            if (allowAnon is not null)
            {
                base.OnActionExecuting(context);
                return;
            }

            var userId = http.Session.GetInt32("UserId");
            var path = http.Request.Path.Value?.ToLowerInvariant() ?? "/";

            // allow root and auth routes without session
            if (path == "/" || path.StartsWith("/auth"))
            {
                base.OnActionExecuting(context);
                return;
            }

            if (!userId.HasValue)
            {
                context.Result = new RedirectToActionResult("Index", "Auth", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}