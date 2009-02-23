using System;
using NUnit.Framework;
using NUnit.Framework.Extensions;

namespace MultiMethods.Tests
{
	[TestFixture]
	public class DispatchTableTests
	{
		MethodInvoker m1 = (target, args) => 1;
		MethodInvoker m2 = (target, args) => 2;

		object[] a1 = { 1 };
		object[] a2 = { 1L, 2L };
		object[] a3 = { 1F, 2F, 3F };
		object[] a4 = { 1M, 2M, 3M, 4M };
		object[] a4_2 = { 1, 2L, 3F, 4M };

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void Can_Match_Method_With_1_Arg(bool is32BitPlatform)
		{
			DispatchTable tbl = new DispatchTable(1, is32BitPlatform);
			tbl.Add(a1, m1);
			tbl.Add(a2, m2);

			Assert.AreSame(m1, tbl.Match(a1));
			Assert.AreSame(m2, tbl.Match(a2));
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void Can_Match_Method_With_2_Args(bool is32BitPlatform)
		{
			DispatchTable tbl = new DispatchTable(2, is32BitPlatform);
			tbl.Add(a2, m1);
			tbl.Add(a3, m2);
			Assert.AreSame(m1, tbl.Match(a2));
			Assert.AreSame(m2, tbl.Match(a3));
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void Can_Match_Method_With_3_Args(bool is32BitPlatform)
		{
			DispatchTable tbl = new DispatchTable(3, is32BitPlatform);
			tbl.Add(a3, m1);
			tbl.Add(a4, m2);
			Assert.AreSame(m1, tbl.Match(a3));
			Assert.AreSame(m2, tbl.Match(a4));
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void Can_Match_Method_With_4_Args(bool is32BitPlatform)
		{
			DispatchTable tbl = new DispatchTable(4, is32BitPlatform);
			tbl.Add(a4, m1);
			tbl.Add(a4_2, m2);
			Assert.AreSame(m1, tbl.Match(a4));
			Assert.AreSame(m2, tbl.Match(a4_2));
		}

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void Add_Is_Ignored_If_Entry_Exists(bool is32BitPlatform)
		{
			DispatchTable tbl = new DispatchTable(4, is32BitPlatform);
			tbl.Add(a4, m1);
			tbl.Add(a4, m2);
			Assert.AreEqual(1, tbl.Size);
			Assert.AreSame(m1, tbl.Match(a4));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Only_Upto_4_Arguments_Are_Supported()
		{
			new DispatchTable(5, true);
		}
	}
}
