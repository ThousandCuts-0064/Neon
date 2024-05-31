using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Neon.Web.Utils.Localization;

public static class LocalizationEx
{
    public static void CopyFrom<T>(this DefaultModelBindingMessageProvider provider)
        where T : ModelBindingMessageProvider, new()
    {
        var newProvider = new T();

        provider.SetAttemptedValueIsInvalidAccessor(newProvider.AttemptedValueIsInvalidAccessor);
        provider.SetMissingBindRequiredValueAccessor(newProvider.MissingBindRequiredValueAccessor);
        provider.SetMissingKeyOrValueAccessor(newProvider.MissingKeyOrValueAccessor);
        provider.SetMissingRequestBodyRequiredValueAccessor(newProvider.MissingRequestBodyRequiredValueAccessor);
        provider.SetNonPropertyAttemptedValueIsInvalidAccessor(newProvider.NonPropertyAttemptedValueIsInvalidAccessor);
        provider.SetNonPropertyUnknownValueIsInvalidAccessor(newProvider.NonPropertyUnknownValueIsInvalidAccessor);
        provider.SetNonPropertyValueMustBeANumberAccessor(newProvider.NonPropertyValueMustBeANumberAccessor);
        provider.SetUnknownValueIsInvalidAccessor(newProvider.UnknownValueIsInvalidAccessor);
        provider.SetValueIsInvalidAccessor(newProvider.ValueIsInvalidAccessor);
        provider.SetValueMustBeANumberAccessor(newProvider.ValueMustBeANumberAccessor);
        provider.SetValueMustNotBeNullAccessor(newProvider.ValueMustNotBeNullAccessor);
    }
}