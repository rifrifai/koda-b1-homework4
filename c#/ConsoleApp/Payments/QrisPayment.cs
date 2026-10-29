using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Exceptions;

namespace ConsoleApp.Payments;

public class QrisPayment(bool scanned) : IPayment
{
	private readonly bool _scanned = scanned;

	public string Name => "Qris";
	public PaymentResult Process(int total)
	{
		if (!_scanned)
			throw new PaymentFailedException("Pembayaran Qris belum diterima/ dibatalkan!");
		
		return new PaymentResult(Name, total, total, 0);
	}
}