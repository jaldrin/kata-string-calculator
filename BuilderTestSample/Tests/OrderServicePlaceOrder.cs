using BuilderTestSample.Exceptions;
using BuilderTestSample.Model;
using BuilderTestSample.Services;
using BuilderTestSample.Tests.TestBuilder;
using System;
using Xunit;
using Xunit.Abstractions;

namespace BuilderTestSample.Tests
{
    public class OrderServicePlaceOrder
    {
        #region Constructor
        private readonly OrderService _orderService = new();
        private readonly OrderBuilder _orderBuilder = new();
        private readonly ITestOutputHelper _logger;
        public OrderServicePlaceOrder(ITestOutputHelper logger) 
            => _logger = logger;
        #endregion

        public enum TestCase
        {
            UseNull,
            UseObjectValidId,
            UseObjectInvalidId
        }

        [Theory]
        [InlineData(-1, false, "Negative ID")]
        [InlineData(0, true, "Valid ID")]
        [InlineData(1, false, "Positive ID")]
        public void OrderIdMustBeZero(int id, bool passFail, string description)
        {
            var order = _orderBuilder.WithTestValues()
                                     .Id(id)
                                     .Customer(BuildTestCustomer(TestCase.UseObjectValidId))
                                     .Build();
            RunOrderTest(passFail, description, order);
        }

        [Theory]
        [InlineData(-1, false, "Negative Amount")]
        [InlineData(0, false, "Edge Case Amount")]
        [InlineData(1, true, "Positive Amount")]
        public void OrderAmountMustBeGreaterThanZero(decimal amount, bool passFail, string description)
        {
            var order = _orderBuilder.WithTestValues()
                                     .Id(0)
                                     .TotalAmount(amount)
                                     .Customer(BuildTestCustomer(TestCase.UseObjectValidId))
                                     .Build();
            RunOrderTest(passFail, description, order);
        }

        [Theory]
        [InlineData(TestCase.UseNull, false, "Customer is null")]
        [InlineData(TestCase.UseObjectValidId, true, "Customer is not null")]
        public void OrderMustHaveACustomer(TestCase testCase, bool passFail, string description)
        {
            var order = _orderBuilder.WithTestValues()
                                     .Id(0)
                                     .Customer(BuildTestCustomer(testCase))
                                     .Build();
            RunOrderTest(passFail, description, order);
        }

        private static Customer BuildTestCustomer(TestCase testCase)
        {
            Customer _customer;
            switch (testCase)
            {
                case TestCase.UseObjectValidId:
                    _customer = new(1);
                    break;
                case TestCase.UseObjectInvalidId:
                    _customer = new(0);
                    break;
                default:
                    _customer = null;
                    break;
            }

            return _customer;
        }

        private void RunOrderTest(bool passFail, string description, Model.Order order)
        {
            if (passFail)
                _orderService.PlaceOrder(order);
            else
                Assert.Throws<InvalidOrderException>(() => _orderService.PlaceOrder(order));

            _logger.WriteLine($"Pass/Fail: {passFail} - {description}");
        }
    }
}
