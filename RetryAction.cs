using Loxifi.Interfaces;

namespace Loxifi
{
	public class RetryAction : RetryDelegate
	{
		private readonly Action _action;

		public RetryAction(Action action, int retries) : base(retries)
		{
			_action = action;
		}

		public void Invoke()
		{
			int retries = Retries;

			do
			{
				try
				{
					_action.Invoke();
					return;
				}
				catch (Exception e) when (retries-- > 0 && Handle(e) is HandleResult hr && hr.IsHandled)
				{
					if(hr.Delay > 0)
					{
						System.Threading.Thread.Sleep(hr.Delay);
					}
				}
			} while (true);
		}

		public RetryAction Handle<TException>() where TException : Exception
		{
			_exceptionHandlers.Add(ExceptionEvent.Create<TException>(true));

			return this;
		}

		public RetryAction Handle<TException>(Func<IExceptionEventArgs<TException>, bool> func) where TException : Exception
		{
			_exceptionHandlers.Add(ExceptionEvent.Create<TException>((e) => func.Invoke((IExceptionEventArgs<TException>)e)));

			return this;
		}

		public RetryAction Handle<TException>(Action<IExceptionEventArgs<TException>> action) where TException : Exception
		{
			_exceptionHandlers.Add(ExceptionEvent.Create<TException>((e) => action.Invoke((IExceptionEventArgs<TException>)e), true));

			return this;
		}

		public RetryAction On<TException>() where TException : Exception
		{
			_exceptionHandlers.Add(ExceptionEvent.Create<TException>(false));

			return this;
		}

		public RetryAction On<TException>(Action<IExceptionEventArgs<TException>> action) where TException : Exception
		{
			_exceptionHandlers.Add(ExceptionEvent.Create<TException>((e) => action.Invoke((IExceptionEventArgs<TException>)e), false));

			return this;
		}
	}
}
