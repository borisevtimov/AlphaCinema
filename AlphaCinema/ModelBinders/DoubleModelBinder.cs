using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace AlphaCinema.ModelBinders
{
    public class DoubleModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult result = bindingContext
                .ValueProvider.GetValue(bindingContext.ModelName);

            if (result != ValueProviderResult.None && !string.IsNullOrEmpty(result.FirstValue))
            {
                double resultValue = 0;
                bool isSuccess = false;

                try
                {
                    string doubleFirstValue = result.FirstValue;

                    doubleFirstValue = doubleFirstValue
                        .Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    doubleFirstValue = doubleFirstValue
                        .Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                    resultValue = Convert.ToDouble(doubleFirstValue, CultureInfo.CurrentCulture);
                    isSuccess = true;
                }
                catch (FormatException fe)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, fe, bindingContext.ModelMetadata);
                }

                if (isSuccess)
                {
                    bindingContext.Result = ModelBindingResult.Success(resultValue);
                }
            }

            return Task.CompletedTask;
        }
    }
}
