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
            context.ValidationMetadata.ValidatorMetadata.All(x => x.GetType() != typeof(RequiredAttribute)))
        {
            context.ValidationMetadata.ValidatorMetadata.Add(new RequiredAttribute());
        }

        foreach (var metadata in context.ValidationMetadata.ValidatorMetadata)
        {
            if (metadata is not ValidationAttribute
                {
                    ErrorMessage: null,
                    ErrorMessageResourceType: null
                }
                attribute)
            {
                continue;
            }

            if (attribute.ErrorMessageResourceName is not null)
            {
                attribute.ErrorMessageResourceType = typeof(Resource);

                continue;
            }

            attribute.ErrorMessage = attribute switch
            {
                RequiredAttribute => Resource.Error_Validation_Required,
                StringLengthAttribute x when x.MinimumLength != 0 => Resource.Error_Validation_StringLengthWithMin,
                StringLengthAttribute => Resource.Error_Validation_StringLength,
                CompareAttribute => Resource.Error_Validation_Compare,
                MinLengthAttribute => Resource.Error_Validation_MinLength,
                MaxLengthAttribute => Resource.Error_Validation_MaxLength,

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
                RangeAttribute => throw new NotImplementedException(),

                RegularExpressionAttribute => throw new NotSupportedException(),

                _ => throw new NotImplementedException()
            };
        }
    }
}