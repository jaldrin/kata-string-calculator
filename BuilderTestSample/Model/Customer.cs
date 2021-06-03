﻿using System.Collections.Generic;

namespace BuilderTestSample.Model
{
    public class Customer
    {
        #region Constructor
        public Customer(int id)
            => Id = id;
        #endregion

        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address HomeAddress { get; set; }
        public int CreditRating { get; set; }
        public decimal TotalPurchases { get; set; }

        public List<Order> OrderHistory { get; set; } = new();
    }
}
