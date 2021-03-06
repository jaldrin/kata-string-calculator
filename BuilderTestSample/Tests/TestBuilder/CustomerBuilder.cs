using BuilderTestSample.Model;

namespace BuilderTestSample.Tests.TestBuilder
{
    /// <summary>
    /// Reference: https://ardalis.com/improve-tests-with-the-builder-pattern-for-test-data
    /// </summary>
    public class CustomerBuilder
    {
        private readonly int _id;
        private string _firstName;
        private string _lastName;
        private Address _homeAddress;
        private int _creditRating;
        private decimal _totalPurchases;

        public CustomerBuilder(int id) 
            => _id = id;

        public CustomerBuilder FirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        public CustomerBuilder LastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }

        public CustomerBuilder HomeAddress(Address homeAddress)
        {
            _homeAddress = homeAddress;
            return this;
        }

        public CustomerBuilder CreditRating(int creditRating)
        {
            _creditRating = creditRating;
            return this;
        }

        public CustomerBuilder TotalPurchases(decimal totalPurchases)
        {
            _totalPurchases = totalPurchases;
            return this;
        }

        public Customer Build()
        {
            return new Customer(_id)
            {
                FirstName = _firstName,
                LastName = _lastName,
                HomeAddress = _homeAddress,
                CreditRating = _creditRating,
                TotalPurchases = _totalPurchases
            };
        }

        public CustomerBuilder WithDefaultValues()
        {
            _firstName = nameof(Customer.FirstName);
            _lastName = nameof(Customer.LastName);
            _homeAddress = new AddressBuilder().WithDefaultValues().Build();
            _creditRating = 600;
            _totalPurchases = 0m;

            return this;
        }
    }
}
