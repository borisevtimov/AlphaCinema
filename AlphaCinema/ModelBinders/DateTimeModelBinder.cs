using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace AlphaCinema.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        private readonly string dateFormat;

        public DateTimeModelBinder(string dateFormat)
        {
            this.dateFormat = dateFormat;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult result = bindingContext
                .ValueProvider.GetValue(bindingContext.ModelName);

            if (result != ValueProviderResult.None && !string.IsNullOrEmpty(result.FirstValue))
            {
                DateTime resultDate = DateTime.MinValue;
                bool isSuccess = false;
                string dateValue = result.FirstValue;

                try
                {
                    resultDate = DateTime.ParseExact(dateValue, dateFormat, CultureInfo.InvariantCulture);
                    isSuccess = true;
                }
                catch (FormatException)
                {
                    try
                    {
                        resultDate = DateTime.Parse(dateValue, new CultureInfo("bg-bg"));
                    }
                    catch (Exception e)
                    {
                        bindingContext.ModelState.AddModelError(bindingContext.ModelName, e, bindingContext.ModelMetadata);
                    }

                }
                catch (Exception e) 
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, e, bindingContext.ModelMetadata);
                }

                if (isSuccess)
                {
                    bindingContext.Result = ModelBindingResult.Success(resultDate);
                }
            }

            return Task.CompletedTask;
        }
    }
}
