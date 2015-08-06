namespace KitLabelConverter.Concrete.Validators
{
  using FluentValidation;
  using KitLabelConverter.Abstract;

  public class JobValidator : AbstractValidator<Job>
  {
    private readonly ISettingsService _settings;

    public JobValidator(ISettingsService settings)
    {
      _settings = settings;
      RuleFor(k => k.KitLabels).SetCollectionValidator(new KitLabelValidator(_settings));
    }
  }
}