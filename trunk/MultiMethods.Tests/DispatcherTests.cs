using System;
using NUnit.Framework;
using System.Reflection;
using NUnit.Framework.Constraints;
using NUnit.Framework.Extensions;

namespace MultiMethods.Tests
{
	[TestFixture]
	public class DispatcherTests
	{
		MethodInfo multiMethod = typeof(Shape).GetMethod("Intersect");

		IShape circle, rect, roundedRect, morph, ellipse;
		
		[SetUp]
		public void Setup()
		{
			DispatcherCache.Clear();

			circle = new Circle();
			rect = new Rectangle();
			roundedRect = new RoundedRectangle();
			morph = new Morph();
			ellipse = new Ellipse();
		}

		[Test]
		public void Basic_Dispatch_Should_Work()
		{
			Assert.AreEqual("Circle x Circle", circle.Intersect(circle));
			Assert.AreEqual("Circle x Rectangle", circle.Intersect(rect));
			Assert.AreEqual("Circle x Rectangle", circle.Intersect(roundedRect));
			Assert.AreEqual("Circle x IShape", circle.Intersect(morph));

			Assert.AreEqual("Rectangle x Rectangle", rect.Intersect(rect));
			Assert.AreEqual("Rectangle x Circle", rect.Intersect(circle));
			Assert.AreEqual("Shape x Shape", rect.Intersect(morph));
			Assert.AreEqual("Rectangle x Ellipse", rect.Intersect(ellipse));

			Assert.AreEqual("RoundedRectange x Rectangle", roundedRect.Intersect(rect));
			Assert.AreEqual("Rectangle x Circle", roundedRect.Intersect(circle));
			Assert.AreEqual("Shape x Shape", roundedRect.Intersect(morph));
			Assert.AreEqual("RoundedRectange x Ellipse", roundedRect.Intersect(ellipse));

			Assert.AreEqual("Shape x Shape", morph.Intersect(morph));
		}

		[Test]
		public void Can_Dispatch_To_Closed_Generic_Types()
		{
			IShape generic = new GenericShape<int>();

			Assert.AreEqual("GenericShape<Int32> x GenericShape<Int32>", generic.Intersect(generic));
		}

		[Test]
		public void Will_Give_Precedence_To_HideBySig()
		{
			// protected new string Intersect(Rectangle other)
			Assert.AreEqual("RoundedRectange x Rectangle", roundedRect.Intersect(rect));
		}

		[Test]
		public void Will_Give_Precedence_To_Override()
		{
			// protected override string Intersect(Ellipse other)
			Assert.AreEqual("RoundedRectange x Ellipse", roundedRect.Intersect(ellipse));
		}

		[Test]
		public void Using_MultiMethod_Will_Add_To_DispatcherCache()
		{
			// Shape constructor: MultiMethod.Func<IShape, string>(this.Intersect)
			Assert.Greater(DispatcherCache.ForType(typeof(Circle)).Size, 0);
		}

		[Test]
		public void Will_Register_Concrete_Methods_In_Cache()
		{
			Dispatcher dispatcher = Dispatcher.For(typeof(Circle), multiMethod);

			// protected string Intersect(Circle other)
			// protected string Intersect(Rectangle other)
			Assert.AreEqual(2, dispatcher.DispatchTableSize);

			DispatchResult result = dispatcher.Dispatch(circle, new Circle());
			Assert.IsFalse(result.IsDynamicInvoke);

			result = dispatcher.Dispatch(circle, new Rectangle());
			Assert.IsFalse(result.IsDynamicInvoke);
		}

		[Test]
		public void Will_Not_Automatically_Register_Methods_With_Interface_Parameters_In_Cache()
		{
			Dispatcher dispatcher = Dispatcher.For(typeof(Circle), multiMethod);

			// protected new string Intersect(IShape shape)

			DispatchResult result = dispatcher.Dispatch(circle, new Morph());
			Assert.IsTrue(result.IsDynamicInvoke);
		}

		[Test]
		public void Will_Register_Dynamically_Resolved_Methods_In_Cache()
		{
			Dispatcher dispatcher = Dispatcher.For(typeof(Circle), multiMethod);

			Assert.AreEqual(2, dispatcher.DispatchTableSize);

			// protected new string Intersect(IShape shape)
			DispatchResult result = dispatcher.Dispatch(circle, new Morph());
			Assert.IsTrue(result.IsDynamicInvoke);

			Assert.AreEqual(3, dispatcher.DispatchTableSize);

			result = dispatcher.Dispatch(circle, new Morph());
			Assert.IsFalse(result.IsDynamicInvoke);
		}

