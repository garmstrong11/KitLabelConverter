namespace KitLabelConverter.Concrete.Validators
{
  using System.Linq;
  using FluentValidation;
  using KitLabelConverter.Abstract;

  public class KitLabelValidator : AbstractValidator<KitLabel>
  {
    private readonly ISettingsService _settings;

    public KitLabelValidator(ISettingsService settings)
    {
      _settings = settings;

      RuleFor(k => k.Sbu).NotEmpty()
        .WithMessage("Row {0} has no data for SBU", c => c.RowIndex);

      RuleFor(k => k.ItemNumber).NotEmpty()
        .WithMessage("Row {0} has no data for Item Number", c => c.RowIndex);

      RuleFor(k => k.KitName).NotEmpty()
        .WithMessage("Row {0} has no data for Kit Name", c => c.RowIndex);

      RuleFor(k => k.Upc).Must(NotContainSpaces)
        .WithMessage("The Upc value in Row {0} contains spaces", c => c.RowIndex);

      RuleFor(k => k.Sbu).Must(BeInApprovedSbuList)
        .WithMessage("Row {0} has an unapproved value for SBU: \"{1}\".", c=> c.RowIndex, c => c.Sbu);
    }

    public bool NotContainSpaces(string itemNumber)
    {
      return !itemNumber.Contains(" ");
    }

    public bool BeInApprovedSbuList(string sbu)
    {
      var approvedSbus = _settings.ValidSbuNames.Split(';');
      return approvedSbus.Contains(sbu);
    }
  }
}