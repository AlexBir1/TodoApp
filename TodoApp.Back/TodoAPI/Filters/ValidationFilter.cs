
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoAPI.APIResponse.Implementations;

namespace TodoAPI.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
                context.Result = new OkObjectResult(new APIResponse<string>(false, context.ModelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

            base.OnActionExecuting(context);
        }
    }
}