		[Test]
		public void Will_Not_Try_To_Invoke_MultiMethod_Recursively()
		{
			Dispatcher dispatcher = Dispatcher.For(typeof(Morph), multiMethod);

			DispatchResult result = dispatcher.Dispatch(morph, new Morph());

			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.NoMatch);
		}

		[Test]
		public void Will_Return_NoMatch_If_Method_Target_Is_Null()
		{
			Dispatcher dispatcher = Dispatcher.For(typeof(Circle), multiMethod);

			DispatchResult result = dispatcher.Dispatch(null, new Rectangle());
			Assert.IsTrue(result.NoMatch);
			Assert.IsFalse(result.IsDynamicInvoke);
		}

		[Test]
		public void Will_Return_NoMatch_If_Method_Argument_Is_Null()
		{
			Dispatcher dispatcher = Dispatcher.For(typeof(Circle), multiMethod);

			DispatchResult result = dispatcher.Dispatch(new Circle(), new object[] { null });
			Assert.IsTrue(result.NoMatch);
			Assert.IsFalse(result.IsDynamicInvoke);
		}

		[Test]
		public void Will_Trap_And_Cache_NoMatch()
		{
			Dispatcher dispatcher = Dispatcher.For(typeof(Morph), multiMethod);

			Assert.AreEqual(0, dispatcher.DispatchTableSize);

			DispatchResult result = dispatcher.Dispatch(morph, new Morph());
			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.NoMatch);
			Assert.IsTrue(result.IsDynamicInvoke);

