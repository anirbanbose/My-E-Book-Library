using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyEbookLibrary.Common;
using MyEbookLibrary.Service.Contracts;

namespace MyEbookLibrary.Api.ActionFilters
{
    public class AccessTokenProcessorAttribute : IActionFilter
    {

        private readonly IAuthService _authService;
        public AccessTokenProcessorAttribute(IAuthService authService)
        {
            _authService = authService;            
        }

        public void OnActionExecuted(ActionExecutedContext context){}

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken);
            if (string.IsNullOrEmpty(accessToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var result = _authService.GetUserFromAccessToken(accessToken).GetAwaiter().GetResult();
            if (result == null)             
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            context.HttpContext.Items.Add(Constants.USER_DTO_NAME, result);
        }
    }
}
