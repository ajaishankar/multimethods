using System;
using System.Reflection;
using NUnit.Framework;

namespace MultiMethods.Tests
{
	[TestFixture]
	public class DispatcherCacheTests
	{
		MethodInfo m1 = typeof(A).GetMethod("M1");
		MethodInfo m2 = typeof(A).GetMethod("M2");

		class A
		{
			public void M1(IShape s) { }
			public void M2(IShape s) { }
		}

		class B { }

		[SetUp]
		public void Setup()
		{
			DispatcherCache.Clear();
		}

		[Test]
		public void Can_Get_Cache_For_Type()
		{
			DispatcherCache a = DispatcherCache.ForType(typeof(A));
			DispatcherCache b = DispatcherCache.ForType(typeof(B));

			Assert.IsNotNull(a);
			Assert.IsNotNull(b);

			Assert.AreSame(a, DispatcherCache.ForType(typeof(A)));
			Assert.AreSame(b, DispatcherCache.ForType(typeof(B)));
		}

		[Test]
		public void Can_Get_Cached_Dispatcher_By_Method()
		{
			Dispatcher d1 = Dispatcher.For(typeof(A), m1);
			Dispatcher d2 = Dispatcher.For(typeof(A), m2);

			Assert.AreNotSame(d1, d2);
			Assert.AreSame(d1, DispatcherCache.ForType(typeof(A)).DispatcherFor(m1));
			Assert.AreSame(d2, DispatcherCache.ForType(typeof(A)).DispatcherFor(m2));
		}
	}
}