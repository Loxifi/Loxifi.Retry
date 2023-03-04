using Loxifi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loxifi
{
	public static class ExceptionEventArgs
	{
		public static IExceptionEventArgs Create(Exception exception)
		{
			Type toCreate = typeof(ExceptionEventArgs<>).MakeGenericType(exception.GetType());

			IExceptionEventArgs created = Activator.CreateInstance(toCreate) as IExceptionEventArgs;

			created.Exception = exception;

			return created;
		}
	}
	public class ExceptionEventArgs<TException> : IExceptionEventArgs<TException> where TException : Exception
	{
		public ExceptionEventArgs()
		{
		}
		public TException Exception { get; set; }
		public bool StopProcessing { get; set; }
		public int Wait { get; set; }
		Exception IExceptionEventArgs.Exception 
		{
			get => Exception;
			set => Exception = (TException)value; 
		}
	}
}
