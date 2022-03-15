using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace AlphaCinema.ModelBinders
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult result = bindingContext
                .ValueProvider.GetValue(bindingContext.ModelName);

            if (result != ValueProviderResult.None && !string.IsNullOrEmpty(result.FirstValue))
            {
                decimal resultValue = 0;
                bool isSuccess = false;

                try
                {
                    string decFirstValue = result.FirstValue;

                    decFirstValue = decFirstValue
                        .Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    decFirstValue = decFirstValue
                        .Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                    resultValue = Convert.ToDecimal(decFirstValue, CultureInfo.CurrentCulture);
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
