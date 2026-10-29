using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Models;

public abstract class MenuItem
{
	public string Name { get; } = string.Empty;
	public int Price { get; }
	public int Stock { get; private set; }
	protected MenuItem(string name, int price, int stock)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Nama menu tidak boleh kosong!");
		}
		if (price <= 0)
		{
			throw new ArgumentException("Harga harus lebih dari 0!");
		}
		if (stock <= 0)
		{
			throw new ArgumentException("Stok tidak boleh negatif!");
		}

		Name = name;
		Price = price;
		Stock = stock;
	}

	public abstract string Category { get; }
	public bool IsAvailable(int quantity) => quantity > 0 && quantity <= Stock;
	public void ReduceStock(int quantity)
	{
		if (!IsAvailable(quantity))
		{
			throw new InsufficientExecutionStackException($"Stok {Name} tidak mencukupi (sisa: {Stock})");
		}
		Stock -= quantity;
	}
	public override string ToString() => $"{Name} [{Category}]";
}