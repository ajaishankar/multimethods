using System;
using NUnit.Framework;
using System.Reflection;

namespace MultiMethods.Tests
{
	[TestFixture]
	public class MethodInvokerTests
	{
		MethodInfo action0 = typeof(A).GetMethod("Action0");
		MethodInfo func0 = typeof(A).GetMethod("Func0");
		MethodInfo action1 = typeof(A).GetMethod("Action1");
		MethodInfo func1 = typeof(A).GetMethod("Func1");
		MethodInfo action2 = typeof(A).GetMethod("Action2");
		MethodInfo func2 = typeof(A).GetMethod("Func2");
		MethodInfo action3 = typeof(A).GetMethod("Action3");
		MethodInfo func3 = typeof(A).GetMethod("Func3");
		MethodInfo action4 = typeof(A).GetMethod("Action4");
		MethodInfo func4 = typeof(A).GetMethod("Func4");

		[Test]
		public void Can_Create_And_Invoke_0_Arg_Action()
		{
			A target = new A();
			MethodInvoker action = MethodInvokerHelper.CreateInvoker(action0);
			action(target);
			Assert.AreEqual("0", target.LastResult);
		}

		[Test]
		public void Can_Create_And_Invoke_0_Arg_Func()
		{
			A target = new A();
			MethodInvoker func = MethodInvokerHelper.CreateInvoker(func0);
			object result = func(target);
			Assert.AreEqual("0", result);
		}

		[Test]
		public void Can_Create_And_Invoke_1_Arg_Action()
		{
			A target = new A();
			MethodInvoker action = MethodInvokerHelper.CreateInvoker(action1);
			action(target, 1);
			Assert.AreEqual("1", target.LastResult);
		}

		[Test]
		public void Can_Create_And_Invoke_1_Arg_Func()
		{
			A target = new A();
			MethodInvoker func = MethodInvokerHelper.CreateInvoker(func1);
			object result = func(target, 1);
			Assert.AreEqual("1", result);
		}

		[Test]
		public void Can_Create_And_Invoke_2_Arg_Action()
		{
			A target = new A();
			MethodInvoker action = MethodInvokerHelper.CreateInvoker(action2);
			action(target, 1, 2L);
			Assert.AreEqual("12", target.LastResult);
		}

		[Test]
		public void Can_Create_And_Invoke_2_Arg_Func()
		{
			A target = new A();
			MethodInvoker func = MethodInvokerHelper.CreateInvoker(func2);
			object result = func(target, 1, 2L);
			Assert.AreEqual("12", result);
		}

		[Test]
		public void Can_Create_And_Invoke_3_Arg_Action()
		{
			A target = new A();
			MethodInvoker action = MethodInvokerHelper.CreateInvoker(action3);
			action(target, 1, 2L, 3F);
			Assert.AreEqual("123", target.LastResult);
		}

		[Test]
		public void Can_Create_And_Invoke_3_Arg_Func()
		{
			A target = new A();
			MethodInvoker func = MethodInvokerHelper.CreateInvoker(func3);
			object result = func(target, 1, 2L, 3F);
			Assert.AreEqual("123", result);
		}

		[Test]
		public void Can_Create_And_Invoke_4_Arg_Action()
		{
			A target = new A();
			MethodInvoker action = MethodInvokerHelper.CreateInvoker(action4);
			action(target, 1, 2L, 3F, 4M);
			Assert.AreEqual("1234", target.LastResult);
		}

		[Test]
		public void Can_Create_And_Invoke_4_Arg_Func()
		{
			A target = new A();
			MethodInvoker func = MethodInvokerHelper.CreateInvoker(func4);
			object result = func(target, 1, 2L, 3F, 4M);
			Assert.AreEqual("1234", result);
		}

		private class A
		{
			public string LastResult { get; private set; }

			public void Action0()
			{
				this.LastResult = "0";
			}

			public string Func0()
			{
				return "0";
			}

			public void Action1(int a1)
			{
				this.LastResult = a1.ToString();
			}

			public string Func1(int a1)
			{
				return a1.ToString();
			}

			public void Action2(int a1, long a2)
			{
				this.LastResult = (a1 * 10 + (int)a2).ToString();
			}

			public string Func2(int a1, long a2)
			{
				return (a1 * 10 + (int)a2).ToString();
			}

			public void Action3(int a1, long a2, float a3)
			{
				this.LastResult = (a1 * 100 + (int)a2 * 10 + (int)a3).ToString();
			}

			public string Func3(int a1, long a2, float a3)
			{
				return (a1 * 100 + (int)a2 * 10 + (int)a3).ToString();
			}

			public void Action4(int a1, long a2, float a3, decimal a4)
			{
				this.LastResult = (a1 * 1000 + (int)a2 * 100 + (int)a3 * 10 + (int)a4).ToString();
			}

			public string Func4(int a1, long a2, float a3, decimal a4)
			{
				return (a1 * 1000 + (int)a2 * 100 + (int)a3 * 10 + (int)a4).ToString();
			}
		}
	}
}
