using System;
using NUnit.Framework;
using System.Reflection;

namespace MultiMethods.Tests
{
	[TestFixture]
	public class ConstraintsTests
	{
		MethodInfo m0 = typeof(A).GetMethod("M0");
		MethodInfo m4 = typeof(A).GetMethod("M4");
		MethodInfo m5 = typeof(A).GetMethod("M5");
		MethodInfo g1 = typeof(A).GetMethod("G1");
		MethodInfo r1 = typeof(A).GetMethod("R1");
		MethodInfo o1 = typeof(A).GetMethod("O1");
		MethodInfo s1 = typeof(A).GetMethod("S1");

		class A
		{
			public void M0() { }
			public void M4(int a1, int a2, int a3, int a4) { }
			public void M5(int a1, int a2, int a3, int a4, int a5) { }
			public void G1<A>(A a) { }
			public void R1(ref int x) { }
			public void O1(out int x) { x = 0;  }
			public static void S1(int x) { }
		}

		class G<T> { }

		[Test]
		public void Can_Support_Methods_Upto_4_Parameters()
		{
			Constraints.CheckMethod(m4);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Cannot_Support_Open_Generic_Types()
		{
			Constraints.CheckImplementationType(typeof(G<>));
		}

		[Test]
		public void Can_Support_Closed_Generic_Types()
		{
			Constraints.CheckImplementationType(typeof(G<int>));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Cannot_Support_Methods_Taking_More_Than_4_Parameters()
		{
			Constraints.CheckMethod(m5);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Cannot_Support_Open_Generic_Methods()
		{
			Constraints.CheckMethod(g1);
		}

		[Test]
		public void Can_Support_Closed_Generic_Methods()
		{
			Constraints.CheckMethod(g1.MakeGenericMethod(typeof(int)));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Cannot_Support_Methods_Taking_ByRef_Parameter()
		{
			Constraints.CheckMethod(r1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Cannot_Support_Methods_Taking_Out_Parameter()
		{
			Constraints.CheckMethod(o1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void MultiMethods_Should_Have_AtLeast_1_Parameter()
		{
			Constraints.CheckMultiMethod(m0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void MultiMethods_Cannot_Be_Static()
		{
			Constraints.CheckMultiMethod(s1);
		}
	}
}
