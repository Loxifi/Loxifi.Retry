using Loxifi.Interfaces;
using Loxifi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loxifi
{
	public class RetryFunc<TOut> : RetryDelegate
	{
		private readonly Func<TOut> _func;

		public RetryFunc(Func<TOut> func, int retries) : base(retries)
		{
			_func = func;
		}

		public TOut Invoke()
		{
			int retries = this.Retries;

			do
			{
				try
				{
					return _func.Invoke();
				}
				catch (Exception e) when (retries-- > 0 && Handle(e) is HandleResult hr && hr.IsHandled)
				{
					if (hr.Delay > 0)
					{
						System.Threading.Thread.Sleep(hr.Delay);
					}
				}
			} while (true);
		}

		public RetryFunc<TOut> Handle<TException>() where TException : Exception 
		{
			_exceptionHandlers.Add(ExceptionEvent.Create<TException>(true));

			return this;
		}

		public RetryFunc<TOut> Handle<TException>(Func<IExceptionEventArgs<TException>, bool> func) where TException : Exception
		{
			_exceptionHandlers.Add(ExceptionEvent.Create<TException>((e) => func.Invoke((IExceptionEventArgs<TException>)e)));

			return this;
		}

		public RetryFunc<TOut> Handle<TException>(Action<IExceptionEventArgs<TException>> action) where TException : Exception
		{
			_exceptionHandlers.Add(ExceptionEvent.Create<TException>((e) => action.Invoke((IExceptionEventArgs<TException>)e), true));

			return this;
		}

		public RetryFunc<TOut> On<TException>() where TException : Exception
		{
			_exceptionHandlers.Add(ExceptionEvent.Create<TException>(false));

			return this;
		}

		public RetryFunc<TOut> On<TException>(Action<IExceptionEventArgs<TException>> action) where TException : Exception
		{
			_exceptionHandlers.Add(ExceptionEvent.Create<TException>((e) => action.Invoke((IExceptionEventArgs<TException>)e), false));

			return this;
		}
	}
}
