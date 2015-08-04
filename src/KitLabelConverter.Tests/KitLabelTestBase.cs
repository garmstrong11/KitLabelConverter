namespace KitLabelConverter.Tests
{
  using System.IO;
  using System.Text.RegularExpressions;
  using FakeItEasy;
  using KitLabelConverter.Abstract;
  using NUnit.Framework;

  public class KitLabelTestBase
  {
    protected const string TesterFileName = "TestFile.xlsx";

    public ISettingsService SettingsService
    {
      get
      {
        var settings = A.Fake<ISettingsService>();
        A.CallTo(() => settings.SbuColumnName).Returns("SBU (Use Drop Down)");
        A.CallTo(() => settings.AttnColumnName).Returns("Attn:");
        A.CallTo(() => settings.DepartmentColumnName).Returns("Department");
        A.CallTo(() => settings.ItemNumberColumnName).Returns("Item Number");
        A.CallTo(() => settings.UpcColumnName).Returns("UPC number (Can't have any spaces)");
        A.CallTo(() => settings.UpcEncodedColumnName).Returns("UPC Encoded");
        A.CallTo(() => settings.KitNameColumnName).Returns("Kit Name");
        A.CallTo(() => settings.InStoreDateColumnName).Returns("In-Store Date");
        A.CallTo(() => settings.SetDateColumnName).Returns("Set Date");
        A.CallTo(() => settings.DestroyDateColumnName).Returns("Destroy Date");

        return settings;
      }
    }

    protected string DataFilesDir { get; private set; }
		protected string TesterPath { get; set; }

    public KitLabelTestBase()
    {
	    var testDir = TestContext.CurrentContext.TestDirectory;

	    DataFilesDir = Regex.Replace(testDir, @"bin\\.*$", "TestFiles");
			TesterPath = Path.Combine(DataFilesDir, TesterFileName);
    }
  }
}