namespace KitLabelConverter.Console
{
  using System.Collections.Generic;
  using System.Linq;
  using KitLabelConverter.Abstract;
  using KitLabelConverter.Console.Properties;

  public class KitLabelSettings : ISettingsService
  {
    private readonly Settings _settings = Settings.Default;

    public string SbuColumnName
    {
      get { return _settings.SbuColumnName; }
      set { _settings.SbuColumnName = value; }
    }
    public string AttnColumnName 
    {
	    get { return _settings.AttnColumnName; }
	    set { _settings.AttnColumnName = value; }
    }
    public string DepartmentColumnName 
		{
			get { return _settings.DepartmentColumnName; }
			set { _settings.DepartmentColumnName = value; }
		}
    public string ItemNumberColumnName 
		{
			get { return _settings.ItemNumberColumnName; }
			set { _settings.ItemNumberColumnName = value; }
		}
    public string UpcColumnName 
		{
			get { return _settings.UpcColumnName; }
			set { _settings.UpcColumnName = value; }
		}
    public string KitNameColumnName
		{
			get { return _settings.KitNameColumnName; }
			set { _settings.KitNameColumnName = value; }
		}
    public string InStoreDateColumnName 
		{
			get { return _settings.InStoreDateColumnName; }
			set { _settings.InStoreDateColumnName = value; }
		}
    public string SetDateColumnName 
		{
			get { return _settings.SetDateColumnName; }
			set { _settings.SetDateColumnName = value; }
		}
    public string DestroyDateColumnName
		{
			get { return _settings.DestroyDateColumnName; }
			set { _settings.DestroyDateColumnName = value; }
		}
    public string UpcEncodedColumnName 
		{
			get { return _settings.UpcEncodedColumnName; }
			set { _settings.UpcEncodedColumnName = value; }
		}

    public string ValidSbuNames
    {
      get { return _settings.ValidSbuNames; }
      set { _settings.ValidSbuNames = value; }
    }

    public IEnumerable<string> ValidColumnNames
    {
      get
      {
        return GetType().GetProperties()
          .Where(p => p.Name.EndsWith("ColumnName"))
          .Select(info => info.GetValue(_settings).ToString());
      }
    }

    public string ErrorOutputPath
    {
      get { return _settings.ErrorOutputPath; }
      set { _settings.ErrorOutputPath = value; }
    }

    public string DataErrorFileName
    {
      get { return _settings.DataErrorFileName; }
      set { _settings.DataErrorFileName = value; }
    }
  }
}