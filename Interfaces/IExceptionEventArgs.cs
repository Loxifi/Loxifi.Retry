using System;
using System.Collections.Generic;
using System.Text;

namespace Loxifi.Interfaces
{
	public interface IExceptionEventArgs
	{
		bool StopProcessing { get; set; }
		int Wait { get; set; }
		Exception Exception { get; set; }
	}
}
