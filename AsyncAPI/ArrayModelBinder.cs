using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AsyncAPI
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                // Binder is only for enumerable types
                if (!bindingContext.ModelMetadata.IsEnumerableType)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    return Task.CompletedTask;
                }

                // Get the inputted value through the value provider (current value for bookIds to find a model name)
                var value = bindingContext.ValueProvider
                    .GetValue(bindingContext.ModelName).ToString();

                // If that value is null or whitespace, return null 
                if (string.IsNullOrWhiteSpace(value))
                {
                    bindingContext.Result = ModelBindingResult.Success(null);
                    return Task.CompletedTask;
                }

                // The value isnt white space
                // an the type of the model is enumerable
                // Get the enumerable's type and a converter
                var elementType = bindingContext.ModelType.GetTypeInfo().GetGenericArguments()[0];
                var converter = TypeDescriptor.GetConverter(elementType);

                // Convert each item in the value list to the enumerable type
                var values = value.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => converter.ConvertFromString(x.Trim()))
                    .ToArray();

                // Create an array of that type, and set it as the Model value 
                var typedValues = Array.CreateInstance(elementType, values.Length);
                values.CopyTo(typedValues, 0);
                bindingContext.Model = typedValues;

                // return a successful result, passing in the Model
                bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            
        }
    }
}