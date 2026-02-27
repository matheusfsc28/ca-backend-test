using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Text.Json;

namespace BillingSystem.Api.Configurations.ModelBinding
{
    public class SnakeCasePropertyBindingMetadataProvider : IBindingMetadataProvider
    {
        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
            if (context.Key.MetadataKind == ModelMetadataKind.Property && context.Key.Name != null)
            {
                var typeNamespace = context.Key.ContainerType?.Namespace;

                if (typeNamespace == null || !typeNamespace.StartsWith("BillingSystem"))
                    return;

                if (string.IsNullOrEmpty(context.BindingMetadata.BinderModelName))
                    context.BindingMetadata.BinderModelName = JsonNamingPolicy.SnakeCaseLower.ConvertName(context.Key.Name);
            }
        }
    }
}
