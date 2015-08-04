namespace KitLabelConverter.Tests
{
  using System.IO;
  using System.IO.Abstractions;
  using System.Linq;
  using System.Text;
  using FakeItEasy;
  using FluentAssertions;
  using KitLabelConverter.Extractor;
  using NUnit.Framework;

  [TestFixture]
  public class KitLabelExtractorTests : KitLabelTestBase
  {
    [Test]
    public void CanExtractKitLabels()
    {
      var fileSystem = A.Fake<IFileSystem>();
      A.CallTo(() => fileSystem.File.Exists(A<string>._)).Returns(true);

      var dapter = new FlexCelDataSourceAdapter();
      var extractor = new KitLabelExtractor(dapter, fileSystem);

      extractor.Initialize(TesterPath);
      var columnMap = extractor.GetColumnMap(SettingsService, 1, 1);

      var extracts = extractor.Extract(columnMap, 1, 2).ToList();

      extracts.Count().Should().Be(10);

      var outPath = Path.Combine(DataFilesDir, "testOut.txt");
      var lines = extracts.Select(x => x.ToString()).ToList();
      lines.Insert(0, columnMap.GetOutputHeaderString());

      File.WriteAllLines(outPath, lines, Encoding.Unicode);
    }
  }
}