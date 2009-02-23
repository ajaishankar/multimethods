using System;
using System.Collections.Generic;
using System.Text;

namespace MultiMethods.Tests
{
	public class A { }
	public class B : A { }
	public class C : B { }
	public class D : C { }

	class DispatchTestHelper
	{
		public string LastResult { get; private set; }

		private MultiMethod.Action<A> _mm1;
		private MultiMethod.Action<A, A> _mm2;
		private MultiMethod.Action<A, A, A> _mm3;
		private MultiMethod.Action<A, A, A, A> _mm4;

		private MultiMethod.Func<A, string> _ff1;
		private MultiMethod.Func<A, A, string> _ff2;
		private MultiMethod.Func<A, A, A, string> _ff3;
		private MultiMethod.Func<A, A, A, A, string> _ff4;

		public static A CreateInstance(string aType)
		{
			switch (aType)
			{
				case "A": return new A();
				case "B": return new B();
				case "C": return new C();
				case "D": return new D();
				default:
					throw new Exception("Unknown Type " + aType);
			}
		}

		public DispatchTestHelper()
		{
			_mm1 = Dispatcher.Action<A>(this.MM1);
			_mm2 = Dispatcher.Action<A, A>(this.MM2);
			_mm3 = Dispatcher.Action<A, A, A>(this.MM3);
			_mm4 = Dispatcher.Action<A, A, A, A>(this.MM4);

			_ff1 = Dispatcher.Func<A, string>(this.FF1);
			_ff2 = Dispatcher.Func<A, A, string>(this.FF2);
			_ff3 = Dispatcher.Func<A, A, A, string>(this.FF3);
			_ff4 = Dispatcher.Func<A, A, A, A, string>(this.FF4);
		}

		// begin multi methods
		public void MM1(A a1)
		{
			if (_mm1(a1).NoMatch) LastResult = "A";
		}
		public void MM2(A a1, A a2)
		{
			if (_mm2(a1, a2).NoMatch) LastResult = "AA";
		}
		public void MM3(A a1, A a2, A a3)
		{
			if (_mm3(a1, a2, a3).NoMatch) LastResult = "AAA";
		}
		public void MM4(A a1, A a2, A a3, A a4)
		{
			if (_mm4(a1, a2, a3, a4).NoMatch) LastResult = "AAAA";
		}

		public string FF1(A a1)
		{
			DispatchResult<string> result = _ff1(a1);
			return result.NoMatch ? "A" : result.ReturnValue;
		}
		public string FF2(A a1, A a2)
		{
			DispatchResult<string> result = _ff2(a1, a2);
			return result.NoMatch ? "AA" : result.ReturnValue;
		}
		public string FF3(A a1, A a2, A a3)
		{
			DispatchResult<string> result = _ff3(a1, a2, a3);
			return result.NoMatch ? "AAA" : result.ReturnValue;
		}
		public string FF4(A a1, A a2, A a3, A a4)
		{
			DispatchResult<string> result = _ff4(a1, a2, a3, a4);
			return result.NoMatch ? "AAAA" : result.ReturnValue;
		}
		// end multi methods

