using Loxifi.Interfaces;

namespace Loxifi
{
	public abstract class RetryDelegate
	{
		internal List<ExceptionEvent> _exceptionHandlers = new();

		public RetryDelegate(int retries)
		{
			Retries = retries;
		}

		public int Retries { get; private set; }

		protected HandleResult Handle(Exception exception)
		{
			IExceptionEventArgs eventArgs = ExceptionEventArgs.Create(exception);

			bool isRetry = false;

			for (int i = 0; i < _exceptionHandlers.Count && !eventArgs.StopProcessing; i++)
			{
				ExceptionEvent eevent = _exceptionHandlers[i];

				isRetry = eevent.TryHandle(eventArgs) || isRetry;
			}

			return new HandleResult()
			{
				Delay = eventArgs.Wait,
				IsHandled = isRetry
			};
		}
	}
}