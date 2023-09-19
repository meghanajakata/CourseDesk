using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using CourseDesk.Models;
using CourseDesk.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CourseDesk.Filters
{
    public class AuthorizationAttribute: Attribute, IAuthorizationFilter
    {
        private readonly UserType _allowedUser;
        public AuthorizationAttribute(UserType  allowedUser)
        {
            _allowedUser = allowedUser;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionUser = context.HttpContext.Session;
            Debug.WriteLine($"session USER is " + sessionUser);
            if (sessionUser == null || sessionUser.GetString("user_type") != _allowedUser.ToString())
            {
                context.Result = new RedirectToActionResult("Error", "Authorization", null);
            }
        }
    }
}
