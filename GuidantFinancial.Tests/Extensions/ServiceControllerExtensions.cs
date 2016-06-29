using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using GuidantFinancial.Controllers.Api;
using Moq;

namespace GuidantFinancial.Tests.Extensions
{    
    public static class ServiceControllerExtensions
    {

        public static void MockCurrentUser<TController>(this TController controller, string userId, string password) 
        {
            var identity = new GenericIdentity(userId);
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userId));
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId));
            var principal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = new GenericPrincipal
            (
                identity,
                new[] { "admin" }
            );
            
        }        
    }
}
