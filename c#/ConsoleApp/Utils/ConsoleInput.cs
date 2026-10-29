using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Utils;
public class ConsoleInput
{
	public string ReadLine(string prompt)
	{
		Console.Write(prompt);
        string? line = Console.ReadLine() ?? throw new EndOfStreamException("Input berakhir");
        return line.Trim();
	}

	public int ReadInt(string prompt)
	{
		while (true)
		{
			string line = ReadLine(prompt);
			if (int.TryParse(line, out int value))
				return value;
			Console.WriteLine("Input tidak valid! Masukkan angka bulat.");
		}
	}

	public bool Confirm(string prompt)
	{
		while (true)
		{
			string line = ReadLine(prompt).ToLower();
			if (line == "" || line == "y") return true;
			if (line == "n") return false;
			Console.WriteLine("Input tidak valid! Ketik Y atau n.");
		}
	}
}