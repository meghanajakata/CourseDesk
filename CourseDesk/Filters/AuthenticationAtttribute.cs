using System.Web ;
using System.Web.Mvc ;
using System.Web.Mvc.Filters;

namespace CourseDesk.Filters
{
    public class AuthenticationAtttribute : IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            // Implement your custom authentication logic here.
            // You can access HttpContext, User, Request, and Response in filterContext.
            if (!IsAuthenticated())
            {
                // Redirect to the login page or take other actions for unauthorized users.
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            // Implement your custom authentication challenge logic here.
        }

        private bool IsAuthenticated( )
        {
            HttpContextAccessor obj = new HttpContextAccessor();
            if(obj.HttpContext.Session.GetInt32("user_id") != null)
            {
                return true;
            }
            return false;
        }
    }
}
