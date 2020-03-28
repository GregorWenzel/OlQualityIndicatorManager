using Newtonsoft.Json;
using OlQualityIndicatorManager.Infrastructure.Converters;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlBase: BindableBase
    {
        private long idKey = -1;
        public long IdKey
        {
            get => idKey;
            set => SetProperty(ref idKey, value);
        }

        private string uid;
        [JsonProperty(PropertyName="uid")]
        public string Uid
        {
            get => uid;
            set => SetProperty(ref uid, value);
        }

        private string id;
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string title;
        [JsonProperty(PropertyName = "title")]
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string state;
        [JsonProperty(PropertyName = "state")]
        public string State
        {
            get => state;
            set => SetProperty(ref state, value);
        }

        private string url;
        [JsonProperty(PropertyName = "url")]
        public string Url
        {
            get => url;
            set => SetProperty(ref url, value);
        }

        private string type;
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

    }
}
