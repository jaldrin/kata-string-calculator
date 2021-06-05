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

        [Theory]
        [InlineData(null, null, false, "Both names are null")]
        [InlineData(null, "Last", false, "First name is null")]
        [InlineData("First", null, false, "Last name is null")]
        [InlineData("", "", false, "Both names are empty")]
        [InlineData("", "Last", false, "First name is empty")]
        [InlineData("First", "", false, "Last name is empty")]
        [InlineData(" ", " ", false, "Both names are blank")]
        [InlineData(" ", "Last", false, "First name is blank")]
        [InlineData("First", " ", false, "Last name is blank")]
        [InlineData("First", "Last", true, "First & Last name have values")]
        public void CustomerMustHaveFirstAndLastNames(string firstName, string lastName,
                                                      bool passFail, string description)
        {
            var customer = new CustomerBuilder(1).WithTestValues()
                                                 .FirstName(firstName)
                                                 .LastName(lastName)
                                                 .Build();
            var order = new OrderBuilder().WithTestValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidCustomerException));
        }

        [Theory]
        [InlineData(0, false, "Credit too low")]
        [InlineData(200, false, "Credit edge case")]
        [InlineData(201, true, "Credit is valid")]
        public void CustomerMustHaveCreditRatingOver200(int creditRating, bool passFail, string description)
        {
            var customer = new CustomerBuilder(1).WithTestValues()
                                                 .CreditRating(creditRating)
                                                 .Build();
            var order = new OrderBuilder().WithTestValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InsufficientCreditException));
        }

        [Theory]
        [InlineData(-1, false, "Negative Purchases")]
        [InlineData(0, true, "Edge Case Purchases")]
        [InlineData(+1, true, "Positive Purchases")]
        public void CustomerMustHavePositiveTotalPurchases(decimal totalPurchases, bool passFail, string description)
        {
            var customer = new CustomerBuilder(1).WithTestValues()
                                                 .TotalPurchases(totalPurchases)
                                                 .Build();
            var order = new OrderBuilder().WithTestValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidCustomerException));
        }

        [Theory]
        [InlineData(false, "Does not have an address")]
        [InlineData(true, "Has an address")]
        public void CustomerMustHaveAnAddress(bool hasAddress, string description)
        {
            Address address = null;
            if (hasAddress)
                address = new AddressBuilder().WithDefaultValues().Build();

            var customer = new CustomerBuilder(1).WithTestValues()
                                                 .HomeAddress(address)
                                                 .Build();
            var order = new OrderBuilder().WithTestValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(hasAddress, description, order, typeof(InvalidCustomerException));
        }
        #endregion

        #region Validate Address
        [Theory]
        [InlineData(null, false, "Street1 is null")]
        [InlineData("", false, "Street1 is empty")]
        [InlineData(" ", false, "Street1 is blank")]
        [InlineData("Has value", true, "Street1 has value")]
        public void AddressStreet1IsRequired(string street1, bool passFail, string description)
        {
            var address = new AddressBuilder().WithDefaultValues()
                                              .Street1(street1)
                                              .Build();
            var customer = new CustomerBuilder(1).WithTestValues()
                                                 .HomeAddress(address)
                                                 .Build();
            var order = new OrderBuilder().WithTestValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
        }

        [Theory]
        [InlineData(null, false, "City is null")]
        [InlineData("", false, "City is empty")]
        [InlineData(" ", false, "City is blank")]
        [InlineData("Has value", true, "City has value")]
        public void AddressCityIsRequired(string city, bool passFail, string description)
        {
            var address = new AddressBuilder().WithDefaultValues()
                                              .City(city)
                                              .Build();
            var customer = new CustomerBuilder(1).WithTestValues()
                                                 .HomeAddress(address)
                                                 .Build();
            var order = new OrderBuilder().WithTestValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
        }

        [Theory]
        [InlineData(null, false, "State is null")]
        [InlineData("", false, "State is empty")]
        [InlineData(" ", false, "State is blank")]
        [InlineData("Has value", true, "State has value")]
        public void AddressStateIsRequired(string state, bool passFail, string description)
        {
            var address = new AddressBuilder().WithDefaultValues()
                                              .City(state)
                                              .Build();
            var customer = new CustomerBuilder(1).WithTestValues()
                                                 .HomeAddress(address)
                                                 .Build();
            var order = new OrderBuilder().WithTestValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
        }

        [Theory]
        [InlineData(null, false, "Postal Code is null")]
        [InlineData("", false, "Postal Code is empty")]
        [InlineData(" ", false, "Postal Code is blank")]
        [InlineData("Has value", true, "Postal Code has value")]
        public void AddressPostalCodeIsRequired(string postalCode, bool passFail, string description)
        {
            var address = new AddressBuilder().WithDefaultValues()
                                              .PostalCode(postalCode)
                                              .Build();
            var customer = new CustomerBuilder(1).WithTestValues()
                                                 .HomeAddress(address)
                                                 .Build();
            var order = new OrderBuilder().WithTestValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
        }

        [Theory]
        [InlineData(null, false, "Postal Code is null")]
        [InlineData("", false, "Postal Code is empty")]
        [InlineData(" ", false, "Postal Code is blank")]
        [InlineData("Has value", true, "Postal Code has value")]
        public void AddressCountryCodeIsRequired(string country, bool passFail, string description)
        {
            var address = new AddressBuilder().WithDefaultValues()
                                              .Country(country)
                                              .Build();
            var customer = new CustomerBuilder(1).WithTestValues()
                                                 .HomeAddress(address)
                                                 .Build();
            var order = new OrderBuilder().WithTestValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
        }
        #endregion

        #region Expedite Order
        [Theory]
        [InlineData(5000, 500, false, "Edge cases")]
        [InlineData(5001, 501, true, "Positive test case")]
        public void ValidateExpediteOrderRules(decimal totalPurchases, int creditRating, bool passFail, string description)
        {
            var customer = new CustomerBuilder(1).WithTestValues()
                                                 .TotalPurchases(totalPurchases)
                                                 .CreditRating(creditRating)
                                                 .Build();
            var order = new OrderBuilder().WithTestValues()
                                          .Customer(customer)
                                          .Build();

            _orderService.PlaceOrder(order);
            Assert.Equal(passFail, order.IsExpected);
            _logger.WriteLine($"Pass/Fail: {passFail} - {description}");
        }
        #endregion

        #region Private Test Methods
        private static Customer BuildTestCustomer(TestCase testCase)
            => testCase switch
            {
                TestCase.UseObjectValidId => new CustomerBuilder(1).WithTestValues().Build(),
                TestCase.UseObjectInvalidId => new CustomerBuilder(0).WithTestValues().Build(),
                _ => null,
            };

        private void RunOrderTest(bool passFail, string description, Model.Order order, Type exceptionType)
        {
            string message;

            try
            {
                _orderService.PlaceOrder(order);
                Assert.True(passFail);
                message = $"Pass/Fail: {passFail} - {description}";
            }
            catch (Exception ex)
            {
                Assert.Equal(exceptionType.Name, ex.GetType().Name);
                Assert.False(passFail);
                message = $"Pass/Fail: {passFail} - {ex.GetType().Name} - {description}";
            }

            _logger.WriteLine(message);
        }
        #endregion
    }
}
