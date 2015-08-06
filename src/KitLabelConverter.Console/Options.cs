namespace KitLabelConverter.Console
{
  using CommandLine;

  public class Options
  {
    [Option('i', "input", Required = true, HelpText = "Input file to read")]
    public string InputPath { get; set; }

    [Option('o', "output", Required = true, HelpText = "Output file path")]
    public string OutputPath { get; set; }
  }
}