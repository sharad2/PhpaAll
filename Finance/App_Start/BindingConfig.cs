using System;
using System.Globalization;
using System.Web.Mvc;

namespace PhpaAll
{
    public static class BindingConfig
    {
        /// <summary>
        /// Expect dates to be posted in dd/mm/yyyy format
        /// http://blog.greatrexpectations.com/2013/01/10/custom-date-formats-and-the-mvc-model-binder/
        /// </summary>
        private class DateTimeModelBinder : IModelBinder
        {

            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (value == null)
                {
                    return null;
                }
                var x = DateTime.ParseExact(value.AttemptedValue, "d/M/yyyy", CultureInfo.InvariantCulture);
                return x;
            }
        }

        public static void RegisterBinders(ModelBinderDictionary binders)
        {
            var binder = new DateTimeModelBinder();
            binders.Add(typeof(DateTime), binder);
            binders.Add(typeof(DateTime?), binder);
        }
    }
}