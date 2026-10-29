using ConsoleApp.UI;
try
{
	new Start().Run();
}
catch (EndOfStreamException)
{
	Console.WriteLine();
	Console.WriteLine("Input berakhir. Program ditutup.");
}
