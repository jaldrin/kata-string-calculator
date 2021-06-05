using BuilderTestSample.Model;

namespace BuilderTestSample.Tests.TestBuilder
{
    public class AddressBuilder
    {
        private string _street1;
        private string _street2;
        private string _street3;
        private string _city;
        private string _state;
        private string _postalCode;
        private string _country;

        public AddressBuilder Street1(string street1)
        {
            _street1 = street1;
            return this;
        }

        public AddressBuilder Street2(string street2)
        {
            _street2 = street2;
            return this;
        }

        public AddressBuilder Street3(string street3)
        {
            _street3 = street3;
            return this;
        }

        public AddressBuilder City(string city)
        {
            _city = city;
            return this;
        }

        public AddressBuilder State(string state)
        {
            _state = state;
            return this;
        }

        public AddressBuilder PostalCode(string postalCode)
        {
            _postalCode = postalCode;
            return this;
        }

        public AddressBuilder Country(string country)
        {
            _country = country;
            return this;
        }

        public Address Build()
        {
            return new Address
            {
                Street1 = _street1,
                Street2 = _street2,
                Street3 = _street3,
                City = _city,
                State = _state,
                PostalCode = _postalCode,
                Country = _country
            };
        }

        public AddressBuilder WithDefaultValues()
        {
            _street1 = nameof(Address.Street1);
            _street2 = null;
            _street3 = null;
            _city = "Las Vegas";
            _state = "NV";
            _postalCode = "89001";
            _country = "USA";

            return this;
        }
    }
}
