using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlFile: BindableBase
    {
        private string url;
        [JsonProperty(PropertyName = "url")]
        public string Url
        {
            get => url;
            set => SetProperty(ref url, value);
        }

        private string type;
        [JsonProperty(PropertyName = "file_type")]
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        private string fileSize;
        [JsonProperty(PropertyName = "file_size")]
        public string FileSize
        {
            get => fileSize;
            set => SetProperty(ref fileSize, value);
        }

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

    }
}
