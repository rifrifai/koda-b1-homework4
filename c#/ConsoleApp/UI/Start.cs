using ConsoleApp.Exceptions;
using ConsoleApp.Models;
using ConsoleApp.Payments;
using ConsoleApp.Services;
using ConsoleApp.Utils;

namespace ConsoleApp.UI;
public class Start
{
	private readonly ConsoleInput _in = new();
	private readonly MenuService _menuService = new();
	private readonly Cart _cart = new();
	private readonly CheckoutService _checkoutService;

	public Start()
	{
		_checkoutService = new CheckoutService(_cart);
	}

	public void Run()
	{
		bool running = true;
		while (running)
		{
			Console.WriteLine();
			Console.WriteLine("========== WELCOME TO BURGER BANGOR ==========");
			Console.WriteLine("1. Lihat Menu");
			Console.WriteLine("2. Keranjang");
			Console.WriteLine("3. Checkout");
			Console.WriteLine("4. Exit");
			int choice = _in.ReadInt("Silahkan pilih menu: ");

			switch (choice)
			{
				case 1: ShowMenu(); break;
				case 2: ShowCart(); break;
				case 3: Checkout(); break;
				case 4:
					Console.WriteLine("Terima kasih, sampai jumpa!");
					running = false;
					break;
				default:
					Console.WriteLine("Pilihan tidak tersedia!");
					break;
			}
		}
	}

	private void ShowMenu()
	{
		List<MenuItem> current = _menuService.GetAll();
		while (true)
		{
			PrintMenu(current);
			string input = _in.ReadLine(
				"Pilih makanan (nomor, C=cari, U=harga termurah, D=harga termahal, 0=kembali): ");

			switch (input.ToLower())
			{
				case "0":
					return;
				case "c":
					string keyword = _in.ReadLine("Cari menu: ");
					List<MenuItem> found = _menuService.Search(keyword);
					if (found.Count == 0)
						Console.WriteLine("Menu tidak ditemukan!");
					else
						current = found;
					break;
				case "u":
					current = _menuService.SortByPrice(current, true);
					break;
				case "d":
					current = _menuService.SortByPrice(current, false);
					break;
				default:
					if (!int.TryParse(input, out int idx))
					{
						Console.WriteLine("Input tidak valid!");
					}
					else if (idx < 1 || idx > current.Count)
					{
						Console.WriteLine("Nomor menu tidak tersedia!");
					}
					else
					{
						AddToCart(current[idx - 1]);
					}
					break;
			}
		}
	}

	private void AddToCart(MenuItem item)
	{
		int qty = _in.ReadInt("Masukkan quantity: ");
		try
		{
			_cart.Add(item, qty);
			Console.WriteLine("Berhasil ditambahkan ke keranjang!");
		}
		catch (Exception e) when (e is InsufficientStockException or ArgumentException)
		{
			Console.WriteLine("Gagal: " + e.Message);
		}
	}

    private static void PrintMenu(List<MenuItem> list)
	{
		Console.WriteLine();
		Console.WriteLine("=============== MENU ===============");
		for (int i = 0; i < list.Count; i++)
		{
			MenuItem m = list[i];
			Console.WriteLine(
				$"{(i + 1) + ".",-3} {m.Name,-22} {m.Category,-10} stok:{m.Stock,-3} {CurrencyFormatter.Rupiah(m.Price)}");
		}
	}

    private void ShowCart()
	{
		while (true)
		{
			PrintCart();
			if (_cart.IsEmpty) return;

			Console.WriteLine("1. Ubah quantity");
			Console.WriteLine("2. Hapus item");
			Console.WriteLine("0. Kembali");
			int choice = _in.ReadInt("Pilih: ");
			try
			{
				switch (choice)
				{
					case 1:
						int no = _in.ReadInt("Nomor item: ");
						int qty = _in.ReadInt("Quantity baru: ");
						_cart.UpdateQuantity(no - 1, qty);
						Console.WriteLine("Quantity berhasil diubah!");
						break;
					case 2:
						int del = _in.ReadInt("Nomor item yang dihapus: ");
						_cart.Remove(del - 1);
						Console.WriteLine("Item berhasil dihapus!");
						break;
					case 0:
						return;
					default:
						Console.WriteLine("Pilihan tidak tersedia!");
						break;
				}
			}
			catch (Exception e) when (e is InsufficientStockException or ArgumentException)
			{
				Console.WriteLine("Gagal: " + e.Message);
			}
		}
	}

	private void PrintCart()
	{
		Console.WriteLine();
		Console.WriteLine("============= KERANJANG =============");
		if (_cart.IsEmpty)
		{
			Console.WriteLine("Keranjang masih kosong!");
			return;
		}
		var items = _cart.Items;
		for (int i = 0; i < items.Count; i++)
		{
			CartItem ci = items[i];
			Console.WriteLine(
				$"{i + 1}. {ci.Item.Name} (x{ci.Quantity}) {CurrencyFormatter.Rupiah(ci.SubTotal)}");
		}
		Console.WriteLine("Total Belanja: " + CurrencyFormatter.Rupiah(_cart.Total));
	}

    private void Checkout()
	{
		if (_cart.IsEmpty)
		{
			Console.WriteLine("Gagal: " + new EmptyCartException().Message);
			return;
		}
		PrintCart();
		if (!_in.Confirm("Checkout (Y/n): "))
			return;

		while (true)
		{
			Console.WriteLine("Metode Pembayaran:");
			Console.WriteLine("1. Cash");
			Console.WriteLine("2. QRIS");
			Console.WriteLine("0. Batal");
			int method = _in.ReadInt("Pilih metode: ");

			IPayment payment;
			switch (method)
			{
				case 0:
					return;
				case 1:
					payment = new CashPayment(_in.ReadInt("Masukkan uang: "));
					break;
				case 2:
					Console.WriteLine("Scan QRIS untuk membayar " + CurrencyFormatter.Rupiah(_cart.Total));
					payment = new QrisPayment(_in.Confirm("Sudah dibayar? (Y/n): "));
					break;
				default:
					Console.WriteLine("Metode tidak tersedia!");
					continue;
			}

			try
			{
				var snapshot = _cart.Items.ToList();
				PaymentResult result = _checkoutService.Checkout(payment);
				Console.WriteLine();
				ReceiptPrinter.Print(snapshot, result);
				_cart.Clear();
				return;
			}
			catch (Exception e) when (e is EmptyCartException or InsufficientStockException)
			{
				Console.WriteLine("Gagal: " + e.Message);
				return;
			}
			catch (PaymentFailedException e)
			{
				Console.WriteLine("Pembayaran gagal: " + e.Message);
				if (!_in.Confirm("Coba lagi? (Y/n): ")) return;
			}
		}
	}

}