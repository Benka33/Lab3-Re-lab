using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;

namespace ClientSide
{
    //The class that setups the connection with the NoSQL database instance of MongoDB
    //it adds the data recevied from the clientSide to the database and returns the requested data to the clientSide depending on called method
    public sealed class Mongo
    {
        private string connectionString = "mongodb://localhost:C2y6yDjf5%2FR%2Bob0N8A7Cgv30VRDJIWEHLM%2B4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw%2FJw%3D%3D@localhost:10255/admin?ssl=true";
        private string databaseName = "TravelDB";
           private String customer = "Customer", 
                          transaction = "Transaction";

        private MongoClientSettings settings;
        private MongoClient mongoClient;
        private IMongoDatabase database;
        //Settings for MongoDB
        Mongo()
        {
            settings = MongoClientSettings.FromUrl(
                new MongoUrl(connectionString)
            );
            settings.SslSettings =
                new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            mongoClient = new MongoClient(settings);
            database = mongoClient.GetDatabase(databaseName);
        }

        private static readonly object padlock = new object();
        private static Mongo instance = null;
        //Create a MongoDB instance
        public static Mongo Instance
        {
            get
            {
                if (instance==null)
                {
                    instance = new Mongo();
                }

                return instance;
            }
        }
        //Method that saves the transaction sent from the clientSide and inserts it into the database
        public void SaveTransaction(Transaction transaction)
        {
            var collection = database.GetCollection<Transaction>(this.transaction);
            var document = transaction;
            collection.InsertOne(document);
        }
        //Method that saves the customer sent from the clientSide and inserts it into the database
        public void SaveCustomer(Customer customer)
        {
            var collection = database.GetCollection<Customer>(this.customer);
            var document = customer;
            collection.InsertOne(document);
        }
        //Returns the customers from the database so they can be printed on the page at the clientSide
        public List<BsonDocument> GetCardHolders()
        {
            var projection = Builders<BsonDocument>.Projection.Exclude("_id").Exclude("Balance");
            return database.GetCollection<BsonDocument>(customer).Find(new BsonDocument()).Project(projection).ToList();
        }

        //Returns the specific transaction from the database so it can be printed on the page at the clientSide
        public List<BsonDocument> GetSpecificTransactions(string card)
        {
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Eq("CardNumber", Convert.ToInt32(card));
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");
            return database.GetCollection<BsonDocument>(transaction).Find(filter).Project(projection).ToList();
        }

    }

}