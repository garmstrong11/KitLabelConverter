namespace KitLabelConverter.Concrete
{
  public class KitLabel
  {
    public KitLabel() {}

    public KitLabel(string upc)
    {
      Upc = upc;
    }
    
    public KitLabel(string sbu, string attn, string department, string itemNumber, 
      string upc, string kitName, string inStoreDate, string setDate, string destroyDate)
    {
      Sbu = sbu;
      Attn = attn;
      Department = department;
      ItemNumber = itemNumber;
      Upc = upc;
      KitName = kitName;
      InStoreDate = inStoreDate;
      SetDate = setDate;
      DestroyDate = destroyDate;
    }
    
    public string Sbu { get; set; }
    public string Attn { get; set; }
    public string Department { get; set; }
    public string ItemNumber { get; set; }
    public string Upc { get; set; }

    public string UpcEncoded
    {
      get
      {
        return string.IsNullOrWhiteSpace(Upc) 
          ? string.Empty 
          : BarcodeConverter128.StringToBarcode(Upc);
      }
    }

    public string KitName { get; set; }
    public string InStoreDate { get; set; }
    public string SetDate { get; set; }
    public string DestroyDate { get; set; }

    public bool AllColumnsNull
    {
      get
      {
        return string.IsNullOrWhiteSpace(Sbu)
               && string.IsNullOrWhiteSpace(Attn)
               && string.IsNullOrWhiteSpace(Department)
               && string.IsNullOrWhiteSpace(ItemNumber)
               && string.IsNullOrWhiteSpace(Upc)
               && string.IsNullOrWhiteSpace(KitName)
               && string.IsNullOrWhiteSpace(InStoreDate)
               && string.IsNullOrWhiteSpace(SetDate)
               && string.IsNullOrWhiteSpace(DestroyDate);
      }
    }

    public override string ToString()
    {
      const string fmt = "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}";

      var result = string.Format(fmt, 
        Sbu ?? string.Empty, 
        Attn ?? string.Empty, 
        Department ?? string.Empty, 
        ItemNumber ?? string.Empty,
        Upc ?? string.Empty,
        UpcEncoded ?? string.Empty,
        KitName ?? string.Empty,
        InStoreDate ?? string.Empty,
        SetDate ?? string.Empty,
        DestroyDate ?? string.Empty);

      return result;
    }
  }
}