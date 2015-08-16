namespace KitLabelConverter.Concrete.Validators
{
  using FluentValidation;
  using KitLabelConverter.Abstract;

  public class JobValidator : AbstractValidator<Job>
  {
    public JobValidator(ISettingsService settings)
    {
      RuleFor(k => k.KitLabels).SetCollectionValidator(new KitLabelValidator(settings));

      RuleFor(k => k.KitLabelCount).GreaterThan(0)
        .WithMessage("The data file has the correct format, but no Kit Label rows are present");
    }
  }
}