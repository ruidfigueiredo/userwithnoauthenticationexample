using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UserDefinedInWebConfig
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            AuthenticateRequest += OnAuthenticateRequest;
        }

        private void OnAuthenticateRequest(object sender, System.EventArgs e)
        {
            if (ConfigurationManager.AppSettings["test-security"] != "true")
                return;
            
            var username = ConfigurationManager.AppSettings["username"];
            var roles = ConfigurationManager.AppSettings["roles"].Split(' ');

            ClaimsIdentity identity = new ClaimsIdentity(authenticationType: "test-security");
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            roles.ToList().ForEach(role => identity.AddClaim(new Claim(ClaimTypes.Role, role)));
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            HttpContext.Current.User = principal;           
        }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
