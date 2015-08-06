namespace KitLabelConverter.Extractor
{
  using System;
  using System.Collections.Generic;
  using System.IO.Abstractions;
  using KitLabelConverter.Abstract;
  using KitLabelConverter.Concrete;

  public class KitLabelExtractor : ExtractorBase<KitLabel>
  {
    public KitLabelExtractor(IDataSourceAdapter adapter, IFileSystem fileSystem) 
      : base(adapter, fileSystem) {}

    public override void Initialize(string path)
    {
      base.Initialize(path);
      IsInitialized = true;
    }

    public override IEnumerable<KitLabel> Extract(ColumnMap columnMap, int sheetIndex, int startRowIndex)
    {
      if (columnMap == null) throw new ArgumentNullException("columnMap");
      if (sheetIndex < 1 || sheetIndex > XlAdapter.SheetCount) {
        throw new ArgumentOutOfRangeException("sheetIndex");
      }

      XlAdapter.ActiveSheet = sheetIndex;
      var rowCount = XlAdapter.RowCount;

      if (startRowIndex < 1 || startRowIndex > rowCount) {
        throw new ArgumentOutOfRangeException("startRowIndex");
      }

      for (var row = startRowIndex; row <= rowCount; row++) {
        var label = new KitLabel(row)
        {
          Sbu = XlAdapter.ExtractString(row, columnMap.SbuColumnId),
          Attn = XlAdapter.ExtractString(row, columnMap.AttnColumnId),
          Department = XlAdapter.ExtractString(row, columnMap.DepartmentColumnId),
          ItemNumber = XlAdapter.ExtractString(row, columnMap.ItemNumberColumnId),
          Upc = XlAdapter.ExtractString(row, columnMap.UpcColumnId),
          KitName = XlAdapter.ExtractString(row, columnMap.KitNameColumnId),
          InStoreDate = XlAdapter.ExtractRawString(row, columnMap.InStoreDateColumnId),
          SetDate = XlAdapter.ExtractRawString(row, columnMap.SetDateColumnId),
          DestroyDate = XlAdapter.ExtractRawString(row, columnMap.DestroyDateColumnId)
        };

        if (label.AllColumnsNull) continue;

        yield return label;
      }
    }
  }
}