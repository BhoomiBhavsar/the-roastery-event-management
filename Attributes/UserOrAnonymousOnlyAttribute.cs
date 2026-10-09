using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventMVC.Attributes
{
    public class UserOrAnonymousOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity != null &&
                user.Identity.IsAuthenticated &&
                user.IsInRole("Admin"))
            {
                context.Result = new RedirectToActionResult(
                    "Index",              // Change if your dashboard action is not Index
                    "AdminDashboard",     // Controller name without "Controller"
                    new { area = "Admin" }
                );

                return;
            }

            base.OnActionExecuting(context);
        }
    }
}