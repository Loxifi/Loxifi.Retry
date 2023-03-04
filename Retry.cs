namespace Loxifi
{
	public static class Retry
	{
		public static RetryAction Call(Action action, int retries) => new(action, retries);
		public static RetryFunc<TReturn> Call<TReturn>(Func<TReturn> action, int retries) => new(action, retries);
	}
}
