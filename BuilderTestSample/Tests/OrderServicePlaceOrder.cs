using BuilderTestSample.Exceptions;
using BuilderTestSample.Model;
using BuilderTestSample.Services;
using BuilderTestSample.Tests.TestBuilder;
using Shouldly;
using System;
using Xunit;
using Xunit.Abstractions;

namespace BuilderTestSample.Tests
{
    public class OrderServicePlaceOrder
    {
        #region Constructor
        private readonly OrderBuilder _orderBuilder = new();
        private readonly ITestOutputHelper _logger;
        public OrderServicePlaceOrder(ITestOutputHelper logger)
            => _logger = logger;
        #endregion

        #region Validate Order
        [Theory]
        [InlineData(-1, false, "Negative ID")]
        [InlineData(0, true, "Valid ID")]
        [InlineData(1, false, "Positive ID")]
        public void OrderIdMustBeZero(int id, bool passFail, string description)
        {
            var order = _orderBuilder.WithDefaultValues()
                                     .Id(id)
                                     .Build();
            RunOrderTest(passFail, description, order, typeof(InvalidOrderException));
        }

        [Theory]
        [InlineData(-1, false, "Negative Amount")]
        [InlineData(0, false, "Edge Case Amount")]
        [InlineData(1, true, "Positive Amount")]
        public void OrderAmountMustBeGreaterThanZero(decimal amount, bool passFail, string description)
        {
            var order = _orderBuilder.WithDefaultValues()
                                     .Id(0)
                                     .TotalAmount(amount)
                                     .Build();
            RunOrderTest(passFail, description, order, typeof(InvalidOrderException));
        }

