namespace KitLabelConverter.Extractor
{
  using System.Collections.Generic;
  using System.Linq;
  using KitLabelConverter.Abstract;

  public class ColumnMap
  {
    protected readonly HashSet<ColumnLocator> ColumnLocators;
    protected readonly ISettingsService Settings;
    
    public ColumnMap(ISettingsService settingsService)
    {
      ColumnLocators = new HashSet<ColumnLocator>();
      Settings = settingsService;
    }

    private IEnumerable<string> GetApprovedColumnNames()
    {
      var infos = Settings.GetType().GetProperties();

      var propertyValues = infos.Where(p => p.PropertyType == typeof(string))
        .Select(info => info.GetValue(Settings).ToString());

      return propertyValues.Where(p => !string.IsNullOrWhiteSpace(p));
    }

    public List<ColumnLocator> UnapprovedColumns
    {
      get
      {
        var approvedNames = GetApprovedColumnNames();
        var unknownColumns = ColumnLocators.Where(c => !approvedNames.Contains(c.Name));

        return unknownColumns.ToList();
      }
    }

    public void AddColumnLocator(ColumnLocator item)
    {
      var isAdded = ColumnLocators.Add(item);
      if (!isAdded) HasDuplicateColumns = true;
    }

    public bool HasDuplicateColumns { get; private set; }

    public int SbuColumnId
    {
      get { return FindColumnIndex(Settings.SbuColumnName); }
    }

    private int FindColumnIndex(string columnName)
    {
      var locator = ColumnLocators.FirstOrDefault(f => f.Name == columnName);
      return locator == null ? -1 : locator.Index;
    } 

    public int AttnColumnId
    {
      get { return FindColumnIndex(Settings.AttnColumnName); }
    }

    public int DepartmentColumnId
    {
      get { return FindColumnIndex(Settings.DepartmentColumnName); }
    }

    public int ItemNumberColumnId
    {
      get { return FindColumnIndex(Settings.ItemNumberColumnName); }
    }

    public int UpcColumnId
    {
      get { return FindColumnIndex(Settings.UpcColumnName); }
    }

    public int KitNameColumnId
    {
      get { return FindColumnIndex(Settings.KitNameColumnName); }
    }

    public int InStoreDateColumnId
    {
      get { return FindColumnIndex(Settings.InStoreDateColumnName); }
    }

    public int SetDateColumnId
    {
      get { return FindColumnIndex(Settings.SetDateColumnName); }
    }

    public int DestroyDateColumnId
    {
      get { return FindColumnIndex(Settings.DestroyDateColumnName); }
    }

    public string GetOutputHeaderString()
    {
      const string fmt = "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}";

      var result = string.Format(fmt,
        Settings.SbuColumnName,
        Settings.AttnColumnName,
        Settings.DepartmentColumnName,
        Settings.ItemNumberColumnName,
        Settings.UpcColumnName,
        Settings.UpcEncodedColumnName,
        Settings.KitNameColumnName,
        Settings.InStoreDateColumnName,
        Settings.SetDateColumnName,
        Settings.DestroyDateColumnName);

      return result;
    }
  }
}