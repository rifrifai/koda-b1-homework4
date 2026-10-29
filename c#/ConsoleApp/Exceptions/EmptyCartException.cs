using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Exceptions;
public class EmptyCartException : Exception
{
	public EmptyCartException() : base("Keranjang masih kosong!") { }
}