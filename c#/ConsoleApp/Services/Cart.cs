using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Exceptions;
using ConsoleApp.Models;

namespace ConsoleApp.Services;
public class Cart
{
	private readonly List<CartItem> _items = new();
	public IReadOnlyList<CartItem> Items => _items;
	public bool IsEmpty => _items.Count == 0;

	public void Add(MenuItem item, int quantity)
	{
		if (quantity <= 0) throw new ArgumentException("Quantity harus lebih dari 0!");
		CartItem? existing = _items.FirstOrDefault(ci => ci.Item == item);
		int newQty = quantity + (existing?.Quantity ?? 0);

		if (!item.IsAvailable(newQty))
			throw new InsufficientStockException($"Stok {item.Name} tidak mencukupi (sisa: {item.Stock})");
		
		if (existing == null)
			_items.Add(new CartItem(item, quantity));
		else 
			existing.Quantity += quantity;
	}
	
	public void UpdateQuantity(int index, int quantity)
	{
		CartItem ci = Get(index);
		if (quantity <= 0)
			throw new ArgumentException("Quantity harus lebih dari 0");
		if (!ci.Item.IsAvailable(quantity))
			throw new InsufficientStockException(
				$"Stok {ci.Item.Name} tidak mencukupi (sisa: {ci.Item.Stock})");

		ci.Quantity = quantity;
	}

	public void Remove(int index) => _items.Remove(Get(index));
	public void Clear() => _items.Clear();
	public int Total => _items.Sum(ci => ci.SubTotal);

	private CartItem Get(int index)
	{
		if (index < 0 || index >= _items.Count)
			throw new ArgumentException("Nomor item tidak ada di keranjang");
		return _items[index];
	}
}