		// begin all other combinations
		protected void MM1(B b1) { LastResult = "B"; }
		protected void MM1(C c1) { LastResult = "C"; }
		protected void MM1(D d1) { LastResult = "D"; }
		protected void MM2(A a1, B b2) { LastResult = "AB"; }
		protected void MM2(A a1, C c2) { LastResult = "AC"; }
		protected void MM2(A a1, D d2) { LastResult = "AD"; }
		protected void MM2(B b1, A a2) { LastResult = "BA"; }
		protected void MM2(B b1, B b2) { LastResult = "BB"; }
		protected void MM2(B b1, C c2) { LastResult = "BC"; }
		protected void MM2(B b1, D d2) { LastResult = "BD"; }
		protected void MM2(C c1, A a2) { LastResult = "CA"; }
		protected void MM2(C c1, B b2) { LastResult = "CB"; }
		protected void MM2(C c1, C c2) { LastResult = "CC"; }
		protected void MM2(C c1, D d2) { LastResult = "CD"; }
		protected void MM2(D d1, A a2) { LastResult = "DA"; }
		protected void MM2(D d1, B b2) { LastResult = "DB"; }
		protected void MM2(D d1, C c2) { LastResult = "DC"; }
		protected void MM2(D d1, D d2) { LastResult = "DD"; }
		protected void MM3(A a1, A a2, B b3) { LastResult = "AAB"; }
		protected void MM3(A a1, A a2, C c3) { LastResult = "AAC"; }
		protected void MM3(A a1, A a2, D d3) { LastResult = "AAD"; }
		protected void MM3(A a1, B b2, A a3) { LastResult = "ABA"; }
		protected void MM3(A a1, B b2, B b3) { LastResult = "ABB"; }
		protected void MM3(A a1, B b2, C c3) { LastResult = "ABC"; }
		protected void MM3(A a1, B b2, D d3) { LastResult = "ABD"; }
		protected void MM3(A a1, C c2, A a3) { LastResult = "ACA"; }
		protected void MM3(A a1, C c2, B b3) { LastResult = "ACB"; }
		protected void MM3(A a1, C c2, C c3) { LastResult = "ACC"; }
		protected void MM3(A a1, C c2, D d3) { LastResult = "ACD"; }
		protected void MM3(A a1, D d2, A a3) { LastResult = "ADA"; }
		protected void MM3(A a1, D d2, B b3) { LastResult = "ADB"; }
		protected void MM3(A a1, D d2, C c3) { LastResult = "ADC"; }
		protected void MM3(A a1, D d2, D d3) { LastResult = "ADD"; }
		protected void MM3(B b1, A a2, A a3) { LastResult = "BAA"; }
		protected void MM3(B b1, A a2, B b3) { LastResult = "BAB"; }
		protected void MM3(B b1, A a2, C c3) { LastResult = "BAC"; }
		protected void MM3(B b1, A a2, D d3) { LastResult = "BAD"; }
		protected void MM3(B b1, B b2, A a3) { LastResult = "BBA"; }
		protected void MM3(B b1, B b2, B b3) { LastResult = "BBB"; }
		protected void MM3(B b1, B b2, C c3) { LastResult = "BBC"; }
		protected void MM3(B b1, B b2, D d3) { LastResult = "BBD"; }
		protected void MM3(B b1, C c2, A a3) { LastResult = "BCA"; }
		protected void MM3(B b1, C c2, B b3) { LastResult = "BCB"; }
		protected void MM3(B b1, C c2, C c3) { LastResult = "BCC"; }
		protected void MM3(B b1, C c2, D d3) { LastResult = "BCD"; }
		protected void MM3(B b1, D d2, A a3) { LastResult = "BDA"; }
		protected void MM3(B b1, D d2, B b3) { LastResult = "BDB"; }
		protected void MM3(B b1, D d2, C c3) { LastResult = "BDC"; }
		protected void MM3(B b1, D d2, D d3) { LastResult = "BDD"; }
		protected void MM3(C c1, A a2, A a3) { LastResult = "CAA"; }
		protected void MM3(C c1, A a2, B b3) { LastResult = "CAB"; }
		protected void MM3(C c1, A a2, C c3) { LastResult = "CAC"; }
		protected void MM3(C c1, A a2, D d3) { LastResult = "CAD"; }
		protected void MM3(C c1, B b2, A a3) { LastResult = "CBA"; }
		protected void MM3(C c1, B b2, B b3) { LastResult = "CBB"; }
		protected void MM3(C c1, B b2, C c3) { LastResult = "CBC"; }
		protected void MM3(C c1, B b2, D d3) { LastResult = "CBD"; }
		protected void MM3(C c1, C c2, A a3) { LastResult = "CCA"; }
		protected void MM3(C c1, C c2, B b3) { LastResult = "CCB"; }
		protected void MM3(C c1, C c2, C c3) { LastResult = "CCC"; }
		protected void MM3(C c1, C c2, D d3) { LastResult = "CCD"; }
		protected void MM3(C c1, D d2, A a3) { LastResult = "CDA"; }
		protected void MM3(C c1, D d2, B b3) { LastResult = "CDB"; }
		protected void MM3(C c1, D d2, C c3) { LastResult = "CDC"; }
		protected void MM3(C c1, D d2, D d3) { LastResult = "CDD"; }
		protected void MM3(D d1, A a2, A a3) { LastResult = "DAA"; }
		protected void MM3(D d1, A a2, B b3) { LastResult = "DAB"; }
		protected void MM3(D d1, A a2, C c3) { LastResult = "DAC"; }
		protected void MM3(D d1, A a2, D d3) { LastResult = "DAD"; }
		protected void MM3(D d1, B b2, A a3) { LastResult = "DBA"; }
		protected void MM3(D d1, B b2, B b3) { LastResult = "DBB"; }
		protected void MM3(D d1, B b2, C c3) { LastResult = "DBC"; }
		protected void MM3(D d1, B b2, D d3) { LastResult = "DBD"; }
		protected void MM3(D d1, C c2, A a3) { LastResult = "DCA"; }
		protected void MM3(D d1, C c2, B b3) { LastResult = "DCB"; }
		protected void MM3(D d1, C c2, C c3) { LastResult = "DCC"; }
		protected void MM3(D d1, C c2, D d3) { LastResult = "DCD"; }
		protected void MM3(D d1, D d2, A a3) { LastResult = "DDA"; }
		protected void MM3(D d1, D d2, B b3) { LastResult = "DDB"; }
		protected void MM3(D d1, D d2, C c3) { LastResult = "DDC"; }
		protected void MM3(D d1, D d2, D d3) { LastResult = "DDD"; }
		protected void MM4(A a1, A a2, A a3, B b4) { LastResult = "AAAB"; }
		protected void MM4(A a1, A a2, A a3, C c4) { LastResult = "AAAC"; }
		protected void MM4(A a1, A a2, A a3, D d4) { LastResult = "AAAD"; }
		protected void MM4(A a1, A a2, B b3, A a4) { LastResult = "AABA"; }
		protected void MM4(A a1, A a2, B b3, B b4) { LastResult = "AABB"; }
		protected void MM4(A a1, A a2, B b3, C c4) { LastResult = "AABC"; }
		protected void MM4(A a1, A a2, B b3, D d4) { LastResult = "AABD"; }
		protected void MM4(A a1, A a2, C c3, A a4) { LastResult = "AACA"; }
		protected void MM4(A a1, A a2, C c3, B b4) { LastResult = "AACB"; }
		protected void MM4(A a1, A a2, C c3, C c4) { LastResult = "AACC"; }
		protected void MM4(A a1, A a2, C c3, D d4) { LastResult = "AACD"; }
		protected void MM4(A a1, A a2, D d3, A a4) { LastResult = "AADA"; }
		protected void MM4(A a1, A a2, D d3, B b4) { LastResult = "AADB"; }
		protected void MM4(A a1, A a2, D d3, C c4) { LastResult = "AADC"; }
		protected void MM4(A a1, A a2, D d3, D d4) { LastResult = "AADD"; }
		protected void MM4(A a1, B b2, A a3, A a4) { LastResult = "ABAA"; }
		protected void MM4(A a1, B b2, A a3, B b4) { LastResult = "ABAB"; }
		protected void MM4(A a1, B b2, A a3, C c4) { LastResult = "ABAC"; }
		protected void MM4(A a1, B b2, A a3, D d4) { LastResult = "ABAD"; }
		protected void MM4(A a1, B b2, B b3, A a4) { LastResult = "ABBA"; }
		protected void MM4(A a1, B b2, B b3, B b4) { LastResult = "ABBB"; }
		protected void MM4(A a1, B b2, B b3, C c4) { LastResult = "ABBC"; }
		protected void MM4(A a1, B b2, B b3, D d4) { LastResult = "ABBD"; }
		protected void MM4(A a1, B b2, C c3, A a4) { LastResult = "ABCA"; }
		protected void MM4(A a1, B b2, C c3, B b4) { LastResult = "ABCB"; }
		protected void MM4(A a1, B b2, C c3, C c4) { LastResult = "ABCC"; }
		protected void MM4(A a1, B b2, C c3, D d4) { LastResult = "ABCD"; }
		protected void MM4(A a1, B b2, D d3, A a4) { LastResult = "ABDA"; }
		protected void MM4(A a1, B b2, D d3, B b4) { LastResult = "ABDB"; }
		protected void MM4(A a1, B b2, D d3, C c4) { LastResult = "ABDC"; }
		protected void MM4(A a1, B b2, D d3, D d4) { LastResult = "ABDD"; }
		protected void MM4(A a1, C c2, A a3, A a4) { LastResult = "ACAA"; }
		protected void MM4(A a1, C c2, A a3, B b4) { LastResult = "ACAB"; }
		protected void MM4(A a1, C c2, A a3, C c4) { LastResult = "ACAC"; }
		protected void MM4(A a1, C c2, A a3, D d4) { LastResult = "ACAD"; }
		protected void MM4(A a1, C c2, B b3, A a4) { LastResult = "ACBA"; }
		protected void MM4(A a1, C c2, B b3, B b4) { LastResult = "ACBB"; }
		protected void MM4(A a1, C c2, B b3, C c4) { LastResult = "ACBC"; }
		protected void MM4(A a1, C c2, B b3, D d4) { LastResult = "ACBD"; }
		protected void MM4(A a1, C c2, C c3, A a4) { LastResult = "ACCA"; }
		protected void MM4(A a1, C c2, C c3, B b4) { LastResult = "ACCB"; }
		protected void MM4(A a1, C c2, C c3, C c4) { LastResult = "ACCC"; }
		protected void MM4(A a1, C c2, C c3, D d4) { LastResult = "ACCD"; }
		protected void MM4(A a1, C c2, D d3, A a4) { LastResult = "ACDA"; }
		protected void MM4(A a1, C c2, D d3, B b4) { LastResult = "ACDB"; }
		protected void MM4(A a1, C c2, D d3, C c4) { LastResult = "ACDC"; }
		protected void MM4(A a1, C c2, D d3, D d4) { LastResult = "ACDD"; }
		protected void MM4(A a1, D d2, A a3, A a4) { LastResult = "ADAA"; }
		protected void MM4(A a1, D d2, A a3, B b4) { LastResult = "ADAB"; }
		protected void MM4(A a1, D d2, A a3, C c4) { LastResult = "ADAC"; }
		protected void MM4(A a1, D d2, A a3, D d4) { LastResult = "ADAD"; }
		protected void MM4(A a1, D d2, B b3, A a4) { LastResult = "ADBA"; }
		protected void MM4(A a1, D d2, B b3, B b4) { LastResult = "ADBB"; }
		protected void MM4(A a1, D d2, B b3, C c4) { LastResult = "ADBC"; }
		protected void MM4(A a1, D d2, B b3, D d4) { LastResult = "ADBD"; }
		protected void MM4(A a1, D d2, C c3, A a4) { LastResult = "ADCA"; }
		protected void MM4(A a1, D d2, C c3, B b4) { LastResult = "ADCB"; }
		protected void MM4(A a1, D d2, C c3, C c4) { LastResult = "ADCC"; }
		protected void MM4(A a1, D d2, C c3, D d4) { LastResult = "ADCD"; }
		protected void MM4(A a1, D d2, D d3, A a4) { LastResult = "ADDA"; }
		protected void MM4(A a1, D d2, D d3, B b4) { LastResult = "ADDB"; }
		protected void MM4(A a1, D d2, D d3, C c4) { LastResult = "ADDC"; }
		protected void MM4(A a1, D d2, D d3, D d4) { LastResult = "ADDD"; }
		protected void MM4(B b1, A a2, A a3, A a4) { LastResult = "BAAA"; }
		protected void MM4(B b1, A a2, A a3, B b4) { LastResult = "BAAB"; }
		protected void MM4(B b1, A a2, A a3, C c4) { LastResult = "BAAC"; }
		protected void MM4(B b1, A a2, A a3, D d4) { LastResult = "BAAD"; }
		protected void MM4(B b1, A a2, B b3, A a4) { LastResult = "BABA"; }
		protected void MM4(B b1, A a2, B b3, B b4) { LastResult = "BABB"; }
		protected void MM4(B b1, A a2, B b3, C c4) { LastResult = "BABC"; }
		protected void MM4(B b1, A a2, B b3, D d4) { LastResult = "BABD"; }
		protected void MM4(B b1, A a2, C c3, A a4) { LastResult = "BACA"; }
		protected void MM4(B b1, A a2, C c3, B b4) { LastResult = "BACB"; }
		protected void MM4(B b1, A a2, C c3, C c4) { LastResult = "BACC"; }
		protected void MM4(B b1, A a2, C c3, D d4) { LastResult = "BACD"; }
		protected void MM4(B b1, A a2, D d3, A a4) { LastResult = "BADA"; }
		protected void MM4(B b1, A a2, D d3, B b4) { LastResult = "BADB"; }
		protected void MM4(B b1, A a2, D d3, C c4) { LastResult = "BADC"; }
		protected void MM4(B b1, A a2, D d3, D d4) { LastResult = "BADD"; }
		protected void MM4(B b1, B b2, A a3, A a4) { LastResult = "BBAA"; }
		protected void MM4(B b1, B b2, A a3, B b4) { LastResult = "BBAB"; }
		protected void MM4(B b1, B b2, A a3, C c4) { LastResult = "BBAC"; }
		protected void MM4(B b1, B b2, A a3, D d4) { LastResult = "BBAD"; }
		protected void MM4(B b1, B b2, B b3, A a4) { LastResult = "BBBA"; }
		protected void MM4(B b1, B b2, B b3, B b4) { LastResult = "BBBB"; }
		protected void MM4(B b1, B b2, B b3, C c4) { LastResult = "BBBC"; }
		protected void MM4(B b1, B b2, B b3, D d4) { LastResult = "BBBD"; }
		protected void MM4(B b1, B b2, C c3, A a4) { LastResult = "BBCA"; }
		protected void MM4(B b1, B b2, C c3, B b4) { LastResult = "BBCB"; }
		protected void MM4(B b1, B b2, C c3, C c4) { LastResult = "BBCC"; }
		protected void MM4(B b1, B b2, C c3, D d4) { LastResult = "BBCD"; }
		protected void MM4(B b1, B b2, D d3, A a4) { LastResult = "BBDA"; }
		protected void MM4(B b1, B b2, D d3, B b4) { LastResult = "BBDB"; }
		protected void MM4(B b1, B b2, D d3, C c4) { LastResult = "BBDC"; }
		protected void MM4(B b1, B b2, D d3, D d4) { LastResult = "BBDD"; }
		protected void MM4(B b1, C c2, A a3, A a4) { LastResult = "BCAA"; }
		protected void MM4(B b1, C c2, A a3, B b4) { LastResult = "BCAB"; }
		protected void MM4(B b1, C c2, A a3, C c4) { LastResult = "BCAC"; }
		protected void MM4(B b1, C c2, A a3, D d4) { LastResult = "BCAD"; }
		protected void MM4(B b1, C c2, B b3, A a4) { LastResult = "BCBA"; }
		protected void MM4(B b1, C c2, B b3, B b4) { LastResult = "BCBB"; }
		protected void MM4(B b1, C c2, B b3, C c4) { LastResult = "BCBC"; }
		protected void MM4(B b1, C c2, B b3, D d4) { LastResult = "BCBD"; }
		protected void MM4(B b1, C c2, C c3, A a4) { LastResult = "BCCA"; }
		protected void MM4(B b1, C c2, C c3, B b4) { LastResult = "BCCB"; }
		protected void MM4(B b1, C c2, C c3, C c4) { LastResult = "BCCC"; }
		protected void MM4(B b1, C c2, C c3, D d4) { LastResult = "BCCD"; }
		protected void MM4(B b1, C c2, D d3, A a4) { LastResult = "BCDA"; }
		protected void MM4(B b1, C c2, D d3, B b4) { LastResult = "BCDB"; }
		protected void MM4(B b1, C c2, D d3, C c4) { LastResult = "BCDC"; }
		protected void MM4(B b1, C c2, D d3, D d4) { LastResult = "BCDD"; }
		protected void MM4(B b1, D d2, A a3, A a4) { LastResult = "BDAA"; }
		protected void MM4(B b1, D d2, A a3, B b4) { LastResult = "BDAB"; }
		protected void MM4(B b1, D d2, A a3, C c4) { LastResult = "BDAC"; }
		protected void MM4(B b1, D d2, A a3, D d4) { LastResult = "BDAD"; }
		protected void MM4(B b1, D d2, B b3, A a4) { LastResult = "BDBA"; }
		protected void MM4(B b1, D d2, B b3, B b4) { LastResult = "BDBB"; }
		protected void MM4(B b1, D d2, B b3, C c4) { LastResult = "BDBC"; }
		protected void MM4(B b1, D d2, B b3, D d4) { LastResult = "BDBD"; }
		protected void MM4(B b1, D d2, C c3, A a4) { LastResult = "BDCA"; }
		protected void MM4(B b1, D d2, C c3, B b4) { LastResult = "BDCB"; }
		protected void MM4(B b1, D d2, C c3, C c4) { LastResult = "BDCC"; }
		protected void MM4(B b1, D d2, C c3, D d4) { LastResult = "BDCD"; }
		protected void MM4(B b1, D d2, D d3, A a4) { LastResult = "BDDA"; }
		protected void MM4(B b1, D d2, D d3, B b4) { LastResult = "BDDB"; }
		protected void MM4(B b1, D d2, D d3, C c4) { LastResult = "BDDC"; }
		protected void MM4(B b1, D d2, D d3, D d4) { LastResult = "BDDD"; }
		protected void MM4(C c1, A a2, A a3, A a4) { LastResult = "CAAA"; }
		protected void MM4(C c1, A a2, A a3, B b4) { LastResult = "CAAB"; }
		protected void MM4(C c1, A a2, A a3, C c4) { LastResult = "CAAC"; }
		protected void MM4(C c1, A a2, A a3, D d4) { LastResult = "CAAD"; }
		protected void MM4(C c1, A a2, B b3, A a4) { LastResult = "CABA"; }
		protected void MM4(C c1, A a2, B b3, B b4) { LastResult = "CABB"; }
		protected void MM4(C c1, A a2, B b3, C c4) { LastResult = "CABC"; }
		protected void MM4(C c1, A a2, B b3, D d4) { LastResult = "CABD"; }
		protected void MM4(C c1, A a2, C c3, A a4) { LastResult = "CACA"; }
		protected void MM4(C c1, A a2, C c3, B b4) { LastResult = "CACB"; }
		protected void MM4(C c1, A a2, C c3, C c4) { LastResult = "CACC"; }
		protected void MM4(C c1, A a2, C c3, D d4) { LastResult = "CACD"; }
		protected void MM4(C c1, A a2, D d3, A a4) { LastResult = "CADA"; }
		protected void MM4(C c1, A a2, D d3, B b4) { LastResult = "CADB"; }
		protected void MM4(C c1, A a2, D d3, C c4) { LastResult = "CADC"; }
		protected void MM4(C c1, A a2, D d3, D d4) { LastResult = "CADD"; }
		protected void MM4(C c1, B b2, A a3, A a4) { LastResult = "CBAA"; }
		protected void MM4(C c1, B b2, A a3, B b4) { LastResult = "CBAB"; }
		protected void MM4(C c1, B b2, A a3, C c4) { LastResult = "CBAC"; }
		protected void MM4(C c1, B b2, A a3, D d4) { LastResult = "CBAD"; }
		protected void MM4(C c1, B b2, B b3, A a4) { LastResult = "CBBA"; }
		protected void MM4(C c1, B b2, B b3, B b4) { LastResult = "CBBB"; }
		protected void MM4(C c1, B b2, B b3, C c4) { LastResult = "CBBC"; }
		protected void MM4(C c1, B b2, B b3, D d4) { LastResult = "CBBD"; }
		protected void MM4(C c1, B b2, C c3, A a4) { LastResult = "CBCA"; }
		protected void MM4(C c1, B b2, C c3, B b4) { LastResult = "CBCB"; }
		protected void MM4(C c1, B b2, C c3, C c4) { LastResult = "CBCC"; }
		protected void MM4(C c1, B b2, C c3, D d4) { LastResult = "CBCD"; }
		protected void MM4(C c1, B b2, D d3, A a4) { LastResult = "CBDA"; }
		protected void MM4(C c1, B b2, D d3, B b4) { LastResult = "CBDB"; }
		protected void MM4(C c1, B b2, D d3, C c4) { LastResult = "CBDC"; }
		protected void MM4(C c1, B b2, D d3, D d4) { LastResult = "CBDD"; }
		protected void MM4(C c1, C c2, A a3, A a4) { LastResult = "CCAA"; }
		protected void MM4(C c1, C c2, A a3, B b4) { LastResult = "CCAB"; }
		protected void MM4(C c1, C c2, A a3, C c4) { LastResult = "CCAC"; }
		protected void MM4(C c1, C c2, A a3, D d4) { LastResult = "CCAD"; }
		protected void MM4(C c1, C c2, B b3, A a4) { LastResult = "CCBA"; }
		protected void MM4(C c1, C c2, B b3, B b4) { LastResult = "CCBB"; }
		protected void MM4(C c1, C c2, B b3, C c4) { LastResult = "CCBC"; }
		protected void MM4(C c1, C c2, B b3, D d4) { LastResult = "CCBD"; }
		protected void MM4(C c1, C c2, C c3, A a4) { LastResult = "CCCA"; }
		protected void MM4(C c1, C c2, C c3, B b4) { LastResult = "CCCB"; }
		protected void MM4(C c1, C c2, C c3, C c4) { LastResult = "CCCC"; }
		protected void MM4(C c1, C c2, C c3, D d4) { LastResult = "CCCD"; }
		protected void MM4(C c1, C c2, D d3, A a4) { LastResult = "CCDA"; }
		protected void MM4(C c1, C c2, D d3, B b4) { LastResult = "CCDB"; }
		protected void MM4(C c1, C c2, D d3, C c4) { LastResult = "CCDC"; }
		protected void MM4(C c1, C c2, D d3, D d4) { LastResult = "CCDD"; }
		protected void MM4(C c1, D d2, A a3, A a4) { LastResult = "CDAA"; }
		protected void MM4(C c1, D d2, A a3, B b4) { LastResult = "CDAB"; }
		protected void MM4(C c1, D d2, A a3, C c4) { LastResult = "CDAC"; }
		protected void MM4(C c1, D d2, A a3, D d4) { LastResult = "CDAD"; }
		protected void MM4(C c1, D d2, B b3, A a4) { LastResult = "CDBA"; }
		protected void MM4(C c1, D d2, B b3, B b4) { LastResult = "CDBB"; }
		protected void MM4(C c1, D d2, B b3, C c4) { LastResult = "CDBC"; }
		protected void MM4(C c1, D d2, B b3, D d4) { LastResult = "CDBD"; }
		protected void MM4(C c1, D d2, C c3, A a4) { LastResult = "CDCA"; }
		protected void MM4(C c1, D d2, C c3, B b4) { LastResult = "CDCB"; }
		protected void MM4(C c1, D d2, C c3, C c4) { LastResult = "CDCC"; }
		protected void MM4(C c1, D d2, C c3, D d4) { LastResult = "CDCD"; }
		protected void MM4(C c1, D d2, D d3, A a4) { LastResult = "CDDA"; }
		protected void MM4(C c1, D d2, D d3, B b4) { LastResult = "CDDB"; }
		protected void MM4(C c1, D d2, D d3, C c4) { LastResult = "CDDC"; }
		protected void MM4(C c1, D d2, D d3, D d4) { LastResult = "CDDD"; }
		protected void MM4(D d1, A a2, A a3, A a4) { LastResult = "DAAA"; }
		protected void MM4(D d1, A a2, A a3, B b4) { LastResult = "DAAB"; }
		protected void MM4(D d1, A a2, A a3, C c4) { LastResult = "DAAC"; }
		protected void MM4(D d1, A a2, A a3, D d4) { LastResult = "DAAD"; }
		protected void MM4(D d1, A a2, B b3, A a4) { LastResult = "DABA"; }
		protected void MM4(D d1, A a2, B b3, B b4) { LastResult = "DABB"; }
		protected void MM4(D d1, A a2, B b3, C c4) { LastResult = "DABC"; }
		protected void MM4(D d1, A a2, B b3, D d4) { LastResult = "DABD"; }
		protected void MM4(D d1, A a2, C c3, A a4) { LastResult = "DACA"; }
		protected void MM4(D d1, A a2, C c3, B b4) { LastResult = "DACB"; }
		protected void MM4(D d1, A a2, C c3, C c4) { LastResult = "DACC"; }
		protected void MM4(D d1, A a2, C c3, D d4) { LastResult = "DACD"; }
		protected void MM4(D d1, A a2, D d3, A a4) { LastResult = "DADA"; }
		protected void MM4(D d1, A a2, D d3, B b4) { LastResult = "DADB"; }
		protected void MM4(D d1, A a2, D d3, C c4) { LastResult = "DADC"; }
		protected void MM4(D d1, A a2, D d3, D d4) { LastResult = "DADD"; }
		protected void MM4(D d1, B b2, A a3, A a4) { LastResult = "DBAA"; }
		protected void MM4(D d1, B b2, A a3, B b4) { LastResult = "DBAB"; }
		protected void MM4(D d1, B b2, A a3, C c4) { LastResult = "DBAC"; }
		protected void MM4(D d1, B b2, A a3, D d4) { LastResult = "DBAD"; }
		protected void MM4(D d1, B b2, B b3, A a4) { LastResult = "DBBA"; }
		protected void MM4(D d1, B b2, B b3, B b4) { LastResult = "DBBB"; }
		protected void MM4(D d1, B b2, B b3, C c4) { LastResult = "DBBC"; }
		protected void MM4(D d1, B b2, B b3, D d4) { LastResult = "DBBD"; }
		protected void MM4(D d1, B b2, C c3, A a4) { LastResult = "DBCA"; }
		protected void MM4(D d1, B b2, C c3, B b4) { LastResult = "DBCB"; }
		protected void MM4(D d1, B b2, C c3, C c4) { LastResult = "DBCC"; }
		protected void MM4(D d1, B b2, C c3, D d4) { LastResult = "DBCD"; }
		protected void MM4(D d1, B b2, D d3, A a4) { LastResult = "DBDA"; }
		protected void MM4(D d1, B b2, D d3, B b4) { LastResult = "DBDB"; }
		protected void MM4(D d1, B b2, D d3, C c4) { LastResult = "DBDC"; }
		protected void MM4(D d1, B b2, D d3, D d4) { LastResult = "DBDD"; }
		protected void MM4(D d1, C c2, A a3, A a4) { LastResult = "DCAA"; }
		protected void MM4(D d1, C c2, A a3, B b4) { LastResult = "DCAB"; }
		protected void MM4(D d1, C c2, A a3, C c4) { LastResult = "DCAC"; }
		protected void MM4(D d1, C c2, A a3, D d4) { LastResult = "DCAD"; }
		protected void MM4(D d1, C c2, B b3, A a4) { LastResult = "DCBA"; }
		protected void MM4(D d1, C c2, B b3, B b4) { LastResult = "DCBB"; }
		protected void MM4(D d1, C c2, B b3, C c4) { LastResult = "DCBC"; }
		protected void MM4(D d1, C c2, B b3, D d4) { LastResult = "DCBD"; }
		protected void MM4(D d1, C c2, C c3, A a4) { LastResult = "DCCA"; }
		protected void MM4(D d1, C c2, C c3, B b4) { LastResult = "DCCB"; }
		protected void MM4(D d1, C c2, C c3, C c4) { LastResult = "DCCC"; }
		protected void MM4(D d1, C c2, C c3, D d4) { LastResult = "DCCD"; }
		protected void MM4(D d1, C c2, D d3, A a4) { LastResult = "DCDA"; }
		protected void MM4(D d1, C c2, D d3, B b4) { LastResult = "DCDB"; }
		protected void MM4(D d1, C c2, D d3, C c4) { LastResult = "DCDC"; }
		protected void MM4(D d1, C c2, D d3, D d4) { LastResult = "DCDD"; }
		protected void MM4(D d1, D d2, A a3, A a4) { LastResult = "DDAA"; }
		protected void MM4(D d1, D d2, A a3, B b4) { LastResult = "DDAB"; }
		protected void MM4(D d1, D d2, A a3, C c4) { LastResult = "DDAC"; }
		protected void MM4(D d1, D d2, A a3, D d4) { LastResult = "DDAD"; }
		protected void MM4(D d1, D d2, B b3, A a4) { LastResult = "DDBA"; }
		protected void MM4(D d1, D d2, B b3, B b4) { LastResult = "DDBB"; }
		protected void MM4(D d1, D d2, B b3, C c4) { LastResult = "DDBC"; }
		protected void MM4(D d1, D d2, B b3, D d4) { LastResult = "DDBD"; }
		protected void MM4(D d1, D d2, C c3, A a4) { LastResult = "DDCA"; }
		protected void MM4(D d1, D d2, C c3, B b4) { LastResult = "DDCB"; }
		protected void MM4(D d1, D d2, C c3, C c4) { LastResult = "DDCC"; }
		protected void MM4(D d1, D d2, C c3, D d4) { LastResult = "DDCD"; }
		protected void MM4(D d1, D d2, D d3, A a4) { LastResult = "DDDA"; }
		protected void MM4(D d1, D d2, D d3, B b4) { LastResult = "DDDB"; }
		protected void MM4(D d1, D d2, D d3, C c4) { LastResult = "DDDC"; }
		protected void MM4(D d1, D d2, D d3, D d4) { LastResult = "DDDD"; }

