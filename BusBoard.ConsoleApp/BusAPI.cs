﻿using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using System.Linq;

namespace BusBoard.ConsoleApp
{
    public class BusAPI
    {
        public RestClient client = new RestClient();

        public BusAPI()
        {
            // create Rest client with TFL API URL
            client.BaseUrl = new Uri("https://api.tfl.gov.uk");

        }

        public List<ArrivalInfo> RequestArrivalInfo(string stopCode)
        {
            // Create request
            var request = new RestRequest
            {
                Resource = "StopPoint/" + stopCode + "/Arrivals"
            };
            var response = client.Execute<List<ArrivalInfo>>(request);

            return response.Data;
        }

        public List<string> GetArrivalInfoAsString(string stopCode, int maxNumber)
        {
            // Request arrival info
            List<ArrivalInfo> arrivals = RequestArrivalInfo(stopCode);

            // check if max number requested is greater than number of elements returned
            int n = (maxNumber > arrivals.Count) ? arrivals.Count : maxNumber;

            // concatenate each row of info into a single string
            return arrivals.Take(n)
                           .OrderBy(a => a.TimeToStation)
                           .Select(a => a.LineName + " - " + a.DestinationName + " - " + a.TimeToStationInMins)
                           .ToList();
        }

        public List<StopPoint> RequestStopInfo(decimal lat, decimal lon)
        {
            List<string> stopTypes = RequestStopType();
            string s =string.Join(",", stopTypes);
            // Create request
            var request = new RestRequest
            {
                Resource = "StopPoint/?stopTypes=" + s + "&lat=" + lat.ToString() + "&lon=" + lon.ToString() + "&modes=bus",
                RootElement = "stopPoints"
            };
            var response = client.Execute<List<StopPoint>>(request);
            return response.Data;
        }

        public List<string> RequestStopType()
        {
            // Create request
            var request = new RestRequest
            {
                Resource = "StopPoint/Meta/StopTypes"
            };
            var response = client.Execute<List<string>>(request);

            return response.Data;
        }
    }
}