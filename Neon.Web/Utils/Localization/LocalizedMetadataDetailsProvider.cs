using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Neon.Web.Resources;

namespace Neon.Web.Utils.Localization;

public class LocalizedMetadataDetailsProvider : IValidationMetadataProvider
{
    public void CreateValidationMetadata(ValidationMetadataProviderContext context)
    {
        if (context.Key.ModelType.IsValueType &&
            context.ValidationMetadata.ValidatorMetadata.All(m => m.GetType() != typeof(RequiredAttribute)))
        {
            context.ValidationMetadata.ValidatorMetadata.Add(new RequiredAttribute());
        }

        foreach (var metadata in context.ValidationMetadata.ValidatorMetadata)
        {
            if (metadata is not ValidationAttribute
                {
                    ErrorMessage: null,
                    ErrorMessageResourceName: null
                }
                attribute)
            {
                continue;
            }

            attribute.ErrorMessage = attribute switch
            {
                RequiredAttribute => Resource.Error_Validation_Required,
                StringLengthAttribute a when a.MinimumLength != 0 => Resource.Error_Validation_StringLengthWithMin,
                StringLengthAttribute => Resource.Error_Validation_StringLength,
                CompareAttribute => Resource.Error_Validation_Compare,

                CreditCardAttribute => throw new NotImplementedException(),
                EmailAddressAttribute => throw new NotImplementedException(),
                EnumDataTypeAttribute => throw new NotImplementedException(),
                FileExtensionsAttribute => throw new NotImplementedException(),
                PhoneAttribute => throw new NotImplementedException(),
                UrlAttribute => throw new NotImplementedException(),

                DataTypeAttribute => Resource.Error_Validation_Invalid,

                RemoteAttribute => throw new NotImplementedException(),
                PageRemoteAttribute => throw new NotImplementedException(),
                RemoteAttributeBase => throw new NotImplementedException(),
                AllowedValuesAttribute => throw new NotImplementedException(),
                Base64StringAttribute => throw new NotImplementedException(),
                CustomValidationAttribute => throw new NotImplementedException(),
                DeniedValuesAttribute => throw new NotImplementedException(),
                LengthAttribute => throw new NotImplementedException(),
                MaxLengthAttribute => throw new NotImplementedException(),
                MinLengthAttribute => throw new NotImplementedException(),
                RangeAttribute => throw new NotImplementedException(),
                RegularExpressionAttribute => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
        }
    }
}