using BusBoard.Api;
using System.Collections.Generic;

namespace BusBoard.Web.ViewModels
{
  public class BusInfo
  {
        public BusInfo(string postCode)
        {
            PostCode = postCode;
        }
        public string PostCode { get; set; }
        public List<BusArrivalInfo> Stops { get; set; }

        public BusInfo()
        {
            Stops = new List<BusArrivalInfo>();
        }

    }
}