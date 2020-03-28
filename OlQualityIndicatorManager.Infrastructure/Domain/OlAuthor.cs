using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlAuthor: OlBase
    {
        private OlFile coIFile;
        [JsonProperty(PropertyName = "conflicts_of_interest")]
        public OlFile CoIFile
        {
            get => coIFile;
            set => SetProperty(ref coIFile, value);
        }
        
        private string firstName;
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string lastName;
        [JsonProperty(PropertyName = "lastname")]
        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private string postTitle;
        [JsonProperty(PropertyName = "post_title")]
        public string PostTitle
        {
            get => postTitle;
            set => SetProperty(ref postTitle, value);
        }

        private string preTitle;
        [JsonProperty(PropertyName = "pra_title")]
        public string PreTitle
        {
            get => preTitle;
            set => SetProperty(ref preTitle, value);
        }


    }
}
