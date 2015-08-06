namespace KitLabelConverter.Concrete.Validators
{
  using FluentValidation;

  public class KitLabelValidator : AbstractValidator<KitLabel>
  {
    public KitLabelValidator()
    {
      RuleFor(k => k.Sbu).NotEmpty()
        .WithMessage("Row {0} has no data for SBU", c => c.RowIndex);

      RuleFor(k => k.ItemNumber).NotEmpty()
        .WithMessage("Row {0} has no data for Item Number", c => c.RowIndex);

      RuleFor(k => k.KitName).NotEmpty()
        .WithMessage("Row {0} has no data for Kit Name", c => c.RowIndex);

      RuleFor(k => k.Upc).Must(NotContainSpaces)
        .WithMessage("The Upc value in Row {0} contains spaces", c => c.RowIndex);
    }

    public bool NotContainSpaces(string itemNumber)
    {
      return !itemNumber.Contains(" ");
    }
  }
}