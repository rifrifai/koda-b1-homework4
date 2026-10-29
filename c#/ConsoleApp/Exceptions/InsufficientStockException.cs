using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Exceptions;
public class InsufficientStockException(string message) : Exception(message)
{
	
}