        [Theory]
        [InlineData(false, "Customer is valid")]
        [InlineData(true, "Customer is invalid")]
        public void OrderMustHaveACustomer(bool passFail, string description)
        {
            var customer = (passFail ? new CustomerBuilder(1).WithDefaultValues().Build() : null);
            var order = _orderBuilder.WithDefaultValues()
                                     .Customer(customer)
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
            var customer = new CustomerBuilder(id).WithDefaultValues().Build();

            var order = new OrderBuilder().Id(0)
                                          .Customer(customer)
                                          .TotalAmount(100m)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidCustomerException));
            order.Customer.Id.ShouldBe(id);
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
            var customer = new CustomerBuilder(1).WithDefaultValues()
                                                 .FirstName(firstName)
                                                 .LastName(lastName)
                                                 .Build();
            var order = new OrderBuilder().WithDefaultValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidCustomerException));
            order.Customer.FirstName.ShouldBe(firstName);
            order.Customer.LastName.ShouldBe(lastName);
        }

        [Theory]
        [InlineData(0, false, "Credit too low")]
        [InlineData(200, false, "Credit edge case")]
        [InlineData(201, true, "Credit is valid")]
        public void CustomerMustHaveCreditRatingOver200(int creditRating, bool passFail, string description)
        {
            var customer = new CustomerBuilder(1).WithDefaultValues()
                                                 .CreditRating(creditRating)
                                                 .Build();
            var order = new OrderBuilder().WithDefaultValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InsufficientCreditException));
            order.Customer.CreditRating.ShouldBe(creditRating);
        }

        [Theory]
        [InlineData(-1, false, "Negative Purchases")]
        [InlineData(0, true, "Edge Case Purchases")]
        [InlineData(+1, true, "Positive Purchases")]
        public void CustomerMustHavePositiveTotalPurchases(decimal totalPurchases, bool passFail, string description)
        {
            var customer = new CustomerBuilder(1).WithDefaultValues()
                                                 .TotalPurchases(totalPurchases)
                                                 .Build();
            var orderAmount = 0.01m;
            var order = new OrderBuilder().WithDefaultValues()
                                          .Customer(customer)
                                          .TotalAmount(orderAmount)
                                          .Build();

            RunOrderTest(passFail, description, order, typeof(InvalidCustomerException));
            if (passFail)
                order.Customer.TotalPurchases.ShouldBe(totalPurchases + orderAmount);
        }

        [Theory]
        [InlineData(false, "Does not have an address")]
        [InlineData(true, "Has an address")]
        public void CustomerMustHaveAnAddress(bool hasAddress, string description)
        {
            var address = (hasAddress) ? new AddressBuilder().WithDefaultValues().Build() : null;
            var customer = new CustomerBuilder(1).WithDefaultValues()
                                                 .HomeAddress(address)
                                                 .Build();
            var order = new OrderBuilder().WithDefaultValues()
                                          .Customer(customer)
                                          .Build();

            RunOrderTest(hasAddress, description, order, typeof(InvalidCustomerException));
            if (hasAddress)
                order.Customer.HomeAddress.ShouldNotBeNull();
            else
                order.Customer.HomeAddress.ShouldBeNull();
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
            var order = BuildOrderToTest(address);

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
            order.Customer.HomeAddress.Street1.ShouldBe(street1);
        }

        [Theory]
        [InlineData(null, true, "Street2 is null")]
        [InlineData("", true, "Street2 is empty")]
        [InlineData(" ", true, "Street2 is blank")]
        [InlineData("Has value", true, "Street2 has value")]
        public void AddressStreet2IsNotRequired(string street2, bool passFail, string description)
        {
            var address = new AddressBuilder().WithDefaultValues()
                                              .Street2(street2)
                                              .Build();
            var order = BuildOrderToTest(address);

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
            order.Customer.HomeAddress.Street2.ShouldBe(street2);
        }

        [Theory]
        [InlineData(null, true, "Street3 is null")]
        [InlineData("", true, "Street3 is empty")]
        [InlineData(" ", true, "Street3 is blank")]
        [InlineData("Has value", true, "Street3 has value")]
        public void AddressStreet3IsNotRequired(string street3, bool passFail, string description)
        {
            var address = new AddressBuilder().WithDefaultValues()
                                              .Street3(street3)
                                              .Build();
            var order = BuildOrderToTest(address);

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
            order.Customer.HomeAddress.Street3.ShouldBe(street3);
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
            var order = BuildOrderToTest(address);

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
            order.Customer.HomeAddress.City.ShouldBe(city);
        }

        [Theory]
        [InlineData(null, false, "State is null")]
        [InlineData("", false, "State is empty")]
        [InlineData(" ", false, "State is blank")]
        [InlineData("Has value", true, "State has value")]
        public void AddressStateIsRequired(string state, bool passFail, string description)
        {
            var address = new AddressBuilder().WithDefaultValues()
                                              .State(state)
                                              .Build();
            var order = BuildOrderToTest(address);

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
            order.Customer.HomeAddress.State.ShouldBe(state);

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
            var order = BuildOrderToTest(address);

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
            order.Customer.HomeAddress.PostalCode.ShouldBe(postalCode);
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
            var order = BuildOrderToTest(address);

            RunOrderTest(passFail, description, order, typeof(InvalidAddressException));
            order.Customer.HomeAddress.Country.ShouldBe(country);
        }
        #endregion

        #region Expedite Order
        [Theory]
        [InlineData(5000, 500, false, "Edge cases")]
        [InlineData(5001, 501, true, "Positive test case")]
        public void ValidateExpediteOrderRules(decimal totalPurchases, int creditRating, bool passFail, string description)
        {
            var customer = new CustomerBuilder(1).WithDefaultValues()
                                                 .TotalPurchases(totalPurchases)
                                                 .CreditRating(creditRating)
                                                 .Build();
            var order = new OrderBuilder().WithDefaultValues()
                                          .Customer(customer)
                                          .Build();

            OrderService.PlaceOrder(order);
            order.IsExpected.ShouldBe(passFail);
            _logger.WriteLine($"Pass/Fail: {passFail} - {description}");
        }
        #endregion

        #region Add Order to History
        [Fact]
        public void AddTheOrderToTheCustomer()
        {
            var order = new OrderBuilder().WithDefaultValues().Build();

            OrderService.PlaceOrder(order);

            order.Customer.OrderHistory.ShouldNotBeEmpty();
            order.Customer.OrderHistory.ShouldHaveSingleItem();
        }

        [Theory]
        [InlineData(0, 100, 100, "Expect $100")]
        [InlineData(100, 100, 200, "Expect $200")]
        public void UpdateCustomerTotalPurchases(decimal customerPurchases, decimal orderAmount, decimal expected, string description)
        {
            var customer = new CustomerBuilder(1).WithDefaultValues()
                                                 .TotalPurchases(customerPurchases)
                                                 .Build();
            var order = new OrderBuilder().WithDefaultValues()
                                          .TotalAmount(orderAmount)
                                          .Customer(customer)
                                          .Build();

            OrderService.PlaceOrder(order);

            order.Customer.TotalPurchases.ShouldBe(expected);
            _logger.WriteLine($"{description}: GOT: {order.Customer.TotalPurchases:C}");
        }
        #endregion

        #region Private Test Methods
        private static Order BuildOrderToTest(Address address)
        {
            var customer = new CustomerBuilder(1).WithDefaultValues()
                                                 .HomeAddress(address)
                                                 .Build();
            var order = new OrderBuilder().WithDefaultValues()
                                          .Customer(customer)
                                          .Build();
            return order;
        }

        private void RunOrderTest(bool passFail, string description, Model.Order order, Type exceptionType)
        {
            string message;

            try
            {
                OrderService.PlaceOrder(order);
                passFail.ShouldBeTrue();
                message = $"Pass/Fail: {passFail} - {description}";
            }
            catch (Exception ex)
            {
                ex.GetType().Name.ShouldBe(exceptionType.Name);
                passFail.ShouldBeFalse();
                message = $"Pass/Fail: {passFail} - {ex.GetType().Name} - {description}";
            }

            _logger.WriteLine(message);
        }
        #endregion
    }
}
