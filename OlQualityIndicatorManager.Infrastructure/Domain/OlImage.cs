using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlImage: OlBase
    {
        private string file;
        [JsonProperty(PropertyName = "file")]
        public string File
        {
            get => file;
            set => SetProperty(ref file, value);
        }

        private string filename;
        [JsonProperty(PropertyName = "filename")]
        public string Filename
        {
            get => filename;
            set => SetProperty(ref filename, value);
        }

        private OlSize size;
        [JsonProperty(PropertyName = "size")]
        public OlSize Size
        {
            get => size;
            set => SetProperty(ref size, value);
        }

    }
}