			result = dispatcher.Dispatch(morph, new Morph());
			Assert.IsTrue(result.NoMatch);
			Assert.IsFalse(result.IsDynamicInvoke);
		}

		class Ambig
		{
			public void MM(A a1, A a2) { }
			public int FF(A a1, A a2) { return 0; }

			protected void MM(A a, B b) { }
			protected void MM(B b, A a) { }

			protected void FF(A a, B b) { }
			protected void FF(B b, A a) { }
		}

		[Test]
		[TestCase("MM")]
		[TestCase("FF")]
		public void Will_Trap_And_Cache_AmbiguousMatch(string methodName)
		{
			Dispatcher dispatcher = Dispatcher.For(
				typeof(Ambig), 
				typeof(Ambig).GetMethod(methodName));

			Assert.AreEqual(2, dispatcher.DispatchTableSize);

			DispatchResult result = dispatcher.Dispatch(new Ambig(), new B(), new B());

			Assert.IsFalse(result.Success);
			Assert.IsTrue(result.AmbiguousMatch);
			Assert.IsTrue(result.IsDynamicInvoke);

			if (methodName == "FF")
			{
				DispatchResult<int> typedResult = result.Typed<int>();

				Assert.IsTrue(typedResult.AmbiguousMatch);
				Assert.AreEqual(0, typedResult.ReturnValue);
				Assert.IsTrue(typedResult.IsDynamicInvoke);
			}

			Assert.AreEqual(3, dispatcher.DispatchTableSize);

			result = dispatcher.Dispatch(new Ambig(), new B(), new B());
			Assert.IsFalse(result.IsDynamicInvoke);
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void Can_Dispatch_To_Methods_Taking_1_To_4_Parameters(bool is32BitPlatform)
		{
			Type type = typeof(DispatchTestHelper);

			DispatchTestHelper target = new DispatchTestHelper();

			Dispatcher mm1 = Dispatcher.Create(type, type.GetMethod("MM1"), is32BitPlatform);
			Dispatcher mm2 = Dispatcher.Create(type, type.GetMethod("MM2"), is32BitPlatform);
			Dispatcher mm3 = Dispatcher.Create(type, type.GetMethod("MM3"), is32BitPlatform);
			Dispatcher mm4 = Dispatcher.Create(type, type.GetMethod("MM4"), is32BitPlatform);

			Dispatcher ff1 = Dispatcher.Create(type, type.GetMethod("FF1"), is32BitPlatform);
			Dispatcher ff2 = Dispatcher.Create(type, type.GetMethod("FF2"), is32BitPlatform);
			Dispatcher ff3 = Dispatcher.Create(type, type.GetMethod("FF3"), is32BitPlatform);
			Dispatcher ff4 = Dispatcher.Create(type, type.GetMethod("FF4"), is32BitPlatform);

			DispatchResult result = mm1.Dispatch(target, new D());
			Assert.IsTrue(result.Success);
			Assert.AreEqual("D", target.LastResult);
				
			result = mm2.Dispatch(target, new D(), new D());
			Assert.IsTrue(result.Success);
			Assert.AreEqual("DD", target.LastResult);
			
			result = mm3.Dispatch(target, new D(), new D(), new D());
			Assert.IsTrue(result.Success);
			Assert.AreEqual("DDD", target.LastResult);

			result = mm4.Dispatch(target, new D(), new D(), new D(), new D());
			Assert.IsTrue(result.Success);
			Assert.AreEqual("DDDD", target.LastResult);

			result = ff1.Dispatch(target, new D());
			Assert.IsTrue(result.Success);
			Assert.AreEqual("D", result.ReturnValue);

			result = ff2.Dispatch(target, new D(), new D());
			Assert.IsTrue(result.Success);
			Assert.AreEqual("DD", result.ReturnValue);

			result = ff3.Dispatch(target, new D(), new D(), new D());
			Assert.IsTrue(result.Success);
			Assert.AreEqual("DDD", result.ReturnValue);

			result = ff4.Dispatch(target, new D(), new D(), new D(), new D());
			Assert.IsTrue(result.Success);
			Assert.AreEqual("DDDD", result.ReturnValue);
		}

		[Test]
		public void MultiMethod_Action_And_Function_Wrappers_Will_Dispatch_To_Correct_Method()
		{
			DispatchTestHelper check = new DispatchTestHelper();

			check.MM1(new D());
			Assert.AreEqual("D", check.LastResult);
				
			check.MM2(new D(), new D());
			Assert.AreEqual("DD", check.LastResult);

			check.MM3(new D(), new D(), new D());
			Assert.AreEqual("DDD", check.LastResult);

			check.MM4(new D(), new D(), new D(), new D());
			Assert.AreEqual("DDDD", check.LastResult);

			Assert.AreEqual("D", check.FF1(new D()));
			Assert.AreEqual("DD", check.FF2(new D(), new D()));
			Assert.AreEqual("DDD", check.FF3(new D(), new D(), new D()));
			Assert.AreEqual("DDDD", check.FF4(new D(), new D(), new D(), new D()));
		}

		[Test]
		public void MultiMethod_Wrappers_Will_Dispatch_To_Correct_Target()
		{
			Circle c1 = new Circle(1);
			Circle c2 = new Circle(2);

			MultiMethod.Func<IShape, string> f1 = Dispatcher.Func<IShape, string>(c1.Intersect);
			MultiMethod.Func<IShape, string> f2 = Dispatcher.Func<IShape, string>(c2.Intersect);

			Assert.AreEqual("Circle(1) x Rectangle", f1(new Rectangle()).ReturnValue);
			Assert.AreEqual("Circle(2) x Rectangle", f2(new Rectangle()).ReturnValue);
		}

		[Test]
		public void MultiMethod_Invoke_Is_Faster_Than_Cached_Dynamic_Invoke_To_Target_Method()
		{
			int count = 100000;

			DispatchTestHelper target = new DispatchTestHelper();

			D d1 = new D(), d2 = new D(), d3 = new D();

			MethodInfo method = typeof(DispatchTestHelper).GetMethod(
				"MM3", BindingFlags.Instance | BindingFlags.NonPublic, 
				Type.DefaultBinder, new Type[] { typeof(D), typeof(D), typeof(D) }, null);			

			DateTime start = DateTime.Now;
			for (int i = 0; i < count; ++i)
			{
				method.Invoke(target, new object[] { d1, d2, d3 });
			}
			double dynamicInvokeDuration = (DateTime.Now - start).TotalMilliseconds;

			start = DateTime.Now;
			for (int i = 0; i < count; ++i)
			{
				target.MM3(d1, d2, d3);
			}
			double multiMethodDuration = (DateTime.Now - start).TotalMilliseconds;

			Assert.Less(multiMethodDuration, dynamicInvokeDuration,
				string.Format("dynamic invoke={0} is faster than multi method invoke={1}",
								dynamicInvokeDuration, multiMethodDuration));

			Console.WriteLine("multi method invoke={0} is faster than dynamic invoke={1}",
								multiMethodDuration, dynamicInvokeDuration);
		}
	}
}