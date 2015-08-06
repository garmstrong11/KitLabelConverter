namespace KitLabelConverter.Concrete.Validators
{
  using FluentValidation;

  public class ColumnMapValidator : AbstractValidator<ColumnMap>
  {

    public ColumnMapValidator()
    {
      RuleFor(c => c.AttnColumnId).GreaterThan(0)
        .WithMessage("\"Attn:\" column not found.");

      RuleFor(c => c.DepartmentColumnId).GreaterThan(0)
        .WithMessage("\"Department\" column not found");

      RuleFor(c => c.SbuColumnId).GreaterThan(0)
        .WithMessage("\"SBU (Use Drop Down)\" column not found");

      RuleFor(c => c.ItemNumberColumnId).GreaterThan(0)
        .WithMessage("\"Item Number\" column not found");

      RuleFor(c => c.UpcColumnId).GreaterThan(0)
        .WithMessage("\"UPC Number (Can't have any spaces)\" column not found");

      RuleFor(c => c.KitNameColumnId).GreaterThan(0)
        .WithMessage("\"Kit Name\" column not found");

      RuleFor(c => c.InStoreDateColumnId).GreaterThan(0)
        .WithMessage("\"In-Store Date\" column not found");

      RuleFor(c => c.SetDateColumnId).GreaterThan(0)
        .WithMessage("\"Set Date\" column not found");

      RuleFor(c => c.DestroyDateColumnId).GreaterThan(0)
        .WithMessage("\"Destroy Date\" column not found");

      RuleFor(c => c.HasDuplicateColumns).NotEqual(true)
        .WithMessage("Duplicate columns detected.");
    }
  }
}