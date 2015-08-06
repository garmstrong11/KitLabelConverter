namespace KitLabelConverter.Concrete
{
  using System.Collections.Generic;
  using System.Linq;

  public class Job
  {
    private readonly string _headerString;
    private readonly IEnumerable<KitLabel> _kitLabels;

    public Job(string headerString, IEnumerable<KitLabel> kitLabels)
    {
      _headerString = headerString;
      _kitLabels = kitLabels;
    }

    public List<KitLabel> KitLabels
    {
      get { return _kitLabels.ToList(); }
    }

    public List<string> GetOutputList()
    {
      var result = KitLabels.Select(k => k.ToString()).ToList();
      result.Insert(0, _headerString);

      return result;
    }
  }
}