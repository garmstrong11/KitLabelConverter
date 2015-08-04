namespace KitLabelConverter.Abstract
{
  public interface ISettingsService
	{
    string SbuColumnName { get; set; }
    string AttnColumnName { get; set; }
    string DepartmentColumnName { get; set; }
    string ItemNumberColumnName { get; set; }
    string UpcColumnName { get; set; }
    string KitNameColumnName { get; set; }
    string InStoreDateColumnName { get; set; }
    string SetDateColumnName { get; set; }
    string DestroyDateColumnName { get; set; }
    string UpcEncodedColumnName { get; set; }
    string ValidSbuNames { get; set; } 
	}
}