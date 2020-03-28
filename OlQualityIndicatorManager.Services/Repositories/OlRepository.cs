using Newtonsoft.Json;
using OlQualityIndicatorManager.Infrastructure.Domain;
using OlQualityIndicatorManager.Infrastructure.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Services.Repositories
{
    public class OlRepository : OlRepositoryBase, IOlRepository
    {
        List<OlGuideline> guidelineList;

        WebClient webClient;
        IEventAggregator eventAggregator;
        string currentAction = string.Empty;
        OlGuideline nextGuideline;

        MySqlRepository mySqlRepository = new MySqlRepository();

        public OlRepository(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            webClient = new WebClient();
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
        }

        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            switch (currentAction)
            {
                case "GetQualityIndicatorList":
                    DeserializeQualityIndicatorList(e.Result);
                    break;
                case "GetGuidelineList":
                    DeserializeGuidelineList(e.Result);
                    break;
                case "GetGuideline":
                    if (e != null)
                    {
                        DeserializeGuideline(e.Result);
                    }
                    
                    if (guidelineList.Any(item => string.IsNullOrEmpty(item.Financing)))
                    {
                        nextGuideline = guidelineList.First(item => string.IsNullOrEmpty(item.Financing));
                        GetGuideline(nextGuideline.Id);
                    }

                    break;
            }
        }             

        private void DeserializeQualityIndicatorList(string jsonString)
        {
            List<OlQualityIndicator> result = new List<OlQualityIndicator>();
            List<OlQualityIndicator> buffer = JsonConvert.DeserializeObject<List<OlQualityIndicator>>(jsonString);

            foreach (OlQualityIndicator qi in buffer)
            {
                if (result.Any(item => item.Id == qi.Id) == false)
                {
                    if (buffer.Count(item => item.Id == qi.Id) > 1)
                    {
                        List<OlQualityIndicator> identicalItems = buffer.Where(item => item.Id == qi.Id).ToList().OrderBy(item => item.Uid).ToList();
                        result.Add(identicalItems.Last());
                    }
                }
            }

            eventAggregator.GetEvent<QualityIndicatorsLoadedEvent>().Publish(result);
        }

        private void DeserializeGuidelineList(string jsonString)
        {
            List<OlGuideline> buffer = JsonConvert.DeserializeObject<List<OlGuideline>>(jsonString);

            OlGuideline currentGuideline;

            foreach (OlGuideline qi in buffer)
            {
                if (guidelineList.Any(item => item.Id == qi.Id) == false)
                {
                    if (buffer.Count(item => item.Id == qi.Id) > 1)
                    {
                        List<OlGuideline> identicalItems = buffer.Where(item => item.Id == qi.Id).ToList().OrderBy(item => item.Uid).ToList();
                        currentGuideline = identicalItems.Last();

                    }
                    else
                    {
                        currentGuideline = qi;
                    }

                    guidelineList.Add(currentGuideline);
                }
            }

            //mySqlRepository.Open();
            //currentAction = "GetGuideline";
            //WebClient_DownloadStringCompleted(this, null);
            eventAggregator.GetEvent<GuidelinesLoadedEvent>().Publish(buffer);

        }

        private OlGuideline DeserializeGuideline(string jsonString)
        {            
            OlGuideline result = JsonConvert.DeserializeObject<OlGuideline>(jsonString); 
            result.GetRecommendationList();
            result.GetQualityIndicatorList();

            if (guidelineList != null)
            {
                int index = guidelineList.IndexOf(guidelineList.First(item => item.Uid == result.Uid));
                guidelineList[index] = result;
            }

            mySqlRepository.SaveGuideline(result);

            return result;
            //eventAggregator.GetEvent<QualityIndicatorsLoadedEvent>().Publish(result);
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressEventArgs ea = new DownloadProgressEventArgs();
            ea.Progress = e.ProgressPercentage;

            switch (currentAction)
            {
                case "GetQualityIndicatorList":
                    ea.GroupName = "Lade Qualitätsindikatoren...";
                    ea.ItemName = "";
                    break;
                case "GetGuidelineList":
                    ea.GroupName = "Lade Leitlinien...";
                    ea.ItemName = "";
                    break;
                case "GetGuideline":
                    ea.GroupName = "Lade Leitlinie...";
                    ea.ItemName = nextGuideline.Title;
                    break;
            }
            
            eventAggregator.GetEvent<DownloadProgressChangedEvent>().Publish(ea);
        }

        public void GetQualityIndicatorList()
        {
            currentAction = "GetQualityIndicatorList";
            DownloadProgressEventArgs ea = new DownloadProgressEventArgs();
            ea.Progress = 0;
            ea.GroupName = "Lade Qualitätsindikatoren...";

            eventAggregator.GetEvent<DownloadProgressChangedEvent>().Publish(ea);

            string URL = SERVER_URL + "get_qualityindicators";
            webClient.DownloadStringAsync(new Uri(URL));
        }

        public void GetGuidelineList()
        {
            guidelineList = new List<OlGuideline>();
            currentAction = "GetGuidelineList";
            DownloadProgressEventArgs ea = new DownloadProgressEventArgs();
            ea.Progress = 0;
            ea.GroupName = "Lade Leitlinien...";

            eventAggregator.GetEvent<DownloadProgressChangedEvent>().Publish(ea);

            string URL = SERVER_URL + "get_guidelines?version_id=public";
            webClient.DownloadStringAsync(new Uri(URL));           
        }

        public void GetGuideline(string id)
        {
            currentAction = "GetGuideline";
            DownloadProgressEventArgs ea = new DownloadProgressEventArgs();
            ea.Progress = 0;
            ea.GroupName = "Lade Leitlinie...";
            ea.ItemName = nextGuideline.Title;

            eventAggregator.GetEvent<DownloadProgressChangedEvent>().Publish(ea);

            string URL = SERVER_URL + $"get_guideline?id={id}";
            webClient.DownloadStringAsync(new Uri(URL));
        }

        public OlGuideline GetGuidelineSync(string id)
        {
            OlGuideline result;
            string URL = SERVER_URL + $"get_guideline?id={id}";
            string jsonString = webClient.DownloadString(new Uri(URL));
            return DeserializeGuideline(jsonString);
        }

        public void GetRecommendation(string url)
        {
            url = @"leitlinien/melanom/published/copy_of_diagnostik-und-therapie-in-der-primaerversorgung/primaerexzision/sicherheitsabstand-bei-primaerexzision/sicherheitsabstand1";
            string URL = SERVER_URL + url;

            string jsonString = new WebClient().DownloadString(URL);
        }
    }
}
