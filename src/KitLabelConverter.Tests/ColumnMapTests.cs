namespace KitLabelConverter.Tests
{
  using System.Linq;
  using FakeItEasy;
  using FluentAssertions;
  using KitLabelConverter.Abstract;
  using KitLabelConverter.Concrete;
  using KitLabelConverter.Extractor;
  using NUnit.Framework;

  [TestFixture]
  public class ColumnMapTests
  {
    private ISettingsService _settings;

    [TestFixtureSetUp]
    public void Init()
    {
      _settings = A.Fake<ISettingsService>();
      A.CallTo(() => _settings.SbuColumnName).Returns("SBU (Use Drop Down)");
      A.CallTo(() => _settings.AttnColumnName).Returns("Attn:");
      A.CallTo(() => _settings.DepartmentColumnName).Returns("Department");
      A.CallTo(() => _settings.ItemNumberColumnName).Returns("Item Number");
      A.CallTo(() => _settings.UpcColumnName).Returns("UPC number (Can't have any spaces)");
      A.CallTo(() => _settings.KitNameColumnName).Returns("Kit Name");
      A.CallTo(() => _settings.InStoreDateColumnName).Returns("In-Store Date");
      A.CallTo(() => _settings.SetDateColumnName).Returns("Set Date");
      A.CallTo(() => _settings.DestroyDateColumnName).Returns("Destroy Date");
    }

    [Test]
    public void AddColumnsWithIdenticalNames_SetsHasDuplicateColumnTrue()
    {
      const string columnName = "SBU (Use Drop Down)";
      var columnMap = new ColumnMap(_settings);

      columnMap.HasDuplicateColumns.Should().BeFalse();
      columnMap.AddColumnLocator(new ColumnLocator(columnName, 69));
      columnMap.AddColumnLocator(new ColumnLocator(columnName, 3));
      columnMap.HasDuplicateColumns.Should().BeTrue();
    }

    [Test]
    public void ColumnLocatorNotFound_ReturnsNegativeOne()
    {
      var columnMap = new ColumnMap(_settings);
      columnMap.SbuColumnId.Should().Be(-1);
    }

    //[Test]
    //public void UnapprovedColumns_ColumnWithUnknownName_ReturnsUnknownColumn()
    //{
    //  var columnMap = new ColumnMap(_settings);
    //  columnMap.AddColumnLocator(new ColumnLocator("SBU (Use Drop Down)", 7));
    //  columnMap.AddColumnLocator(new ColumnLocator("UnknownName1", 5));
    //  columnMap.AddColumnLocator(new ColumnLocator("UnknownName2", 6));

    //  columnMap.UnapprovedColumns.Count.Should().Be(2);
    //}
  }
}