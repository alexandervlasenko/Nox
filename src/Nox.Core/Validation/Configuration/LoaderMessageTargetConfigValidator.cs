using FluentValidation;
using Nox.Core.Configuration;

namespace Nox.Core.Validation.Configuration;

public class LoaderMessageTargetConfigValidator: AbstractValidator<LoaderMessageTargetConfiguration>
{
    public LoaderMessageTargetConfigValidator(List<MessagingProviderConfiguration>? providers)
    {
        RuleFor(lmt => lmt.MessagingProvider)
            .NotEmpty()
            .WithMessage(lmt => string.Format(ValidationResources.LoaderMessageTargetProviderEmpty, lmt.DefinitionFileName));
        
        RuleFor(lmt => lmt!.MessagingProvider.ToLower())
            .Must(providerName => (providers != null && providers.Exists(ec => ec.Name!.ToLower() == providerName)) || providerName == "inprocess")
            .WithMessage(lmt => string.Format(ValidationResources.LoaderMessageTargetMissing, lmt.MessagingProvider, lmt.DefinitionFileName));
    }
}