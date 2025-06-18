using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;

namespace API_Project.Helpers
{
    public class TrimmingModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(string))
            {
                var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
                var fallbackBinder = new SimpleTypeModelBinder(typeof(string), loggerFactory);
                return new TrimModelBinder(fallbackBinder);
            }

            return null;
        }
    }
}
