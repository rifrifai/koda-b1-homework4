using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Utils;
public static class CurrencyFormatter
{
	public static string Rupiah (int amount) => "Rp" + amount.ToString("N0", CultureInfo.InvariantCulture).Replace(",", ".");
}