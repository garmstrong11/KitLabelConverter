namespace KitLabelConverter.Extractor
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.IO.Abstractions;
  using KitLabelConverter.Abstract;
  using KitLabelConverter.Concrete;

  public abstract class ExtractorBase<T> : IExtractor<T> where T : class
  {
    protected readonly IFileSystem FileSystem;
    protected readonly IDataSourceAdapter XlAdapter;
    protected int SheetCount;

    protected ExtractorBase(IDataSourceAdapter adapter, IFileSystem fileSystem)
    {
      if (adapter == null) throw new ArgumentNullException("adapter");
      if (fileSystem == null) throw new ArgumentNullException("fileSystem");

      XlAdapter = adapter;
      FileSystem = fileSystem;
    }

    public string SourcePath { get; protected set; }

    public virtual void Initialize(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        throw new ArgumentNullException("path");

      if (!FileSystem.File.Exists(path))
        throw new FileNotFoundException(string.Format("The file '{0}' could not be found.", path));

      SourcePath = path;
      XlAdapter.Open(SourcePath);
      SheetCount = XlAdapter.SheetCount;
    }

    public ColumnMap GetColumnMap(ISettingsService settingsService, int sheetIndex, int headerRowIndex)
    {
      if (settingsService == null) throw new ArgumentNullException("settingsService");
      if (sheetIndex < 1 || sheetIndex > XlAdapter.SheetCount) {
        throw new ArgumentOutOfRangeException("sheetIndex");
      }

      XlAdapter.ActiveSheet = sheetIndex;
      var rowCount = XlAdapter.RowCount;

      if (headerRowIndex < 1 || headerRowIndex > rowCount) {
        throw new ArgumentOutOfRangeException("headerRowIndex");
      }

      var result = new ColumnMap(settingsService);
      var columnCount = XlAdapter.GetRowColumnCount(headerRowIndex);

      for (var columnIndex = 1; columnIndex <= columnCount; columnIndex++) {
        var columnName = XlAdapter.ExtractString(headerRowIndex, columnIndex).Trim();
        if (string.IsNullOrWhiteSpace(columnName)) continue;

        var locator = new ColumnLocator(columnName, columnIndex);
        result.AddColumnLocator(locator);
      }

      return result;
    }

    public bool IsInitialized { get; protected set; }
    public abstract IEnumerable<T> Extract(ColumnMap columnMap, int sheetIndex, int startRowIndex);
  }
}