using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Models;

public class Minuman(string name, int price, int stock) : MenuItem(name, price, stock)
{
	public override string Category => "Minuman";
}