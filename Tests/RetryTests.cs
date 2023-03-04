using Loxifi.Tests.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Loxifi.Tests
{
	[TestClass]
	public class RetryTests
	{
		[TestMethod]
		public void TestRetryCountAction()
		{
			int shouldTry = 5;

			int tried = 0;

			try
			{
				Retry.Call(() => throw new TestException(), 5).Handle<TestException>((args) => tried++).Invoke();
			}
			catch (TestException ex)
			{

			}

			Debug.WriteLine("Expected Calls: " + shouldTry);
			Debug.WriteLine("Actual Calls: " + tried);
			Assert.AreEqual(shouldTry, tried);
		}

		[TestMethod]
		public void TestUnhandledCountAction()
		{
			int tried = 0;

			try
			{
				Retry.Call(() => throw new TestException(), 5).On<TestException>((args) => tried++).Invoke();
			}
			catch (TestException ex)
			{

			}

			Debug.WriteLine("Expected Calls: 1");
			Debug.WriteLine("Actual Calls: " + tried);
			Assert.AreEqual(1, tried);
		}

		[TestMethod]
		public void TestConditionalHandleAction()
		{
			int tried = 0;

			try
			{
				Retry.Call(() => throw new TestException(), 5).Handle<TestException>((args) => ++tried < 3).Invoke();
			}
			catch (TestException ex)
			{

			}

			Debug.WriteLine("Expected Calls: 3");
			Debug.WriteLine("Actual Calls: " + tried);
			Assert.AreEqual(3, tried);
		}

		[TestMethod]
		public void TestRetryCountFunc()
		{
			int shouldTry = 5;

			int tried = 0;

			try
			{
				bool result = Retry.Call(() => {
					throw new TestException();
#pragma warning disable CS0162 // Unreachable code detected
					return true;
#pragma warning restore CS0162 // Unreachable code detected
				}, 5).Handle<TestException>((args) => tried++).Invoke();
			} catch(TestException ex)
			{

			}

			Debug.WriteLine("Expected Calls: " + shouldTry);
			Debug.WriteLine("Actual Calls: " + tried);
			Assert.AreEqual(shouldTry, tried);
		}

		[TestMethod]
		public void TestUnhandledCountFunc()
		{
			int tried = 0;

			try
			{
				bool result = Retry.Call(() => {
					throw new TestException();
#pragma warning disable CS0162 // Unreachable code detected
					return true;
#pragma warning restore CS0162 // Unreachable code detected
				}, 5).On<TestException>((args) => tried++).Invoke();
			}
			catch (TestException ex)
			{

			}

			Debug.WriteLine("Expected Calls: 1");
			Debug.WriteLine("Actual Calls: " + tried);
			Assert.AreEqual(1, tried);
		}

		[TestMethod]
		public void TestConditionalHandleFunc()
		{
			int tried = 0;

			try
			{
				bool result = Retry.Call(() => {
					throw new TestException();
#pragma warning disable CS0162 // Unreachable code detected
					return true;
#pragma warning restore CS0162 // Unreachable code detected
				}, 5).Handle<TestException>((args) => ++tried < 3).Invoke();
			}
			catch (TestException ex)
			{

			}

			Debug.WriteLine("Expected Calls: 3");
			Debug.WriteLine("Actual Calls: " + tried);
			Assert.AreEqual(3, tried);
		}

		[TestMethod]
		public void TestWaitFunc()
		{
			DateTime start = DateTime.Now;

			try
			{
				bool result = Retry.Call(() => {
					throw new TestException();
#pragma warning disable CS0162 // Unreachable code detected
					return true;
#pragma warning restore CS0162 // Unreachable code detected
				}, 1).Handle<TestException>((args) => args.Wait = 3000).Invoke();
			}
			catch (TestException ex)
			{

			}

			DateTime end = DateTime.Now;

			long total = (long)(end - start).TotalMilliseconds;

			bool goodtime = total is > 3000 and < 4000;

			Assert.IsTrue(goodtime);
		}

		[TestMethod]
		public void TestWaitAction()
		{
			DateTime start = DateTime.Now;
			try
			{
				Retry.Call(() => throw new TestException(), 1).Handle<TestException>((args) => args.Wait = 3000).Invoke();
			}
			catch (TestException ex)
			{

			}

			DateTime end = DateTime.Now;

			long total = (long)(end - start).TotalMilliseconds;

			bool goodtime = total is > 3000 and < 4000;

			Assert.IsTrue(goodtime);
		}
	}
}
