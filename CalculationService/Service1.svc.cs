using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace interfaceMethods
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        //lats, longs, rates and costRedutcion depending on travellerType
        private double[] latitudes = { 59.6519, 55.6181, 49.0097, 51.4707, 50.1167 };
        private double[] longitudes = { 17.9186, 12.6561, 2.5478, -0.4543, 8.6833 };
        private double[] rates = { 0.234, 0.2554, 0.2255, 0.2300, 0.2400 };
        private double[] travellerType = { 0.9, 0.33, 0.25, 0 };

        //Method that converts degrees to radians
        public double degreeToRadians(double angleDegrees)
        {
            return (Math.PI / 180) * angleDegrees;
        }

        //This method checks to see wether the sent in data is either a latitude or longitude 
        private double getLatLongitude(int a, char latorlong)
        {
            if (latorlong == 'L') return latitudes[a];
            return longitudes[a];
        }

        //calculation for distance between destinations 
        private double getFlightDistance(int origin, int destination)
        {
            double earthRadius = 6371.0;
            int i;
            double latitudeFrom, longitudeFrom, latitudeTo, longitudeTo;
            latitudeFrom = getLatLongitude(origin, 'L');
            longitudeFrom = getLatLongitude(origin, 'G');
            latitudeTo = getLatLongitude(destination, 'L');
            longitudeTo = getLatLongitude(destination, 'G');
            if ((latitudeFrom < -999) || (longitudeFrom < -999) || (latitudeTo < -999) || (longitudeTo < -999)) return -1;

            double x1 = degreeToRadians(latitudeFrom);
            double y1 = degreeToRadians(longitudeFrom);
            double x2 = degreeToRadians(latitudeTo);
            double y2 = degreeToRadians(longitudeTo);

            // great circle distance in radians
            double centralAngle = Math.Acos(Math.Sin(x1) * Math.Sin(x2) + Math.Cos(x1) * Math.Cos(x2) * Math.Cos(y1 - y2));
            double distanceXY = earthRadius * centralAngle;
            //return the distance so that ticket prices can be calculated
            return distanceXY;
        }

       

        //this method will calculate the totalcost of the flight
        public double flightCost(string json)
        {
            //take out the information
            JObject flightJson = (JObject)JsonConvert.DeserializeObject(json);
            int from = (int)flightJson["from"];
            int to = (int)flightJson["to"];
            int infants = (int)flightJson["infants"];
            int children = (int)flightJson["children"];
            int adults = (int)flightJson["adults"];
            int oldies = (int)flightJson["oldies"];
            double totalCost = 0;
            double distance = getFlightDistance(from, to);

            //take each traveller group and calculate their ticket price then add to the totalCost double
            for (int i = 0; i < infants; i++)
            {
                totalCost += rates[to] * distance * (1 - travellerType[0]);
            }
            for (int i = 0; i < children; i++)
            {
                totalCost += rates[to] * distance * (1 - travellerType[1]);
            }
            for (int i = 0; i < adults; i++)
            {
                totalCost += rates[to] * distance * (1 - travellerType[3]);
            }
            for (int i = 0; i < oldies; i++)
            {
                totalCost += rates[to] * distance * (1 - travellerType[2]);
            }
            //return the calculated price for the flight cost
            return totalCost;
        }

        //THis method calculates the price of the hotel
        public double hotelCost(string json)
        {
            //take out the information
            JObject hotelJson = (JObject)JsonConvert.DeserializeObject(json);
            int room = (int)hotelJson["roomType"];
            int seniors = (int)hotelJson["seniors"];
            int nights = (int)hotelJson["nights"];
            int travellers = (int)hotelJson["travellers"];

            double totalCost = room * travellers;

            if (seniors > 0)
            {
                totalCost += (room * seniors) / 2;
            }

            //returns the cost * nbr of nights
            return totalCost *= nights;
        }

        public double carCost(string json)
        {
            JObject car = (JObject)JsonConvert.DeserializeObject(json);
            int year = (int)car["modelYear"];
            int seats = (int)car["seats"];
            int age = (int)car["age"];
            int fuel = (int)car["fuelType"];

            int a = 0;
            int b = 0;
            double totalCost = 0;

            if (year == 0 || seats == 0 || age == 0)
            {
                return 0;
            }

            if (seats <= 5)
            {
                a = (year - 2010) * 600;
            }
            else
            {
                a = (year - 2010) * 900;
            }

            if (age < 45)
            {
                b = (50 - age) * 200;
            }
            else
            {
                b = (age - 40) * 220;
            }

            totalCost = a + b;

            if (fuel == 1)
            {
                totalCost *= 1.2;
                //Debug.WriteLine("Car: " + totalCost);
            }

            return totalCost;
        }

        //calculate the total cost of both the flight and hotel prices
        public double totalCost(string json)
        {
            //take out the information and simply add the flightCost and hotelCost for the return value
            JObject paymentJson = (JObject)JsonConvert.DeserializeObject(json);
            double flightCost = (double)paymentJson["flightCost"];
            double hotelCost = (double)paymentJson["hotelCost"];
            double carCost = (double)paymentJson["carCost"];

            return flightCost + hotelCost + carCost;
        }

        
    }
}
