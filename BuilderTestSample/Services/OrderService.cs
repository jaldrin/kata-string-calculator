using BuilderTestSample.Exceptions;
using BuilderTestSample.Model;

namespace BuilderTestSample.Services
{
    public class OrderService
    {
        public void PlaceOrder(Order order)
        {
            ValidateOrder(order);
            ExpediteOrder(order);
            AddOrderToCustomerHistory(order);
        }

        private void ValidateOrder(Order order)
        {
            // throw InvalidOrderException unless otherwise noted.

            if (order.Id != 0) 
                throw new InvalidOrderException("Order ID must be 0.");

            if (order.TotalAmount <= 0) 
                throw new InvalidOrderException("Order amount must be greater than zero.");
            if (order.Customer is null)
                throw new InvalidOrderException("Order must have a customer.");

            ValidateCustomer(order.Customer);
        }

        private void ValidateCustomer(Customer customer)
        {
            if (customer.Id <= 0) 
                throw new InvalidCustomerException("Customer must have an ID.");
            if (customer.HomeAddress is null)
                throw new InvalidCustomerException("Customer must have an address.");
            if (string.IsNullOrWhiteSpace(customer.FirstName) || string.IsNullOrWhiteSpace(customer.LastName))
                throw new InvalidCustomerException("Customer must have a first and last name.");
            if (customer.CreditRating <= 200)
                throw new InsufficientCreditException("Customer must have credit rating > 200");
            if (customer.TotalPurchases < 0)
                throw new InvalidCustomerException("Customer must have total purchases >= 0.");

            ValidateAddress(customer.HomeAddress);
        }

        private void ValidateAddress(Address homeAddress)
        {
            if (string.IsNullOrWhiteSpace(homeAddress.Street1))
                throw new InvalidAddressException("Street1 is required.");
            if (string.IsNullOrWhiteSpace(homeAddress.City))
                throw new InvalidAddressException("City is required.");
            if (string.IsNullOrWhiteSpace(homeAddress.State))
                throw new InvalidAddressException("State is required.");
            if (string.IsNullOrWhiteSpace(homeAddress.PostalCode))
                throw new InvalidAddressException("Postal Code is required.");
            if (string.IsNullOrWhiteSpace(homeAddress.Country))
                throw new InvalidAddressException("Country is required.");
        }

        private void ExpediteOrder(Order order)
        {
            // TODO: if customer's total purchases > 5000 and credit rating > 500 set IsExpedited to true
        }

        private void AddOrderToCustomerHistory(Order order)
        {
            // TODO: add the order to the customer

            // TODO: update the customer's total purchases property
        }
    }
}
