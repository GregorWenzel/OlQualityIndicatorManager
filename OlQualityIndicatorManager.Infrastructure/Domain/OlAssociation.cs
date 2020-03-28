using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OlQualityIndicatorManager.Infrastructure.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlAssociation: OlBase
    {
        private string email;
        [JsonProperty(PropertyName = "email")]
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string abbreviation;
        [JsonProperty(PropertyName = "abbreviation")]
        public string Abbreviation
        {
            get => abbreviation;
            set => SetProperty(ref abbreviation, value);
        }

        private string phone;
        [JsonProperty(PropertyName = "phone")]
        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        private string street;
        [JsonProperty(PropertyName = "street")]
        public string Street
        {
            get => street;
            set => SetProperty(ref street, value);
        }

        private string postalCode;
        [JsonProperty(PropertyName = "postal_code")]
        public string PostalCode
        {
            get => postalCode;
            set => SetProperty(ref postalCode, value);
        }

        private OlImage logo;
        [JsonProperty(PropertyName = "logo")]
        public OlImage Logo
        {
            get => logo;
            set => SetProperty(ref logo, value);
        }

        private string place;
        [JsonProperty(PropertyName = "place")]
        public string Place
        {
            get => place;
            set => SetProperty(ref place, value);
        }

        private string type;
        [JsonProperty(PropertyName = "type")]
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        private string websiteUrl;
        [JsonProperty(PropertyName = "website_url")]
        public string WebsiteUrl
        {
            get => websiteUrl;
            set => SetProperty(ref websiteUrl, value);
        }

    }
}
