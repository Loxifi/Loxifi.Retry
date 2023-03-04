using Loxifi.Interfaces;

namespace Loxifi
{
	internal class ExceptionEvent
	{
		private ExceptionEvent(Type exceptionType, bool isRetry)
		{ 
			ExceptionType = exceptionType; 
			IsRetry= isRetry;
		}

		public Action<IExceptionEventArgs> ExceptionAction { get; private set; }

		public Func<IExceptionEventArgs, bool> ExceptionFunc { get; private set; }

		public Type ExceptionType { get; private set; }
		public bool IsRetry { get; private set; }

		public static ExceptionEvent Create<TException>(bool isRetry) where TException : Exception => new(typeof(TException), isRetry);

		public static ExceptionEvent Create<TException>(Func<IExceptionEventArgs, bool> exceptionFunc) where TException : Exception => new(typeof(TException), true) { ExceptionFunc = exceptionFunc };

		public static ExceptionEvent Create<TException>(Action<IExceptionEventArgs> exceptionAction, bool isRetry) where TException : Exception => new(typeof(TException), isRetry) { ExceptionAction = exceptionAction };

		public bool TryHandle(IExceptionEventArgs e)
		{
			if (!ExceptionType.IsAssignableFrom(e.Exception.GetType()))
			{
				return false;
			}

			if (ExceptionAction is null && ExceptionFunc is null)
			{
				return IsRetry;
			}

			if (ExceptionAction is not null)
			{
				ExceptionAction.Invoke(e);
				return IsRetry;
			}

			if (ExceptionFunc is not null)
			{
				return ExceptionFunc.Invoke(e);
			}

			throw new NotImplementedException("We should not have ended up here");
		}
	}
}