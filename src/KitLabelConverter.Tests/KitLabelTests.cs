namespace KitLabelConverter.Tests
{
  using KitLabelConverter.Concrete;
  using NUnit.Framework;

  [TestFixture]
  public class KitLabelTests
  {
    [TestCase("IDA0750", Result = "ÌIDAÇ'R1Î")]
    [TestCase("IDA0741", Result = "ÌIDAÇ'IbÎ")]
    [TestCase("IDA0739", Result = "ÌIDAÇ'GVÎ")]
    [TestCase("IDA0733", Result = "ÌIDAÇ'A2Î")]
    [TestCase("IDA0734", Result = "ÌIDAÇ'B8Î")]
    [TestCase("IDA0735", Result = "ÌIDAÇ'C>Î")]
    public string Converts_Code128(string upc)
    {
      var kitLabel = new KitLabel(69) {Upc = upc};
      
      return kitLabel.UpcEncoded;
    }
  }
}