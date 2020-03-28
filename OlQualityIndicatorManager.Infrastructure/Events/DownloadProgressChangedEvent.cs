using OlQualityIndicatorManager.Infrastructure.Domain;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Events
{
    public class DownloadProgressEventArgs
    {
        public double Progress { get; set; }
        public string ItemName { get; set; }
        public string GroupName { get; set; }

        public DownloadProgressEventArgs()
        {
            Progress = 0;
            ItemName = "";
            GroupName = "";
        }
    }

    public class DownloadProgressChangedEvent : PubSubEvent<DownloadProgressEventArgs> { }
}
