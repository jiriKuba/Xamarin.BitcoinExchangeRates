using Bitcoin.Curses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Services
{
    public class NetworkService : INetworkService
    {
        private const string CheckUrl = "http://google.com";

        public Task<bool> IsInternetAvailable()
        {
            return Task.Run<bool>(() =>
            {
                try
                {
                    var iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);
                    //iNetRequest.ContinueTimeout = 5000;

                    using (var response = iNetRequest.GetResponseAsync().Result)
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}