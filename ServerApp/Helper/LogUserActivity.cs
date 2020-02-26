using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using ServerApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ServerApp.Helper
{
	public class LogUserActivity: IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var actionResult = await next();
			var userId = int.Parse(actionResult.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
			var repos = actionResult.HttpContext.RequestServices.GetService<IDatingRepository>();
			var user = await repos.GetUser(userId);
			user.LastActive = DateTime.Now;
			await repos.SaveAll();
		}
	}
}