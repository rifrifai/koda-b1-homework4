using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Models;

namespace ConsoleApp.Services;
public class MenuService
{
	private readonly List<MenuItem> _menus = new();
	public MenuService()
	{
		_menus.Add(new Burger("Pitik Bangor", 15_000, 20));
		_menus.Add(new Burger("Single Burger", 23_000, 15));
		_menus.Add(new Burger("Double Burger", 33_000, 12));
		_menus.Add(new Burger("Cheese Burger", 27_000, 15));
		_menus.Add(new Burger("Double Cheese Burger", 38_000, 10));
		_menus.Add(new Burger("Smoky Beef Burger", 30_000, 10));
		_menus.Add(new Burger("Pitik Cheese Burger", 20_000, 15));
		_menus.Add(new Burger("Bangor X", 35_000, 8));
		_menus.Add(new Paket("Paket Hemat Bangor", 42_000, 10));
		_menus.Add(new Minuman("Es Teh", 7000, 30));
		_menus.Add(new Minuman("Es Jeruk", 8000, 30));
	}

	public List<MenuItem> GetAll() => new(_menus);
	public List<MenuItem> Search(string keyword) => _menus.Where(m => m.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
	public List<MenuItem> SortByPrice(List<MenuItem> source, bool asc) => (asc ? source.OrderBy(m => m.Price) : source.OrderByDescending(m => m.Price)).ToList();
	
}