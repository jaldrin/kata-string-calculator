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

        #region Validate Order
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
            RunOrderTest(passFail, description, order, typeof(InvalidOrderException));
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
            RunOrderTest(passFail, description, order, typeof(InvalidOrderException));
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
            RunOrderTest(passFail, description, order, typeof(InvalidOrderException));
        }
        #endregion

        #region Validate Customer
        [Theory]
        [InlineData(-1, false, "Negative ID")]
        [InlineData(0, false, "Edge Case")]
        [InlineData(1, true, "Positive ID")]
        public void CustomerMustHaveAnID(int id, bool passFail, string description)
        {
            var customer = new CustomerBuilder(id).WithTestValues()
                                                  .Build();
            var order = new OrderBuilder().Id(0)
                                          .Customer(customer)
                                          .TotalAmount(100m)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidCustomerException));
        }
        #endregion

        #region Private Test Methods
        private static Customer BuildTestCustomer(TestCase testCase)
        {
            Customer _customer;
            switch (testCase)
            {
                case TestCase.UseObjectValidId:
                    _customer = new CustomerBuilder(1).WithTestValues().Build();
                    break;
                case TestCase.UseObjectInvalidId:
                    _customer = new CustomerBuilder(0).WithTestValues().Build();
                    break;
                default:
                    _customer = null;
                    break;
            }

            return _customer;
        }

        private void RunOrderTest(bool passFail, string description, Model.Order order, Type exceptionType)
        {
            string message = $"Pass/Fail: {passFail} - {description}";

            try
            {
                _orderService.PlaceOrder(order);
            }
            catch (Exception ex)
            {
                Assert.Equal(exceptionType.Name, ex.GetType().Name);
                message = $"Pass/Fail: {passFail} - {ex.GetType().Name} - {description}";
            }

            _logger.WriteLine(message);
        }
        #endregion
    }
}
