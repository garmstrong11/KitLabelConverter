namespace KitLabelConverter.Concrete
{
  using System.Collections.Generic;
  using System.Linq;
  using KitLabelConverter.Abstract;

  public class Job
  {
    private readonly string _headerString;

    public Job(string headerString)
    {
      _headerString = headerString;
    }

    public List<KitLabel> KitLabels { get; set; }

    public List<string> GetOutputList()
    {
      var result = KitLabels.Select(k => k.ToString()).ToList();
      result.Insert(0, _headerString);

      return result;
    }
  }
}