using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlWorkgroup: OlBase
    {
        private List<OlAuthor> memberList;
        [JsonProperty(PropertyName = "author_members")]
        public List<OlAuthor> MemberList
        {
            get => memberList;
            set => SetProperty(ref memberList, value);
        }

        private List<OlAuthor> managerList;
        [JsonProperty(PropertyName = "author_managers")]
        public List<OlAuthor> ManagerList
        {
            get => managerList;
            set => SetProperty(ref managerList, value);
        }


    }
}
