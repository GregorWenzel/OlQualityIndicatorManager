using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlAbbreviation: OlBase
    {
        private string explanation;
        [JsonProperty(PropertyName = "explanation")]
        public string Explanation
        {
            get => explanation;
            set => SetProperty(ref explanation, value);
        }

    }
}
