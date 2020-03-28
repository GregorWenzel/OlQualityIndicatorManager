using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlRecommendationLevelOfEvidence : BindableBase
    {
        private string comment;
        [JsonProperty(PropertyName = "comment")]
        public string Comment
        {
            get => comment;
            set => SetProperty(ref comment, value);
        }

        private OlRecommendationLevelOfEvidenceData data;
        [JsonProperty(PropertyName = "level_of_evidence")]
        public OlRecommendationLevelOfEvidenceData Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }
    }
}
