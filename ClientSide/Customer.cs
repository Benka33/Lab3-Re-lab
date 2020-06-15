using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide
{
    //class used in order to insert the customers into the NoSQL MongoDB database
    //Simple Object of customer
    public class Customer
    {
        private int cardNumber;
        private string holder;
        private string expiryDate;
        private double balance;

        public Customer(int cardNumber, string holder, string expiryDate, double balance)
        {
            this.cardNumber = cardNumber;
            this.holder = holder;
            this.expiryDate = expiryDate;
            this.balance = balance;
        }

        public int CardNumber => cardNumber;

        public string Holder => holder;

        public string ExpiryDate => expiryDate;

        public double Balance => balance;
    }

}