using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide
{
    //class used in order to insert the transactions into the NoSQL MongoDB database
    //Simple Object of Transaction
    public class Transaction
    {
        private string transactionDate;
        private int cardNumber;
        private double transactionAmount;

        public Transaction(string transactionDate, int cardNumber, double transactionAmount)
        {
            this.transactionDate = transactionDate;
            this.cardNumber = cardNumber;
            this.transactionAmount = transactionAmount;
        }

        public string TransactionDate => transactionDate;

        public int CardNumber => cardNumber;

        public double TransactionAmount => transactionAmount;
    }
}