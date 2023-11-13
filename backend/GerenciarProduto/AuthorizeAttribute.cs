using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GerenciarProduto
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private string PolicyName { get; set; }

        public AuthorizeAttribute(string policyName) :base()
        {
            PolicyName = policyName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (PolicyName != "APIAuth")
                return;

            if (!context.HttpContext.User.Claims.Any())
            {
                context.Result = new JsonResult(new { message = "Não autorizado" }) { StatusCode = StatusCodes.Status401Unauthorized };

            }
            //var nome = HttpContext.User.Identity?.Name;
        }

        
    }
}
