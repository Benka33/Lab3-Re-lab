using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


// The clientside which performs actions after the button for getting the price has been pressed
//the application takes in inputs from the user and adds to the database tables, they also show a report further down
// the code for the report can be seen in the default.aspx source code
//the first 6 tables shown will be from the SQL database while the last two from the NoSQL database that uses MongoDB
namespace ClientSide
{
    public partial class Default : System.Web.UI.Page
    {
        private string accountName = "lab3cloud";      // storage account name          
        private string accountKey = "2f0Hs+WbONYYKMVa8yGS9cu3Z2e6w+qA228whjpjo2enzmBFH7pjeFbt9VOznXN7JOqxr/47RRm0viQ3ImExYw=="; //key to account
        private SqlConnection conn;
        private SqlConnection conn1;
        private SqlConnection conn2;

        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=testdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conn1 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=testdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conn2 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=testdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
           //Method to display a specific transaction
            getTransaction();
            //Method for displaying all customers
            getCustomers();
        }



        //When pressing the button the program performs takes the users inputs for the flight, hotel and payment info and puts out the totalCost of the tickets and hotel
        protected void BtnGet_Click(object sender, EventArgs e)
        {
            //check to see so destinations are different places
            if (To.SelectedItem.Value == From.SelectedItem.Value)
            {
                Response.Write("<script>alert('Destinations needs to be different'); </script>");
            }
            else
            {
                try
                {
                    //connection to storage account
                    StorageCredentials credentials = new StorageCredentials(accountName, accountKey);
                    CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, useHttps: true);

                    CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();

                    //queues where information gets stored and read from
                    CloudQueue webQ = cloudQueueClient.GetQueueReference("webqueue");
                    CloudQueue flightQ = cloudQueueClient.GetQueueReference("flightqueue");
                    CloudQueue hotelQ = cloudQueueClient.GetQueueReference("hotelqueue");
                    CloudQueue carQ = cloudQueueClient.GetQueueReference("carqueue");
                    CloudQueue paymentQ = cloudQueueClient.GetQueueReference("paymentqueue");

                    double flightCost = 0;
                    double hotelCost = 0;
                    double carCost = 0;

                    //Create the queues if they do not exist already
                    webQ.CreateIfNotExists();
                    flightQ.CreateIfNotExists();
                    hotelQ.CreateIfNotExists();
                    carQ.CreateIfNotExists();
                    paymentQ.CreateIfNotExists();

                    //json objects for storing the data for the different services
                    JObject flightJson = new JObject();
                    JObject hotelJson = new JObject();
                    JObject carJson = new JObject();
                    JObject paymentJson = new JObject();

                    //flight
                    //Store the data from the labels into the flight json
                    flightJson["from"] = From.SelectedValue;
                    flightJson["to"] = To.SelectedValue;

                    //If labels are empty we just store a 0
                    int flightTemp;
                    if (infants.Text == "")
                    {
                        flightJson["infants"] = 0;
                    }
                    //else save the value into the flight json
                    else
                    {
                        if (int.TryParse(infants.Text, out flightTemp))
                        {
                            flightJson["infants"] = flightTemp;
                        }
                    }

                    if (children.Text == "")
                    {
                        flightJson["children"] = 0;
                    }
                    else
                    {
                        if (int.TryParse(children.Text, out flightTemp))
                        {
                            flightJson["children"] = flightTemp;
                        }
                    }

                    if (adults.Text == "")
                    {
                        flightJson["adults"] = 0;
                    }
                    else
                    {
                        if (int.TryParse(adults.Text, out flightTemp))
                        {
                            flightJson["adults"] = flightTemp;
                        }
                    }

                    if (seniorNbr.Text == "")
                    {
                        flightJson["oldies"] = 0;
                    }
                    else
                    {
                        if (int.TryParse(seniorNbr.Text, out flightTemp))
                        {
                            flightJson["oldies"] = flightTemp;
                        }
                    }

                    //convert the json to a String and save it into a CloudQueueMessage
                    CloudQueueMessage message = new CloudQueueMessage(flightJson.ToString());
                    //add that message to the flightQ
                    flightQ.AddMessage(message);

                    //get the message from webQ
                    message = webQ.GetMessage();
                    while (message == null)
                    {
                        message = webQ.GetMessage();
                    }

                    JObject jObject = (JObject)JsonConvert.DeserializeObject(message.AsString);
                    flightCost = (double)jObject["flightCost"];
                    //delete the message in webQ
                    webQ.DeleteMessage(message);


                    //hotelJson     Not needed for this application in lab3 so it is commented
                    //Store the data from the labels into the hotel json
                    //If labels are empty we just store a 0
                    if (String.IsNullOrEmpty(nights.Text))
                    {
                        paymentJson["hotelCost"] = 0;
                    }
                    else // else save the value into the hotel json
                    {
                        hotelJson["roomType"] = hotelRoom.SelectedValue;

                        int hotelTemp;
                        if (travellers.Text == "")
                        {
                            hotelJson["travellers"] = 0;
                        }
                        else
                        {
                            if (int.TryParse(travellers.Text, out hotelTemp))
                            {
                                hotelJson["travellers"] = hotelTemp;
                            }
                        }

                        if (nights.Text == "")
                        {
                            hotelJson["nights"] = 0;
                        }
                        else
                        {
                            if (int.TryParse(nights.Text, out hotelTemp))
                            {
                                hotelJson["nights"] = hotelTemp;
                            }
                        }

                        if (seniors.Text == "")
                        {
                            hotelJson["seniors"] = 0;
                        }
                        else
                        {
                            if (int.TryParse(seniors.Text, out hotelTemp))
                            {
                                hotelJson["seniors"] = hotelTemp;
                            }
                        }

                        if (primGuest.Text == "")
                        {
                            hotelJson["primGuest"] = "";
                        }
                        else
                        {

                            hotelJson["primGuest"] = primGuest.Text;

                        }

                        //convert the json to a String and save it into a CloudQueueMessage
                        message = new CloudQueueMessage(hotelJson.ToString());
                        //add that message to the hotelQ
                        hotelQ.AddMessage(message);

                        //get the message from webQ
                        message = webQ.GetMessage();
                        while (message == null)
                        {
                            message = webQ.GetMessage();
                        }

                        jObject = (JObject)JsonConvert.DeserializeObject(message.AsString);
                        hotelCost = (double)jObject["hotelCost"];
                        //delete the message in webQ
                        webQ.DeleteMessage(message);
                    }

                    //Carcost
                    // modelYear, seats, age, fuelType

                    carJson["fuelType"] = fuelType.SelectedValue;

                    int carTemp;
                    if (seats.Text == "")
                    {
                        carJson["seats"] = 0;
                    }
                    
                    else
                    {
                        if (int.TryParse(seats.Text, out carTemp))
                        {
                            carJson["seats"] = carTemp;
                        }
                    }

                    if (modelYear.Text == "")
                    {
                        carJson["modelYear"] = 0;
                    }

                    else
                    {
                        if (int.TryParse(modelYear.Text, out carTemp))
                        {
                            carJson["modelYear"] = carTemp;
                        }
                    }

                    if (driverAge.Text == "")
                    {
                        carJson["age"] = 0;
                    }

                    else
                    {
                        if (int.TryParse(driverAge.Text, out carTemp))
                        {
                            carJson["age"] = carTemp;
                        }
                    }

                    //convert the json to a String and save it into a CloudQueueMessage
                    message = new CloudQueueMessage(carJson.ToString());
                    //add that message to the hotelQ
                    carQ.AddMessage(message);

                    //get the message from webQ
                    message = webQ.GetMessage();
                    while (message == null)
                    {
                        message = webQ.GetMessage();
                    }

                    jObject = (JObject)JsonConvert.DeserializeObject(message.AsString);
                    carCost = (double)jObject["carCost"];
                    //delete the message in webQ
                    webQ.DeleteMessage(message);

                    //paymentJson
                    //Store the data from the labels into the hotel json
                    //if labels are empty store 0 or just an empty String to avoid null
                    int paymentTemp;
                    if (cardNbr.Text == "")
                    {
                        paymentJson["cardNbr"] = 0;
                    }
                    else
                    {
                        if (int.TryParse(cardNbr.Text, out paymentTemp))
                        {
                            paymentJson["cardNbr"] = paymentTemp;
                        }
                    }

                    if (cardHolder.Text == "")
                    {
                        paymentJson["cardHolder"] = "";
                    }
                    else
                    {

                        paymentJson["cardHolder"] = cardHolder.Text;

                    }

                    paymentJson["flightCost"] = flightCost;
                    paymentJson["hotelCost"] = hotelCost;
                    paymentJson["carCost"] = carCost;

                    //convert the json to a String and save it into a CloudQueueMessage
                    message = new CloudQueueMessage(paymentJson.ToString());
                    //add that message to the paymentQ
                    paymentQ.AddMessage(message);

                    //get the message from webQ
                    message = webQ.GetMessage();
                    while (message == null)
                    {
                        message = webQ.GetMessage();
                    }

                    jObject = (JObject)JsonConvert.DeserializeObject(message.AsString);
                    //delete the message in webQ
                    webQ.DeleteMessage(message);

                    //print out the totalCost for flight and hotel into the totalCost label
                    totalCost.Text = String.Format("{0:0.##}", jObject["totalCost"]);

                    //adds information to the database for flights
                    try
                    {
                        conn.Open();
                        //statement for the flights table
                        SqlCommand cmd = new SqlCommand(@"INSERT INTO dbo.flights (passportNbr, name, flightNbr, departureDate, price)
                                  VALUES (@passportNbr, @name, @flightNbr, @departureDate, @price)", conn);
                        if (passNumber.Text == "") ;
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("passportNbr", Int32.Parse(passNumber.Text)));
                            cmd.Parameters.Add(new SqlParameter("name", cardHolder.Text));
                            cmd.Parameters.Add(new SqlParameter("flightNbr", Int32.Parse(flightNumber.Text)));
                            cmd.Parameters.Add(new SqlParameter("departureDate", departDate.Text));
                            cmd.Parameters.Add(new SqlParameter("price", float.Parse(totalCost.Text)));
                            cmd.ExecuteNonQuery();

                            conn.Close();
                        }

                    }
                    catch (SqlException ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                    //adds information to the database for customer
                    try
                    {
                        conn1.Open();
                        //statement for the customer table
                        SqlCommand cmd1 = new SqlCommand(@"INSERT INTO dbo.customer (cardNbr, name, expireDate, balance)
                                  VALUES (@cardNbr, @name, @expireDate, @balance)", conn);
                        if (cardHolder.Text == "") ;
                        else
                        {
                            cmd1.Parameters.Add(new SqlParameter("cardNbr", cardNbr.Text));
                            cmd1.Parameters.Add(new SqlParameter("name", cardHolder.Text));
                            cmd1.Parameters.Add(new SqlParameter("expireDate", expireDate.Text));
                            cmd1.Parameters.Add(new SqlParameter("balance", float.Parse(balance.Text)));
                            cmd1.ExecuteNonQuery();

                            conn1.Close();

                            //Here the data for transaction and customers gets saved to the NoSQL MongoDB database
                            Mongo.Instance.SaveTransaction(new Transaction(DateTime.Now.ToString("M/d/yyyy"), Convert.ToInt32(cardNbr.Text), Convert.ToDouble(totalCost.Text)));
                            Mongo.Instance.SaveCustomer(new Customer(Convert.ToInt32(cardNbr.Text), cardHolder.Text, expireDate.Text, Convert.ToDouble(balance.Text)));
                        }
                        //listAirports.DataBind();
                        //GridView1.DataBind();
                    }
                    catch (SqlException ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                    
                    


                    //adds information to the database for bookings
                    try
                    {
                        conn2.Open();
                        //statement for the bookings table
                        SqlCommand cmd2 = new SqlCommand(@"INSERT INTO dbo.bookings (hotelCode, name, passportNbr, arrivalDate, nights, roomRate)
                                  VALUES (@hotelCode, @name, @passportNbr, @arrivalDate, @nights, @roomRate)", conn);
                        if (nights.Text == "") ;
                        else
                        {
                            cmd2.Parameters.Add(new SqlParameter("hotelCode", hotelList.SelectedItem.Value));
                            cmd2.Parameters.Add(new SqlParameter("name", primGuest.Text));
                            cmd2.Parameters.Add(new SqlParameter("passportNbr",Int32.Parse(hotelPassNumber.Text)));
                            cmd2.Parameters.Add(new SqlParameter("arrivalDate", hotelDate.Text));
                            cmd2.Parameters.Add(new SqlParameter("nights", Int32.Parse(nights.Text)));
                            cmd2.Parameters.Add(new SqlParameter("roomRate", (float)hotelCost));
                            cmd2.ExecuteNonQuery();

                            conn2.Close();
                        }
                    }
                    catch (SqlException ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                }
                catch (Exception ee) { Debug.WriteLine(ee); }
            }

        }

        //The three methods below lets the user go between the flight booking, hotel booking and payment via the flight, hotel and payment buttons
        //panels visable or not
        Boolean flightPanelVis = true;
        Boolean hotelPanelVis = false;
        Boolean paymentPanelVis = false;
        Boolean carPanelVis = false;
        protected void flight_Click(object sender, EventArgs e)
        {
            if (flightPanel.Visible == false)
            {
                flightPanelVis = true;
                hotelPanelVis = false;
                carPanelVis = false;
                paymentPanelVis = false;
                flightPanel.Visible = flightPanelVis;
                hotelPanel.Visible = hotelPanelVis;
                carPanel.Visible = carPanelVis;
                paymentPanel.Visible = paymentPanelVis;
            }
        }

        protected void hotel_Click(object sender, EventArgs e)
        {
            if (hotelPanel.Visible == false)
            {
                hotelPanelVis = true;
                flightPanelVis = false;
                paymentPanelVis = false;
                carPanelVis = false;
                hotelPanel.Visible = hotelPanelVis;
                flightPanel.Visible = flightPanelVis;
                carPanel.Visible = carPanelVis;
                paymentPanel.Visible = paymentPanelVis;
            }
        }

        protected void car_Click(object sender, EventArgs e)
        {
            if (carPanel.Visible == false)
            {
                //no need to show car for this assignment
                //carPanelVis = true;
                paymentPanelVis = false;
                flightPanelVis = false;
                hotelPanelVis = false;
                paymentPanel.Visible = paymentPanelVis;
                flightPanel.Visible = flightPanelVis;
                carPanel.Visible = carPanelVis;
                hotelPanel.Visible = hotelPanelVis;
            }
        }

        protected void payment_Click(object sender, EventArgs e)
        {
            if (paymentPanel.Visible == false)
            {
                paymentPanelVis = true;
                flightPanelVis = false;
                hotelPanelVis = false;
                carPanelVis = false;
                paymentPanel.Visible = paymentPanelVis;
                flightPanel.Visible = flightPanelVis;
                carPanel.Visible = carPanelVis;
                hotelPanel.Visible = hotelPanelVis;
            }
        }

        //Method for the NoSQL to retrive the transaction where cardNbr = 5656 in this case
        private void getTransaction()
        {
            //call the Mongo class to get the data from transaction with specific cardNbr
                List<BsonDocument> list = Mongo.Instance.GetSpecificTransactions("5656");
            //Datatable for the transaction
            DataTable dt = new DataTable();
                dt.Columns.Add("Card Number", typeof(string));
                dt.Columns.Add("Transaction Date", typeof(string));
                dt.Columns.Add("Amount", typeof(string));

                DataRow dr;
            //create the table
                foreach (var transaction in list)
                {
                    dr = dt.NewRow();
                    dr["Card Number"] = transaction["CardNumber"];
                    dr["Transaction Date"] = transaction["TransactionDate"];
                    string s = $"{transaction["TransactionAmount"]:0.##}";
                    dr["Amount"] = s;
                    dt.Rows.Add(dr);
                }

                transaction.DataSource = dt;
                transaction.DataBind();
                ViewState["table1"] = dt;
        }

        //Method that contacts the Mongo class to get the customers information
        private void getCustomers()
        {
            //call the Mongo class to get the customer data
            List<BsonDocument> cardholders = Mongo.Instance.GetCardHolders();
            //Datatable for the customers
            DataTable dt = new DataTable();
            dt.Columns.Add("Card Number", typeof(string));
            dt.Columns.Add("Holder's Name", typeof(string));
            dt.Columns.Add("Expiry Date", typeof(string));

            DataRow dr;
            //create the table
            foreach (var holder in cardholders)
            {
                dr = dt.NewRow();
                dr["Card Number"] = holder["CardNumber"];
                dr["Holder's Name"] = holder["Holder"];
                dr["Expiry Date"] = holder["ExpiryDate"];
                dt.Rows.Add(dr);
            }

            customer.DataSource = null;
            customer.DataBind();
            dt = RemoveDuplicateRows(dt, "Card Number");
            customer.DataSource = dt;
            customer.DataBind();
            ViewState["table2"] = dt;
        }

        //Make sure that the datatables does not contain duplicates of the data recived
        public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);
            return dTable;
        }
        
        

    }
}
