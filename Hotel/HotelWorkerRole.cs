using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Auth;
using System.ServiceModel;
using System.ServiceModel.Description;
using Newtonsoft.Json.Linq;
using System.ServiceModel.Web;
using interfaceMethods;

//This workerrole is for the process of the hotel price


namespace Flight
{
    public class FlightWorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private string accountName = "lab3cloud";      // storage account name        
        private string accountKey = "2f0Hs+WbONYYKMVa8yGS9cu3Z2e6w+qA228whjpjo2enzmBFH7pjeFbt9VOznXN7JOqxr/47RRm0viQ3ImExYw==";    //key
        private StorageCredentials credentials;
        private CloudStorageAccount storageAcc;
        private CloudQueueClient queueClient;
        private CloudQueue inputqueue, outputqueue;
        private CloudQueueMessage iputnMessage, outputMessage;
        private WebServiceHost host;
        private ServiceEndpoint endPoint;

        //the following method is called at the start of the worker role to get instances of incoming and outgoing queues 
        private void initQueue()
        {
            //connect to the storage account
            credentials = new StorageCredentials(accountName, accountKey);
            storageAcc = new CloudStorageAccount(credentials, useHttps: true);

            // Create the queue client
            queueClient = storageAcc.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            inputqueue = queueClient.GetQueueReference("hotelqueue");

            // Create the queue if it doesn't already exist
            inputqueue.CreateIfNotExists();

            // Retrieve a reference to a queue
            outputqueue = queueClient.GetQueueReference("webqueue");

            // Create the queue if it doesn't already exist
            outputqueue.CreateIfNotExists();

            //creating a webservicehost
            host = new WebServiceHost(typeof(Service1), new Uri("http://localhost:8000/"));
            endPoint = host.AddServiceEndpoint(typeof(IService1), new WebHttpBinding(), "");
            try
            {
                host.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);

            }
        }

        public override void Run()
        {
            Trace.TraceInformation("Hotel is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            catch (Exception e) {; }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            bool result = base.OnStart();

            Trace.TraceInformation("Hotel has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("Hotel is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("Hotel has stopped");
        }
        //task to be run during the program uptime
        private async Task RunAsync(CancellationToken cancellationToken)
        {
            initQueue();
            double hotelPrice = 0;
            //loop for senging the information to the interface to perform the necesary calculations
            while (!cancellationToken.IsCancellationRequested)
            {
                iputnMessage = inputqueue.GetMessage();
                //when inMesage has a value other than null perform the code
                if (iputnMessage != null)
                {
                    //ChannelFactory so interface can be reached
                    using (ChannelFactory<IService1> cf =
                        new ChannelFactory<IService1>(new WebHttpBinding(), "http://localhost:8000"))
                    {
                        cf.Endpoint.Behaviors.Add(new WebHttpBehavior());

                        //go to interface and go to the totalCost method that adds the flightCost and hotelCost
                        IService1 @interface = cf.CreateChannel();
                        hotelPrice = @interface.hotelCost(iputnMessage.AsString);
                            
                        try
                        {
                            await inputqueue.DeleteMessageAsync(iputnMessage);
                        }
                        catch (Exception e)
                        {
                            iputnMessage = null;
                        }
                        //store the data in jObject
                        JObject jObject = new JObject { ["hotelCost"] = hotelPrice };

                        //send back data
                        outputMessage = new CloudQueueMessage(jObject.ToString());
                        outputqueue.AddMessage(outputMessage);
                    }
                }
            }

        }
    }
}
