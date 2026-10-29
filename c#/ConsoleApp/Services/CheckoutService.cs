using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Exceptions;
using ConsoleApp.Payments;

namespace ConsoleApp.Services;
public class CheckoutService(Cart cart)
{
	private readonly Cart _cart = cart;

	public PaymentResult Checkout(IPayment payment)
	{
		if (_cart.IsEmpty)
			throw new EmptyCartException();
		
		foreach (var ci in _cart.Items)
		{
			if (!ci.Item.IsAvailable(ci.Quantity))
				throw new InsufficientStockException($"Stok {ci.Item.Name} tidak mencukupi (sisa: {ci.Item.Stock})");
		}

		PaymentResult result = payment.Process(_cart.Total);

		foreach (var ci in _cart.Items)
			ci.Item.ReduceStock(ci.Quantity);

		return result;
	}
}