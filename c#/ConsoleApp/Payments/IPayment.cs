using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Payments;
public interface IPayment
{
	string Name { get; }
	PaymentResult Process(int total);
}