using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Exceptions;
using ConsoleApp.Utils;

namespace ConsoleApp.Payments;

public class CashPayment(int amountGiven) : IPayment
{
	private readonly int _amountGiven = amountGiven;
    public string Name => "Cash";

	public PaymentResult Process(int total)
	{
		if (_amountGiven <= 0)
			throw new PaymentFailedException("Nominal uang harus lebih dari 0!");
		if (_amountGiven <= total)
			throw new PaymentFailedException($"Uang kurang! kurang {CurrencyFormatter.Rupiah(total - _amountGiven)}");
		return new PaymentResult(Name, total, _amountGiven, _amountGiven - total);
	}
}