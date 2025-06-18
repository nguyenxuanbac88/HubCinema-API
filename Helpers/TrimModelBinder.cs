using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace API_Project.Helpers
{
    public class TrimModelBinder : IModelBinder
    {
        private readonly IModelBinder _fallbackBinder;

        public TrimModelBinder(IModelBinder fallbackBinder)
        {
            _fallbackBinder = fallbackBinder;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await _fallbackBinder.BindModelAsync(bindingContext);

            if (bindingContext.Result.IsModelSet && bindingContext.Result.Model is string str)
            {
                bindingContext.Result = ModelBindingResult.Success(str.Trim());
            }
        }
    }
}
