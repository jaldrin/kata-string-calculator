using BuilderTestSample.Model;

namespace BuilderTestSample.Tests.TestBuilder
{
    /// <summary>
    /// Reference: https://ardalis.com/improve-tests-with-the-builder-pattern-for-test-data
    /// </summary>
    public class OrderBuilder
    {
        private int _id;
        private Customer _customer;
        private decimal _totalAmount;
        private bool _isExpected;

        public OrderBuilder Id(int id)
        {
            _id = id;
            return this;
        }

        public OrderBuilder Customer(Customer customer)
        {
            _customer = customer;
            return this;
        }

        public OrderBuilder TotalAmount(decimal totalAmount)
        {
            _totalAmount = totalAmount;
            return this;
        }

        public OrderBuilder IsExpected(bool isExpected)
        {
            _isExpected = isExpected;
            return this;
        }

        public Order Build()
        {
            var _order = new Order
            {
                Id = _id,
                Customer = _customer,
                TotalAmount = _totalAmount,
                IsExpected = _isExpected
            };

            return _order;
        }

        public OrderBuilder WithTestValues()
        {
            _totalAmount = 100m;
            // TODO: replace next lines with a CustomerBuilder you create
            // _order.Customer = new Customer();
            // _order.Customer.HomeAddress = new Address();

            return this;
        }
    }
}
