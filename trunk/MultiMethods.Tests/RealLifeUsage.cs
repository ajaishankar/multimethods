using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace MultiMethods.Tests
{
	public interface IPayment
	{
		decimal Amount { get; set; }
	}

	public class ACHPayment : IPayment
	{
		public decimal Amount { get; set; }
	}

	public class CreditCardPayment : IPayment
	{
		public decimal Amount { get; set; }
		public string CardNumber { get; set; }
		public string Bin { get; set; }
	}

	public interface IBillingRule { }

	public class RejectBinRule : IBillingRule
	{
		string[] BinsToReject { get; set; }
	}

	public class FreeTrialRule : IBillingRule
	{
		public int TrialDays { get; set; }
	}

	public class PaymentService
	{
		MultiMethod.Action<IPayment, IBillingRule> _authorize;

		public PaymentService()
		{
			_authorize = Dispatcher.Action<IPayment, IBillingRule>(this.Authorize);
		}

		public void Authorize(IPayment payment, IBillingRule rule)
		{
			_authorize(payment, rule); // multi method dispatch
		}
	}

	public class DefaultPaymentService : PaymentService
	{
		public bool Authorize_Payment_FreeTrialRule_Called { get; private set; }
		public bool Authorize_CreditCardPayment_RejectBinRule_Called { get; private set; }

		// free trial applies to all payment types
		protected void Authorize(IPayment payment, FreeTrialRule rule)
		{
			this.Authorize_Payment_FreeTrialRule_Called = true;
		}
		
		// reject bin rule applies to credit cards
		protected void Authorize(CreditCardPayment payment, RejectBinRule rule)
		{
			this.Authorize_CreditCardPayment_RejectBinRule_Called = true;
		}
	}

	[TestFixture]
	public class RealLifeUsageTest
	{
		PaymentService paymentService;

		[SetUp]
		public void Setup()
		{
			paymentService = new DefaultPaymentService();
		}

		[Test]
		public void Authorize_ACH_With_FreeTrial()
		{
			paymentService.Authorize(new ACHPayment(), new FreeTrialRule());
			Assert.IsTrue((paymentService as DefaultPaymentService)
				.Authorize_Payment_FreeTrialRule_Called);
		}

		[Test]
		public void Authorize_CreditCard_With_FreeTrial()
		{
			paymentService.Authorize(new CreditCardPayment(), new FreeTrialRule());
			Assert.IsTrue((paymentService as DefaultPaymentService)
				.Authorize_Payment_FreeTrialRule_Called);
		}

		[Test]
		public void Authorize_ACH_With_RejectBinRule()
		{
			paymentService.Authorize(new ACHPayment(), new RejectBinRule());
			Assert.IsFalse((paymentService as DefaultPaymentService)
				.Authorize_CreditCardPayment_RejectBinRule_Called);
		}

		[Test]
		public void Authorize_CreditCard_With_RejectBinRule()
		{
			paymentService.Authorize(new CreditCardPayment(), new RejectBinRule());
			Assert.IsTrue((paymentService as DefaultPaymentService)
				.Authorize_CreditCardPayment_RejectBinRule_Called);
		}
	}
}
