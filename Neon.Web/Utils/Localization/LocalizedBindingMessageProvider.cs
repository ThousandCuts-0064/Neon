using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Neon.Web.Utils.Localization;

public class LocalizedBindingMessageProvider : DefaultModelBindingMessageProvider
{
    public override Func<string, string> MissingBindRequiredValueAccessor => base.MissingBindRequiredValueAccessor;
    public override Func<string> MissingKeyOrValueAccessor => base.MissingKeyOrValueAccessor;
    public override Func<string> MissingRequestBodyRequiredValueAccessor => base.MissingRequestBodyRequiredValueAccessor;
    public override Func<string, string> ValueMustNotBeNullAccessor => base.ValueMustNotBeNullAccessor;
    public override Func<string, string, string> AttemptedValueIsInvalidAccessor => base.AttemptedValueIsInvalidAccessor;
    public override Func<string, string> NonPropertyAttemptedValueIsInvalidAccessor => base.NonPropertyAttemptedValueIsInvalidAccessor;
    public override Func<string, string> UnknownValueIsInvalidAccessor => base.UnknownValueIsInvalidAccessor;
    public override Func<string> NonPropertyUnknownValueIsInvalidAccessor => base.NonPropertyUnknownValueIsInvalidAccessor;
    public override Func<string, string> ValueIsInvalidAccessor => base.ValueIsInvalidAccessor;
    public override Func<string, string> ValueMustBeANumberAccessor => base.ValueMustBeANumberAccessor;
    public override Func<string> NonPropertyValueMustBeANumberAccessor => base.NonPropertyValueMustBeANumberAccessor;
}
