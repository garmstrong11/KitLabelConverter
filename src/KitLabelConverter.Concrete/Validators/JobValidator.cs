namespace KitLabelConverter.Concrete.Validators
{
  using FluentValidation;

  public class JobValidator : AbstractValidator<Job>
  {
    public JobValidator()
    {
      RuleFor(k => k.KitLabels).SetCollectionValidator(new KitLabelValidator());
    }
  }
}