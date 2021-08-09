using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace IliadNET
{
    public class Iliad : IIliad
    {
        private readonly string _username;
        private readonly string _password;

        public Iliad(string username, string password) 
        {
            _username = username;
            _password = password;
        }

        public CustomerData GetInfo()
        {
            var token = GetToken();
            var rawData = GetRawData(token, _username, _password);

            var doc = new HtmlDocument();
            doc.LoadHtml(rawData);

            var value = doc.DocumentNode
                .Descendants(0)
                .FirstOrDefault(d => d.HasClass("current-user__infos"));

            var endOfferta = doc.DocumentNode
                .Descendants(0)
                .FirstOrDefault(d => d.HasClass("end_offerta"));

            var nationalTraffic = doc.DocumentNode
                .Descendants(0)
                .FirstOrDefault(d => d.HasClass("conso-local"));

            var chiamateTime = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[4]/div[1]/div[1]/div[1]/div[1]/span[1]").InnerText;
            var chiamateCost = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[4]/div[1]/div[1]/div[1]/div[1]/span[2]").InnerText;
            var smsCount = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[4]/div[1]/div[2]/div[1]/div[1]/span[1]").InnerText;
            var smsCost = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[4]/div[1]/div[2]/div[1]/div[1]/span[2]").InnerText;
            var internetUsed = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[4]/div[2]/div[1]/div[1]/div[1]/span[1]").InnerText;
            var internetTot = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[4]/div[2]/div[1]/div[1]/div[1]").ChildNodes[2].InnerText.Split(' ').LastOrDefault();
            var internetCost = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[4]/div[2]/div[1]/div[1]/div[1]/span[2]").InnerText;
            var mmsCount = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[4]/div[2]/div[2]/div[1]/div[1]/span[1]").InnerText;
            var mmsCost = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[4]/div[2]/div[2]/div[1]/div[1]/span[2]").InnerText;

            var subscription = doc.DocumentNode
                .Descendants(0)
                .FirstOrDefault(d => d.HasClass("p-conso"))
                .ChildNodes[1]
                .ChildNodes[1]
                .InnerText;

            var roamingTraffic = doc.DocumentNode
                .Descendants(0)
                .FirstOrDefault(d => d.HasClass("conso-roaming"));

            var chiamateTimeRoaming = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[5]/div[1]/div[1]/div[1]/div[1]/span[1]").InnerText;
            var chiamateCostRoaming = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[5]/div[1]/div[1]/div[1]/div[1]/span[2]").InnerText;
            var smsCountRoaming = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[5]/div[1]/div[2]/div[1]/div[1]/span[1]").InnerText;
            var smsCostRoaming = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[5]/div[1]/div[2]/div[1]/div[1]/span[2]").InnerText;
            var internetUsedRoaming = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[5]/div[2]/div[1]/div[1]/div[1]/span[1]").InnerText;
            var internetTotRoaming = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[5]/div[2]/div[1]/div[1]/div[1]").ChildNodes[2].InnerText.Split(' ').LastOrDefault();
            var internetCostRoaming = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[5]/div[2]/div[1]/div[1]/div[1]/span[2]").InnerText;
            var mmsCountRoaming = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[5]/div[2]/div[2]/div[1]/div[1]/span[1]").InnerText;
            var mmsCostRoaming = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[5]/div[2]/div[2]/div[1]/div[1]/span[2]").InnerText;


            return new CustomerData
            {
                FullName = value.ChildNodes[1].InnerText,
                Id = GetAfter(value.ChildNodes[3].InnerText, ':'),
                TelephoneNumber = GetAfter(value.ChildNodes[5].InnerText, ':'),
                EndSubscription = GetEndOffertaClean(endOfferta.InnerText),
                SubscriptionName = subscription,
                National = new Traffic
                {
                    CallTime = chiamateTime,
                    CallCost = chiamateCost,
                    SmsSent = smsCount,
                    SmsCost = smsCost,
                    InternetUsed = internetUsed,
                    InternetTotal = internetTot,
                    InternetCost = internetCost,
                    MmsSent = mmsCount,
                    MmsCost = mmsCost
                },
                Roaming = new Traffic
                {
                    CallTime = chiamateTimeRoaming,
                    CallCost = chiamateCostRoaming,
                    SmsSent = smsCountRoaming,
                    SmsCost = smsCostRoaming,
                    InternetUsed = internetUsedRoaming,
                    InternetTotal = internetTotRoaming,
                    InternetCost = internetCostRoaming,
                    MmsSent = mmsCountRoaming,
                    MmsCost = mmsCostRoaming
                }
            };
        }

        private string GetAfter(string text, char symbol)
        {
            return text.Trim()
                .Split(symbol)
                .LastOrDefault()
                .Replace(" ", "");
        }

        private string GetEndOffertaClean(string text)
        {
            return text.Trim()
                .Split(' ')
                .LastOrDefault();
        }

        private string GetRawData(string token, string username, string password)
        {
            HttpWebRequest httpWebRequest = CreateWebRequest("https://www.iliad.it/account/");
            httpWebRequest.Method = "POST";
            var data = $"login-ident={username}&login-pwd={password}";
            httpWebRequest.ContentLength = data.Length;
            httpWebRequest.Headers.Set("Cookie", token);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            using (var stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
            using (var responseStream = new StreamReader(response.GetResponseStream()))
            {
                return responseStream.ReadToEnd();
            }
        }

        private string GetToken()
        {
            HttpWebRequest httpWebRequest = CreateWebRequest("https://www.iliad.it/account/");
            httpWebRequest.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                return response.Headers["Set-Cookie"].Split(';')[0];
            }

        }

        private HttpWebRequest CreateWebRequest(string url)
        {
            return (HttpWebRequest)WebRequest.Create(url);
        }
    }
}
