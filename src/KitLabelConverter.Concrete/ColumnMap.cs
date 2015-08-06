namespace KitLabelConverter.Concrete
{
  using System.Collections.Generic;
  using System.Linq;
  using KitLabelConverter.Abstract;

  public class ColumnMap
  {
    private readonly HashSet<ColumnLocator> _columnLocators;
    private readonly ISettingsService _settings;
    
    public ColumnMap(ISettingsService settingsService)
    {
      _columnLocators = new HashSet<ColumnLocator>();
      _settings = settingsService;
    }

    public void AddColumnLocator(ColumnLocator item)
    {
      var isAdded = _columnLocators.Add(item);
      if (!isAdded) HasDuplicateColumns = true;
    }

    public bool HasDuplicateColumns { get; private set; }

    public int SbuColumnId
    {
      get { return FindColumnIndex(_settings.SbuColumnName); }
    }

    private int FindColumnIndex(string columnName)
    {
      var locator = _columnLocators.FirstOrDefault(f => f.Name == columnName);
      return locator == null ? -1 : locator.Index;
    } 

    public int AttnColumnId
    {
      get { return FindColumnIndex(_settings.AttnColumnName); }
    }

    public int DepartmentColumnId
    {
      get { return FindColumnIndex(_settings.DepartmentColumnName); }
    }

    public int ItemNumberColumnId
    {
      get { return FindColumnIndex(_settings.ItemNumberColumnName); }
    }

    public int UpcColumnId
    {
      get { return FindColumnIndex(_settings.UpcColumnName); }
    }

    public int KitNameColumnId
    {
      get { return FindColumnIndex(_settings.KitNameColumnName); }
    }

    public int InStoreDateColumnId
    {
      get { return FindColumnIndex(_settings.InStoreDateColumnName); }
    }

    public int SetDateColumnId
    {
      get { return FindColumnIndex(_settings.SetDateColumnName); }
    }

    public int DestroyDateColumnId
    {
      get { return FindColumnIndex(_settings.DestroyDateColumnName); }
    }

    public string GetOutputHeaderString()
    {
      const string fmt = "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}";

      var result = string.Format(fmt,
        _settings.SbuColumnName,
        _settings.AttnColumnName,
        _settings.DepartmentColumnName,
        _settings.ItemNumberColumnName,
        _settings.UpcColumnName,
        _settings.UpcEncodedColumnName,
        _settings.KitNameColumnName,
        _settings.InStoreDateColumnName,
        _settings.SetDateColumnName,
        _settings.DestroyDateColumnName);

      return result;
    }
  }
}