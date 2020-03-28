using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlInvolvedAssociation: BindableBase
    {
        private OlAssociation association;
        [JsonProperty(PropertyName = "professionalsociety")]
        public OlAssociation Association
        {
            get => association;
            set => SetProperty(ref association, value);
        }

        private List<OlAuthor> authorList;
        [JsonProperty(PropertyName = "authors")]
        public List<OlAuthor> AuthorList
        {
            get => authorList;
            set => SetProperty(ref authorList, value);
        }

        private string timeSpan;
        [JsonProperty(PropertyName = "tiemspan")]
        public string TimeSpan
        {
            get => timeSpan;
            set => SetProperty(ref timeSpan, value);
        }

    }
}
