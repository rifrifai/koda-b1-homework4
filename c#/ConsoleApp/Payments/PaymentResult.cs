using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Payments;
public class PaymentResult(string methodName, int total, int paid, int change)
{
	public string MethodName { get; } = methodName;
	public int Total { get; } = total;
	public int Paid { get; } = paid;
	public int Change { get; } = change;
}
