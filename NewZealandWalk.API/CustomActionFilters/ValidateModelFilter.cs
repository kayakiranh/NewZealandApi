using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NewZealandWalk.API.CustomActionFilters
{
    [Serializable]
    public class ValidateModelFilter : IActionFilter
    {
        private readonly ILogger<ValidateModelFilter> _logger;

        public ValidateModelFilter(ILogger<ValidateModelFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            string actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            ModelStateDictionary modelState = context.ModelState;
            _logger.LogWarning("Model is not valid {controllerName} {actionName} {modelState}", controllerName, actionName, modelState);
            // This method runs before the action method
            _logger.LogInformation($"Action '{context.ActionDescriptor.DisplayName}' is starting.");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            string actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            ModelStateDictionary modelState = context.ModelState;
            _logger.LogWarning("Model is not valid {controllerName} {actionName} {modelState}", controllerName, actionName, modelState);
            // This method runs after the action method
            _logger.LogInformation($"Action '{context.ActionDescriptor.DisplayName}' has completed.");
        }

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
        //    string actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
        //    ModelStateDictionary modelState = context.ModelState;

        //    _logger.LogWarning("Model is not valid {controllerName} {actionName} {modelState}", controllerName, actionName, modelState);

        //    base.OnActionExecuting(context);

        //    //if (!context.ModelState.IsValid)
        //    //{
        //    //    string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
        //    //    string actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
        //    //    ModelStateDictionary modelState = context.ModelState;

        //    //    _logger.LogWarning("Model is not valid {controllerName} {actionName} {modelState}", controllerName, actionName, modelState);
        //    //    context.Result = new BadRequestResult();
        //    //}
        //    //else
        //    //{
        //    //    string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
        //    //    string actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
        //    //    ModelStateDictionary modelState = context.ModelState;

        //    //    _logger.LogWarning("Model is not valid {controllerName} {actionName} {modelState}", controllerName, actionName, modelState);
        //    //}
        //}
    }
}
