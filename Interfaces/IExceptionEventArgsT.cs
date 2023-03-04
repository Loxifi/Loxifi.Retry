using System;
using System.Collections.Generic;
using System.Text;

namespace Loxifi.Interfaces
{
	public interface IExceptionEventArgs<out TException> : IExceptionEventArgs
	{
		new TException Exception { get; }
	}
}
