using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.Json;

namespace BillingSystem.Api.Configurations.ModelBinding
{
    public class SnakeCaseParameterModelConvention : IParameterModelConvention
    {
        public void Apply(ParameterModel parameter)
        {
            if (parameter.BindingInfo?.BindingSource == Microsoft.AspNetCore.Mvc.ModelBinding.BindingSource.Query)
            {
                if (string.IsNullOrEmpty(parameter.BindingInfo.BinderModelName))
                {
                    parameter.BindingInfo.BinderModelName = JsonNamingPolicy.SnakeCaseLower.ConvertName(parameter.ParameterName);
                }
            }
        }
    }
}
