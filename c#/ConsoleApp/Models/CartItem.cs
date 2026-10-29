using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Models;
public class CartItem(MenuItem item, int quantity)
{
	public MenuItem Item { get; } = item;
	public int Quantity { get; set; } = quantity;

	public int SubTotal => Item.Price * Item.Stock;
}