		protected string FF1(B b1) { return "B"; }
		protected string FF1(C c1) { return "C"; }
		protected string FF1(D d1) { return "D"; }
		protected string FF2(A a1, B b2) { return "AB"; }
		protected string FF2(A a1, C c2) { return "AC"; }
		protected string FF2(A a1, D d2) { return "AD"; }
		protected string FF2(B b1, A a2) { return "BA"; }
		protected string FF2(B b1, B b2) { return "BB"; }
		protected string FF2(B b1, C c2) { return "BC"; }
		protected string FF2(B b1, D d2) { return "BD"; }
		protected string FF2(C c1, A a2) { return "CA"; }
		protected string FF2(C c1, B b2) { return "CB"; }
		protected string FF2(C c1, C c2) { return "CC"; }
		protected string FF2(C c1, D d2) { return "CD"; }
		protected string FF2(D d1, A a2) { return "DA"; }
		protected string FF2(D d1, B b2) { return "DB"; }
		protected string FF2(D d1, C c2) { return "DC"; }
		protected string FF2(D d1, D d2) { return "DD"; }
		protected string FF3(A a1, A a2, B b3) { return "AAB"; }
		protected string FF3(A a1, A a2, C c3) { return "AAC"; }
		protected string FF3(A a1, A a2, D d3) { return "AAD"; }
		protected string FF3(A a1, B b2, A a3) { return "ABA"; }
		protected string FF3(A a1, B b2, B b3) { return "ABB"; }
		protected string FF3(A a1, B b2, C c3) { return "ABC"; }
		protected string FF3(A a1, B b2, D d3) { return "ABD"; }
		protected string FF3(A a1, C c2, A a3) { return "ACA"; }
		protected string FF3(A a1, C c2, B b3) { return "ACB"; }
		protected string FF3(A a1, C c2, C c3) { return "ACC"; }
		protected string FF3(A a1, C c2, D d3) { return "ACD"; }
		protected string FF3(A a1, D d2, A a3) { return "ADA"; }
		protected string FF3(A a1, D d2, B b3) { return "ADB"; }
		protected string FF3(A a1, D d2, C c3) { return "ADC"; }
		protected string FF3(A a1, D d2, D d3) { return "ADD"; }
		protected string FF3(B b1, A a2, A a3) { return "BAA"; }
		protected string FF3(B b1, A a2, B b3) { return "BAB"; }
		protected string FF3(B b1, A a2, C c3) { return "BAC"; }
		protected string FF3(B b1, A a2, D d3) { return "BAD"; }
		protected string FF3(B b1, B b2, A a3) { return "BBA"; }
		protected string FF3(B b1, B b2, B b3) { return "BBB"; }
		protected string FF3(B b1, B b2, C c3) { return "BBC"; }
		protected string FF3(B b1, B b2, D d3) { return "BBD"; }
		protected string FF3(B b1, C c2, A a3) { return "BCA"; }
		protected string FF3(B b1, C c2, B b3) { return "BCB"; }
		protected string FF3(B b1, C c2, C c3) { return "BCC"; }
		protected string FF3(B b1, C c2, D d3) { return "BCD"; }
		protected string FF3(B b1, D d2, A a3) { return "BDA"; }
		protected string FF3(B b1, D d2, B b3) { return "BDB"; }
		protected string FF3(B b1, D d2, C c3) { return "BDC"; }
		protected string FF3(B b1, D d2, D d3) { return "BDD"; }
		protected string FF3(C c1, A a2, A a3) { return "CAA"; }
		protected string FF3(C c1, A a2, B b3) { return "CAB"; }
		protected string FF3(C c1, A a2, C c3) { return "CAC"; }
		protected string FF3(C c1, A a2, D d3) { return "CAD"; }
		protected string FF3(C c1, B b2, A a3) { return "CBA"; }
		protected string FF3(C c1, B b2, B b3) { return "CBB"; }
		protected string FF3(C c1, B b2, C c3) { return "CBC"; }
		protected string FF3(C c1, B b2, D d3) { return "CBD"; }
		protected string FF3(C c1, C c2, A a3) { return "CCA"; }
		protected string FF3(C c1, C c2, B b3) { return "CCB"; }
		protected string FF3(C c1, C c2, C c3) { return "CCC"; }
		protected string FF3(C c1, C c2, D d3) { return "CCD"; }
		protected string FF3(C c1, D d2, A a3) { return "CDA"; }
		protected string FF3(C c1, D d2, B b3) { return "CDB"; }
		protected string FF3(C c1, D d2, C c3) { return "CDC"; }
		protected string FF3(C c1, D d2, D d3) { return "CDD"; }
		protected string FF3(D d1, A a2, A a3) { return "DAA"; }
		protected string FF3(D d1, A a2, B b3) { return "DAB"; }
		protected string FF3(D d1, A a2, C c3) { return "DAC"; }
		protected string FF3(D d1, A a2, D d3) { return "DAD"; }
		protected string FF3(D d1, B b2, A a3) { return "DBA"; }
		protected string FF3(D d1, B b2, B b3) { return "DBB"; }
		protected string FF3(D d1, B b2, C c3) { return "DBC"; }
		protected string FF3(D d1, B b2, D d3) { return "DBD"; }
		protected string FF3(D d1, C c2, A a3) { return "DCA"; }
		protected string FF3(D d1, C c2, B b3) { return "DCB"; }
		protected string FF3(D d1, C c2, C c3) { return "DCC"; }
		protected string FF3(D d1, C c2, D d3) { return "DCD"; }
		protected string FF3(D d1, D d2, A a3) { return "DDA"; }
		protected string FF3(D d1, D d2, B b3) { return "DDB"; }
		protected string FF3(D d1, D d2, C c3) { return "DDC"; }
		protected string FF3(D d1, D d2, D d3) { return "DDD"; }
		protected string FF4(A a1, A a2, A a3, B b4) { return "AAAB"; }
		protected string FF4(A a1, A a2, A a3, C c4) { return "AAAC"; }
		protected string FF4(A a1, A a2, A a3, D d4) { return "AAAD"; }
		protected string FF4(A a1, A a2, B b3, A a4) { return "AABA"; }
		protected string FF4(A a1, A a2, B b3, B b4) { return "AABB"; }
		protected string FF4(A a1, A a2, B b3, C c4) { return "AABC"; }
		protected string FF4(A a1, A a2, B b3, D d4) { return "AABD"; }
		protected string FF4(A a1, A a2, C c3, A a4) { return "AACA"; }
		protected string FF4(A a1, A a2, C c3, B b4) { return "AACB"; }
		protected string FF4(A a1, A a2, C c3, C c4) { return "AACC"; }
		protected string FF4(A a1, A a2, C c3, D d4) { return "AACD"; }
		protected string FF4(A a1, A a2, D d3, A a4) { return "AADA"; }
		protected string FF4(A a1, A a2, D d3, B b4) { return "AADB"; }
		protected string FF4(A a1, A a2, D d3, C c4) { return "AADC"; }
		protected string FF4(A a1, A a2, D d3, D d4) { return "AADD"; }
		protected string FF4(A a1, B b2, A a3, A a4) { return "ABAA"; }
		protected string FF4(A a1, B b2, A a3, B b4) { return "ABAB"; }
		protected string FF4(A a1, B b2, A a3, C c4) { return "ABAC"; }
		protected string FF4(A a1, B b2, A a3, D d4) { return "ABAD"; }
		protected string FF4(A a1, B b2, B b3, A a4) { return "ABBA"; }
		protected string FF4(A a1, B b2, B b3, B b4) { return "ABBB"; }
		protected string FF4(A a1, B b2, B b3, C c4) { return "ABBC"; }
		protected string FF4(A a1, B b2, B b3, D d4) { return "ABBD"; }
		protected string FF4(A a1, B b2, C c3, A a4) { return "ABCA"; }
		protected string FF4(A a1, B b2, C c3, B b4) { return "ABCB"; }
		protected string FF4(A a1, B b2, C c3, C c4) { return "ABCC"; }
		protected string FF4(A a1, B b2, C c3, D d4) { return "ABCD"; }
		protected string FF4(A a1, B b2, D d3, A a4) { return "ABDA"; }
		protected string FF4(A a1, B b2, D d3, B b4) { return "ABDB"; }
		protected string FF4(A a1, B b2, D d3, C c4) { return "ABDC"; }
		protected string FF4(A a1, B b2, D d3, D d4) { return "ABDD"; }
		protected string FF4(A a1, C c2, A a3, A a4) { return "ACAA"; }
		protected string FF4(A a1, C c2, A a3, B b4) { return "ACAB"; }
		protected string FF4(A a1, C c2, A a3, C c4) { return "ACAC"; }
		protected string FF4(A a1, C c2, A a3, D d4) { return "ACAD"; }
		protected string FF4(A a1, C c2, B b3, A a4) { return "ACBA"; }
		protected string FF4(A a1, C c2, B b3, B b4) { return "ACBB"; }
		protected string FF4(A a1, C c2, B b3, C c4) { return "ACBC"; }
		protected string FF4(A a1, C c2, B b3, D d4) { return "ACBD"; }
		protected string FF4(A a1, C c2, C c3, A a4) { return "ACCA"; }
		protected string FF4(A a1, C c2, C c3, B b4) { return "ACCB"; }
		protected string FF4(A a1, C c2, C c3, C c4) { return "ACCC"; }
		protected string FF4(A a1, C c2, C c3, D d4) { return "ACCD"; }
		protected string FF4(A a1, C c2, D d3, A a4) { return "ACDA"; }
		protected string FF4(A a1, C c2, D d3, B b4) { return "ACDB"; }
		protected string FF4(A a1, C c2, D d3, C c4) { return "ACDC"; }
		protected string FF4(A a1, C c2, D d3, D d4) { return "ACDD"; }
		protected string FF4(A a1, D d2, A a3, A a4) { return "ADAA"; }
		protected string FF4(A a1, D d2, A a3, B b4) { return "ADAB"; }
		protected string FF4(A a1, D d2, A a3, C c4) { return "ADAC"; }
		protected string FF4(A a1, D d2, A a3, D d4) { return "ADAD"; }
		protected string FF4(A a1, D d2, B b3, A a4) { return "ADBA"; }
		protected string FF4(A a1, D d2, B b3, B b4) { return "ADBB"; }
		protected string FF4(A a1, D d2, B b3, C c4) { return "ADBC"; }
		protected string FF4(A a1, D d2, B b3, D d4) { return "ADBD"; }
		protected string FF4(A a1, D d2, C c3, A a4) { return "ADCA"; }
		protected string FF4(A a1, D d2, C c3, B b4) { return "ADCB"; }
		protected string FF4(A a1, D d2, C c3, C c4) { return "ADCC"; }
		protected string FF4(A a1, D d2, C c3, D d4) { return "ADCD"; }
		protected string FF4(A a1, D d2, D d3, A a4) { return "ADDA"; }
		protected string FF4(A a1, D d2, D d3, B b4) { return "ADDB"; }
		protected string FF4(A a1, D d2, D d3, C c4) { return "ADDC"; }
		protected string FF4(A a1, D d2, D d3, D d4) { return "ADDD"; }
		protected string FF4(B b1, A a2, A a3, A a4) { return "BAAA"; }
		protected string FF4(B b1, A a2, A a3, B b4) { return "BAAB"; }
		protected string FF4(B b1, A a2, A a3, C c4) { return "BAAC"; }
		protected string FF4(B b1, A a2, A a3, D d4) { return "BAAD"; }
		protected string FF4(B b1, A a2, B b3, A a4) { return "BABA"; }
		protected string FF4(B b1, A a2, B b3, B b4) { return "BABB"; }
		protected string FF4(B b1, A a2, B b3, C c4) { return "BABC"; }
		protected string FF4(B b1, A a2, B b3, D d4) { return "BABD"; }
		protected string FF4(B b1, A a2, C c3, A a4) { return "BACA"; }
		protected string FF4(B b1, A a2, C c3, B b4) { return "BACB"; }
		protected string FF4(B b1, A a2, C c3, C c4) { return "BACC"; }
		protected string FF4(B b1, A a2, C c3, D d4) { return "BACD"; }
		protected string FF4(B b1, A a2, D d3, A a4) { return "BADA"; }
		protected string FF4(B b1, A a2, D d3, B b4) { return "BADB"; }
		protected string FF4(B b1, A a2, D d3, C c4) { return "BADC"; }
		protected string FF4(B b1, A a2, D d3, D d4) { return "BADD"; }
		protected string FF4(B b1, B b2, A a3, A a4) { return "BBAA"; }
		protected string FF4(B b1, B b2, A a3, B b4) { return "BBAB"; }
		protected string FF4(B b1, B b2, A a3, C c4) { return "BBAC"; }
		protected string FF4(B b1, B b2, A a3, D d4) { return "BBAD"; }
		protected string FF4(B b1, B b2, B b3, A a4) { return "BBBA"; }
		protected string FF4(B b1, B b2, B b3, B b4) { return "BBBB"; }
		protected string FF4(B b1, B b2, B b3, C c4) { return "BBBC"; }
		protected string FF4(B b1, B b2, B b3, D d4) { return "BBBD"; }
		protected string FF4(B b1, B b2, C c3, A a4) { return "BBCA"; }
		protected string FF4(B b1, B b2, C c3, B b4) { return "BBCB"; }
		protected string FF4(B b1, B b2, C c3, C c4) { return "BBCC"; }
		protected string FF4(B b1, B b2, C c3, D d4) { return "BBCD"; }
		protected string FF4(B b1, B b2, D d3, A a4) { return "BBDA"; }
		protected string FF4(B b1, B b2, D d3, B b4) { return "BBDB"; }
		protected string FF4(B b1, B b2, D d3, C c4) { return "BBDC"; }
		protected string FF4(B b1, B b2, D d3, D d4) { return "BBDD"; }
		protected string FF4(B b1, C c2, A a3, A a4) { return "BCAA"; }
		protected string FF4(B b1, C c2, A a3, B b4) { return "BCAB"; }
		protected string FF4(B b1, C c2, A a3, C c4) { return "BCAC"; }
		protected string FF4(B b1, C c2, A a3, D d4) { return "BCAD"; }
		protected string FF4(B b1, C c2, B b3, A a4) { return "BCBA"; }
		protected string FF4(B b1, C c2, B b3, B b4) { return "BCBB"; }
		protected string FF4(B b1, C c2, B b3, C c4) { return "BCBC"; }
		protected string FF4(B b1, C c2, B b3, D d4) { return "BCBD"; }
		protected string FF4(B b1, C c2, C c3, A a4) { return "BCCA"; }
		protected string FF4(B b1, C c2, C c3, B b4) { return "BCCB"; }
		protected string FF4(B b1, C c2, C c3, C c4) { return "BCCC"; }
		protected string FF4(B b1, C c2, C c3, D d4) { return "BCCD"; }
		protected string FF4(B b1, C c2, D d3, A a4) { return "BCDA"; }
		protected string FF4(B b1, C c2, D d3, B b4) { return "BCDB"; }
		protected string FF4(B b1, C c2, D d3, C c4) { return "BCDC"; }
		protected string FF4(B b1, C c2, D d3, D d4) { return "BCDD"; }
		protected string FF4(B b1, D d2, A a3, A a4) { return "BDAA"; }
		protected string FF4(B b1, D d2, A a3, B b4) { return "BDAB"; }
		protected string FF4(B b1, D d2, A a3, C c4) { return "BDAC"; }
		protected string FF4(B b1, D d2, A a3, D d4) { return "BDAD"; }
		protected string FF4(B b1, D d2, B b3, A a4) { return "BDBA"; }
		protected string FF4(B b1, D d2, B b3, B b4) { return "BDBB"; }
		protected string FF4(B b1, D d2, B b3, C c4) { return "BDBC"; }
		protected string FF4(B b1, D d2, B b3, D d4) { return "BDBD"; }
		protected string FF4(B b1, D d2, C c3, A a4) { return "BDCA"; }
		protected string FF4(B b1, D d2, C c3, B b4) { return "BDCB"; }
		protected string FF4(B b1, D d2, C c3, C c4) { return "BDCC"; }
		protected string FF4(B b1, D d2, C c3, D d4) { return "BDCD"; }
		protected string FF4(B b1, D d2, D d3, A a4) { return "BDDA"; }
		protected string FF4(B b1, D d2, D d3, B b4) { return "BDDB"; }
		protected string FF4(B b1, D d2, D d3, C c4) { return "BDDC"; }
		protected string FF4(B b1, D d2, D d3, D d4) { return "BDDD"; }
		protected string FF4(C c1, A a2, A a3, A a4) { return "CAAA"; }
		protected string FF4(C c1, A a2, A a3, B b4) { return "CAAB"; }
		protected string FF4(C c1, A a2, A a3, C c4) { return "CAAC"; }
		protected string FF4(C c1, A a2, A a3, D d4) { return "CAAD"; }
		protected string FF4(C c1, A a2, B b3, A a4) { return "CABA"; }
		protected string FF4(C c1, A a2, B b3, B b4) { return "CABB"; }
		protected string FF4(C c1, A a2, B b3, C c4) { return "CABC"; }
		protected string FF4(C c1, A a2, B b3, D d4) { return "CABD"; }
		protected string FF4(C c1, A a2, C c3, A a4) { return "CACA"; }
		protected string FF4(C c1, A a2, C c3, B b4) { return "CACB"; }
		protected string FF4(C c1, A a2, C c3, C c4) { return "CACC"; }
		protected string FF4(C c1, A a2, C c3, D d4) { return "CACD"; }
		protected string FF4(C c1, A a2, D d3, A a4) { return "CADA"; }
		protected string FF4(C c1, A a2, D d3, B b4) { return "CADB"; }
		protected string FF4(C c1, A a2, D d3, C c4) { return "CADC"; }
		protected string FF4(C c1, A a2, D d3, D d4) { return "CADD"; }
		protected string FF4(C c1, B b2, A a3, A a4) { return "CBAA"; }
		protected string FF4(C c1, B b2, A a3, B b4) { return "CBAB"; }
		protected string FF4(C c1, B b2, A a3, C c4) { return "CBAC"; }
		protected string FF4(C c1, B b2, A a3, D d4) { return "CBAD"; }
		protected string FF4(C c1, B b2, B b3, A a4) { return "CBBA"; }
		protected string FF4(C c1, B b2, B b3, B b4) { return "CBBB"; }
		protected string FF4(C c1, B b2, B b3, C c4) { return "CBBC"; }
		protected string FF4(C c1, B b2, B b3, D d4) { return "CBBD"; }
		protected string FF4(C c1, B b2, C c3, A a4) { return "CBCA"; }
		protected string FF4(C c1, B b2, C c3, B b4) { return "CBCB"; }
		protected string FF4(C c1, B b2, C c3, C c4) { return "CBCC"; }
		protected string FF4(C c1, B b2, C c3, D d4) { return "CBCD"; }
		protected string FF4(C c1, B b2, D d3, A a4) { return "CBDA"; }
		protected string FF4(C c1, B b2, D d3, B b4) { return "CBDB"; }
		protected string FF4(C c1, B b2, D d3, C c4) { return "CBDC"; }
		protected string FF4(C c1, B b2, D d3, D d4) { return "CBDD"; }
		protected string FF4(C c1, C c2, A a3, A a4) { return "CCAA"; }
		protected string FF4(C c1, C c2, A a3, B b4) { return "CCAB"; }
		protected string FF4(C c1, C c2, A a3, C c4) { return "CCAC"; }
		protected string FF4(C c1, C c2, A a3, D d4) { return "CCAD"; }
		protected string FF4(C c1, C c2, B b3, A a4) { return "CCBA"; }
		protected string FF4(C c1, C c2, B b3, B b4) { return "CCBB"; }
		protected string FF4(C c1, C c2, B b3, C c4) { return "CCBC"; }
		protected string FF4(C c1, C c2, B b3, D d4) { return "CCBD"; }
		protected string FF4(C c1, C c2, C c3, A a4) { return "CCCA"; }
		protected string FF4(C c1, C c2, C c3, B b4) { return "CCCB"; }
		protected string FF4(C c1, C c2, C c3, C c4) { return "CCCC"; }
		protected string FF4(C c1, C c2, C c3, D d4) { return "CCCD"; }
		protected string FF4(C c1, C c2, D d3, A a4) { return "CCDA"; }
		protected string FF4(C c1, C c2, D d3, B b4) { return "CCDB"; }
		protected string FF4(C c1, C c2, D d3, C c4) { return "CCDC"; }
		protected string FF4(C c1, C c2, D d3, D d4) { return "CCDD"; }
		protected string FF4(C c1, D d2, A a3, A a4) { return "CDAA"; }
		protected string FF4(C c1, D d2, A a3, B b4) { return "CDAB"; }
		protected string FF4(C c1, D d2, A a3, C c4) { return "CDAC"; }
		protected string FF4(C c1, D d2, A a3, D d4) { return "CDAD"; }
		protected string FF4(C c1, D d2, B b3, A a4) { return "CDBA"; }
		protected string FF4(C c1, D d2, B b3, B b4) { return "CDBB"; }
		protected string FF4(C c1, D d2, B b3, C c4) { return "CDBC"; }
		protected string FF4(C c1, D d2, B b3, D d4) { return "CDBD"; }
		protected string FF4(C c1, D d2, C c3, A a4) { return "CDCA"; }
		protected string FF4(C c1, D d2, C c3, B b4) { return "CDCB"; }
		protected string FF4(C c1, D d2, C c3, C c4) { return "CDCC"; }
		protected string FF4(C c1, D d2, C c3, D d4) { return "CDCD"; }
		protected string FF4(C c1, D d2, D d3, A a4) { return "CDDA"; }
		protected string FF4(C c1, D d2, D d3, B b4) { return "CDDB"; }
		protected string FF4(C c1, D d2, D d3, C c4) { return "CDDC"; }
		protected string FF4(C c1, D d2, D d3, D d4) { return "CDDD"; }
		protected string FF4(D d1, A a2, A a3, A a4) { return "DAAA"; }
		protected string FF4(D d1, A a2, A a3, B b4) { return "DAAB"; }
		protected string FF4(D d1, A a2, A a3, C c4) { return "DAAC"; }
		protected string FF4(D d1, A a2, A a3, D d4) { return "DAAD"; }
		protected string FF4(D d1, A a2, B b3, A a4) { return "DABA"; }
		protected string FF4(D d1, A a2, B b3, B b4) { return "DABB"; }
		protected string FF4(D d1, A a2, B b3, C c4) { return "DABC"; }
		protected string FF4(D d1, A a2, B b3, D d4) { return "DABD"; }
		protected string FF4(D d1, A a2, C c3, A a4) { return "DACA"; }
		protected string FF4(D d1, A a2, C c3, B b4) { return "DACB"; }
		protected string FF4(D d1, A a2, C c3, C c4) { return "DACC"; }
		protected string FF4(D d1, A a2, C c3, D d4) { return "DACD"; }
		protected string FF4(D d1, A a2, D d3, A a4) { return "DADA"; }
		protected string FF4(D d1, A a2, D d3, B b4) { return "DADB"; }
		protected string FF4(D d1, A a2, D d3, C c4) { return "DADC"; }
		protected string FF4(D d1, A a2, D d3, D d4) { return "DADD"; }
		protected string FF4(D d1, B b2, A a3, A a4) { return "DBAA"; }
		protected string FF4(D d1, B b2, A a3, B b4) { return "DBAB"; }
		protected string FF4(D d1, B b2, A a3, C c4) { return "DBAC"; }
		protected string FF4(D d1, B b2, A a3, D d4) { return "DBAD"; }
		protected string FF4(D d1, B b2, B b3, A a4) { return "DBBA"; }
		protected string FF4(D d1, B b2, B b3, B b4) { return "DBBB"; }
		protected string FF4(D d1, B b2, B b3, C c4) { return "DBBC"; }
		protected string FF4(D d1, B b2, B b3, D d4) { return "DBBD"; }
		protected string FF4(D d1, B b2, C c3, A a4) { return "DBCA"; }
		protected string FF4(D d1, B b2, C c3, B b4) { return "DBCB"; }
		protected string FF4(D d1, B b2, C c3, C c4) { return "DBCC"; }
		protected string FF4(D d1, B b2, C c3, D d4) { return "DBCD"; }
		protected string FF4(D d1, B b2, D d3, A a4) { return "DBDA"; }
		protected string FF4(D d1, B b2, D d3, B b4) { return "DBDB"; }
		protected string FF4(D d1, B b2, D d3, C c4) { return "DBDC"; }
		protected string FF4(D d1, B b2, D d3, D d4) { return "DBDD"; }
		protected string FF4(D d1, C c2, A a3, A a4) { return "DCAA"; }
		protected string FF4(D d1, C c2, A a3, B b4) { return "DCAB"; }
		protected string FF4(D d1, C c2, A a3, C c4) { return "DCAC"; }
		protected string FF4(D d1, C c2, A a3, D d4) { return "DCAD"; }
		protected string FF4(D d1, C c2, B b3, A a4) { return "DCBA"; }
		protected string FF4(D d1, C c2, B b3, B b4) { return "DCBB"; }
		protected string FF4(D d1, C c2, B b3, C c4) { return "DCBC"; }
		protected string FF4(D d1, C c2, B b3, D d4) { return "DCBD"; }
		protected string FF4(D d1, C c2, C c3, A a4) { return "DCCA"; }
		protected string FF4(D d1, C c2, C c3, B b4) { return "DCCB"; }
		protected string FF4(D d1, C c2, C c3, C c4) { return "DCCC"; }
		protected string FF4(D d1, C c2, C c3, D d4) { return "DCCD"; }
		protected string FF4(D d1, C c2, D d3, A a4) { return "DCDA"; }
		protected string FF4(D d1, C c2, D d3, B b4) { return "DCDB"; }
		protected string FF4(D d1, C c2, D d3, C c4) { return "DCDC"; }
		protected string FF4(D d1, C c2, D d3, D d4) { return "DCDD"; }
		protected string FF4(D d1, D d2, A a3, A a4) { return "DDAA"; }
		protected string FF4(D d1, D d2, A a3, B b4) { return "DDAB"; }
		protected string FF4(D d1, D d2, A a3, C c4) { return "DDAC"; }
		protected string FF4(D d1, D d2, A a3, D d4) { return "DDAD"; }
		protected string FF4(D d1, D d2, B b3, A a4) { return "DDBA"; }
		protected string FF4(D d1, D d2, B b3, B b4) { return "DDBB"; }
		protected string FF4(D d1, D d2, B b3, C c4) { return "DDBC"; }
		protected string FF4(D d1, D d2, B b3, D d4) { return "DDBD"; }
		protected string FF4(D d1, D d2, C c3, A a4) { return "DDCA"; }
		protected string FF4(D d1, D d2, C c3, B b4) { return "DDCB"; }
		protected string FF4(D d1, D d2, C c3, C c4) { return "DDCC"; }
		protected string FF4(D d1, D d2, C c3, D d4) { return "DDCD"; }
		protected string FF4(D d1, D d2, D d3, A a4) { return "DDDA"; }
		protected string FF4(D d1, D d2, D d3, B b4) { return "DDDB"; }
		protected string FF4(D d1, D d2, D d3, C c4) { return "DDDC"; }
		protected string FF4(D d1, D d2, D d3, D d4) { return "DDDD"; }
		// end all other combinations
	}
}
