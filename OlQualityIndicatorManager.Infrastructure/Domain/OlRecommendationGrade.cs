using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlRecommendationGrade : BindableBase
    {
        private string id;
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string name;
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
    }
}
