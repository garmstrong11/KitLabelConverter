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
        var sbu = XlAdapter.ExtractString(row, columnMap.SbuColumnId);
        var attn = XlAdapter.ExtractString(row, columnMap.AttnColumnId);
        var dept = XlAdapter.ExtractString(row, columnMap.DepartmentColumnId);
        var item = XlAdapter.ExtractString(row, columnMap.ItemNumberColumnId);
        var upc = XlAdapter.ExtractString(row, columnMap.UpcColumnId);
        var kit = XlAdapter.ExtractString(row, columnMap.KitNameColumnId);
        var inStore = XlAdapter.ExtractRawString(row, columnMap.InStoreDateColumnId);
        var set = XlAdapter.ExtractRawString(row, columnMap.SetDateColumnId);
        var destroy = XlAdapter.ExtractRawString(row, columnMap.DestroyDateColumnId);

        var label = new KitLabel(sbu, attn, dept, item, upc, kit, inStore, set, destroy);

        if (label.AllColumnsNull) continue;

        yield return label;
      }
    }
  }
}