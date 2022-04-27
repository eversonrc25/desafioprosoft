
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiFrameWork.auth;

namespace WebApiFrameWork.util
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly String[] _someFilterParameter;

        public CustomAuthorizeAttribute() : base()
        {

        }

        public CustomAuthorizeAttribute(string someFilterParameter): base()
        {

            _someFilterParameter = someFilterParameter.Split(',');

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userService = context.HttpContext.RequestServices.GetService(typeof(IUserAuthService)) as UserAuthService;
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {

                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                // context.Result = new UnauthorizedResult();
                return;
            }
            //JwtRegisteredClaimNames.Jti
            //   var identity2 = user.Identity as ClaimsIdentity;
            //   Claim identityClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);


            var identity = user.Identity as ClaimsIdentity;
            Claim identityClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            Claim jti = identity.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

            // you can also use registered services
            // var someService = context.HttpContext.RequestServices.GetService<ISomeService>();
            UserAuth userAuth = userService.FindOne(jti.Value);

            //user.Claims
            //.Where( x=>x.Type.Equals("EMPRESA")).Select( x=> x.Value).First<String>();

            var isAuthorized = (userAuth == null ? false : userAuth.roles.Intersect(_someFilterParameter).Count() == _someFilterParameter.Length); // someService.IsUserAuthorized(user.Identity.Name, _someFilterParameter);
            if (!isAuthorized)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                //context.Result = Result(HttpStatusCode.Forbidden, "N�o Autorizado!");
                return;
            }


        }

        private ActionResult Result(HttpStatusCode statusCode, string reason) => new ContentResult
        {
            StatusCode = (int)statusCode,
            Content = $"Status Code: {(int)statusCode}; {statusCode}; {reason}",
            ContentType = "application/json",
        };


    }
}