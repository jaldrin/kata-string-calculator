using BuilderTestSample.Exceptions;
using BuilderTestSample.Services;
using BuilderTestSample.Tests.TestBuilder;
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

        [Theory]
        [InlineData(-1, false, "Negative ID")]
        [InlineData(0, true, "Valid ID")]
        [InlineData(1, false, "Positive ID")]
        public void OrderIdMustBeZero(int id, bool passFail, string description)
        {
            var order = _orderBuilder.WithTestValues()
                                     .Id(id)
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
                                     .Build();
            RunOrderTest(passFail, description, order);
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
