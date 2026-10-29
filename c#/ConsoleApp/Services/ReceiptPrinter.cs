using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Models;
using ConsoleApp.Payments;
using ConsoleApp.Utils;

namespace ConsoleApp.Services;
public static class ReceiptPrinter
{
	private const int Width = 39;
	public static void Print(IEnumerable<CartItem> items, PaymentResult result)
	{
		Console.WriteLine(Center(" RECEIPT ", '='));
		foreach (var ci in items)
		{
			string left = $"{ci.Item.Name} (x{ci.Quantity})";
			Console.WriteLine(Line(left, CurrencyFormatter.Rupiah(ci.SubTotal)));
		}
		Console.WriteLine(new string('-', Width));
		Console.WriteLine(Line("Total", CurrencyFormatter.Rupiah(result.Total)));
		Console.WriteLine(Line("Metode", result.MethodName));
		Console.WriteLine(Line("Bayar", CurrencyFormatter.Rupiah(result.Paid)));
		Console.WriteLine(Line("Kembalian", CurrencyFormatter.Rupiah(result.Change)));
		Console.WriteLine("Terima kasih sudah membeli!");
	}

	private static string Line(string left, string right)
	{
		int space = Math.Max(1, Width - left.Length - right.Length);
		return left + new string(' ', space) + right;
	}
	
	private static string Center(string text, char fill)
	{
		int side = Math.Max(0, (Width - text.Length) / 2);
		string s = new(fill, side);
		return s + text + s;
	}
}