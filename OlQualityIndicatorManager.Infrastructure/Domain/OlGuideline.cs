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
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }

    public class OlGuideline: OlBase
    {
        private DateTime reviewDate;
        [JsonProperty(PropertyName = "review_date")]
        [JsonConverter(typeof(DateFormatConverter), "dd.MM.yyyy")]
        public DateTime ReviewDate
        {
            get => reviewDate;
            set => SetProperty(ref reviewDate, value);
        }

        private string financing;
        [JsonProperty(PropertyName="guideline_financing")]
        public string Financing
        {
            get => financing;
            set => SetProperty(ref financing, value);
        }

        private string awmfId;
        [JsonProperty(PropertyName = "awf_registernumber")]
        public string AwmfId
        {
            get => awmfId;
            set => SetProperty(ref awmfId, value);
        }

        private string statements;
        [JsonProperty(PropertyName = "statements")]
        public string Statements
        {
            get => statements;
            set => SetProperty(ref statements, value);
        }

        private string state;
        [JsonProperty(PropertyName = "state")]
        public string State
        {
            get => state;
            set => SetProperty(ref state, value);
        }


        private string category;
        [JsonProperty(PropertyName = "category")]
        public string Category
        {
            get => category;
            set => SetProperty(ref category, value);
        }

        private string previousChanges;
        [JsonProperty(PropertyName = "previous_changes")]
        public string PreviousChanges
        {
            get => previousChanges;
            set => SetProperty(ref previousChanges, value);
        }
        
        private string schemeEvidenceGraduation;
        [JsonProperty(PropertyName = "scheme_of_evidencegraduation_sign")]
        public string SchemeEvidenceGraduation
        {
            get => schemeEvidenceGraduation;
            set => SetProperty(ref schemeEvidenceGraduation, value);
        }

        private string expertConsensus;
        [JsonProperty(PropertyName = "expert_consensus")]
        public string ExpertConsensus
        {
            get => expertConsensus;
            set => SetProperty(ref expertConsensus, value);
        }

        private List<OlAssociation> leadAssociationList;
        [JsonProperty(PropertyName = "lead_assosication")]
        public List<OlAssociation> LeadAssociationList
        {
            get => leadAssociationList;
            set => SetProperty(ref leadAssociationList, value);
        }

        private List<OlInvolvedAssociation> associationList;
        [JsonProperty(PropertyName = "involved_professionalsocieties")]
        public List<OlInvolvedAssociation> AssociationList
        {
            get => associationList;
            set => SetProperty(ref associationList, value);
        }

        private List<OlWorkgroup> workgroupList;
        [JsonProperty(PropertyName = "workgroups")]
        public List<OlWorkgroup> WorkgroupList
        {
            get => workgroupList;
            set => SetProperty(ref workgroupList, value);
        }

        private string version;
        [JsonProperty(PropertyName = "version")]
        public string Version
        {
            get => version;
            set => SetProperty(ref version, value);
        }

        private string specialAdvice;
        [JsonProperty(PropertyName = "special_advise")]
        public string SpecialAdvice
        {
            get => specialAdvice;
            set => SetProperty(ref specialAdvice, value);
        }

        private List<OlAbbreviation> abbreviationList;
        [JsonProperty(PropertyName = "abbreviation_references")]
        public List<OlAbbreviation> AbbreviationList
        {
            get => abbreviationList;
            set => SetProperty(ref abbreviationList, value);
        }

        private string email;
        [JsonProperty(PropertyName = "email")]
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string addressDetails;
        [JsonProperty(PropertyName = "address_details")]
        public string AddressDetails
        {
            get => addressDetails;
            set => SetProperty(ref addressDetails, value);
        }

        private string basisOfMethos;
        [JsonProperty(PropertyName = "basis_of_methods")]
        public string BasisOfMethods
        {
            get => basisOfMethos;
            set => SetProperty(ref basisOfMethos, value);
        }

        private string contractorOfGuidelineGroup;
        [JsonProperty(PropertyName = "contractor_of_guidelinegroup")]
        public string ContractorOfGuidelineGroup
        {
            get => contractorOfGuidelineGroup;
            set => SetProperty(ref contractorOfGuidelineGroup, value);
        }

        private List<OlSubsection> subsectionList;
        [JsonProperty(PropertyName = "subsections")]
        public List<OlSubsection> SubsectionList
        {
            get => subsectionList;
            set => SetProperty(ref subsectionList, value);
        }

        private List<OlSubsection> recommendationList;
        public List<OlSubsection> RecommendationList
        {
            get => recommendationList;
            set => SetProperty(ref recommendationList, value);
        }

        private List<OlSubsection> qualityIndicatorList;
        public List<OlSubsection> QualityIndicatorList
        {
            get => qualityIndicatorList;
            set => SetProperty(ref qualityIndicatorList, value);
        }

        private string shortTitle;
        [JsonProperty(PropertyName = "short_title")]
        public string ShortTitle
        {
            get => shortTitle;
            set => SetProperty(ref shortTitle, value);
        }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public string Language
        {
            get
            {
                string[] buffer = this.ShortTitle.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (buffer.Last().Length != 2 || !Helpers.HelperFunctions.IsAllUpper(buffer.Last()))
                {
                    return "DE";
                }
                else
                {
                    return buffer.Last();
                }
            }
        }


        public int RecommendationCount
        {
            get => RecommendationList.Count;
        }

        public int QualityIndicatorCount
        {
            get => QualityIndicatorList.Count;
        }

        public void GetRecommendationList()
        {
            RecommendationList = new List<OlSubsection>();

            List<OlSubsection> chapterList = SubsectionList.Where(item => item.Type == "ChapterCT" && item.Title != "Methodiken").OrderBy(item => item.TypePosition).ToList();

            for (int i=0; i<chapterList.Count; i++)
            {
                OlSubsection subsection = chapterList[i];
                int index = 1;
                subsection.GetRecommendationList(ref recommendationList, i + 3, ref index);
            }

            RaisePropertyChanged("RecommendationCount");
        }

        public void GetQualityIndicatorList()
        {
            QualityIndicatorList = new List<OlSubsection>();

            foreach (OlSubsection subsection in SubsectionList)
            {
                subsection.GetQualityIndicatorList(ref qualityIndicatorList);
            }

            RaisePropertyChanged("QualityIndicatorCount");
        }

        public List<OlSubsection> GetRecommendationByType(string Grade, string Type)
        {
            if (!string.IsNullOrEmpty(Grade) && !string.IsNullOrEmpty(Type))
            {
                return RecommendationList.Where(item => item.RecommendationGrade.Id == Grade && item.RecommendationType.Id == Type).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(Grade))
                {
                    return RecommendationList.Where(item => item.RecommendationGrade.Id == Grade).ToList();
                }
                else if (!string.IsNullOrEmpty(Type))
                {
                   return RecommendationList.Where(item => item.RecommendationType.Id == Type).ToList();
                }
                else
                {
                    return RecommendationList.Where(item => item.RecommendationGrade.Id == Grade && item.RecommendationType.Id == Type).ToList();

                }
            }
        }

        public int GetRecommendationCountByType(string Grade, string Type)
        {
            //Grades: a, b, 0
            //Types: consensbased-statement, consensbased-recommendation, evidencebased-statement, evidencebased-recommendation


            if (!string.IsNullOrEmpty(Grade) && !string.IsNullOrEmpty(Type))
            {
                return RecommendationList.Count(item => item.RecommendationGrade.Id == Grade && item.RecommendationType.Id == Type);
            }
            else
            {
                if (!string.IsNullOrEmpty(Grade))
                {
                    return RecommendationList.Count(item => item.RecommendationGrade.Id == Grade);
                }
                else if (!string.IsNullOrEmpty(Type))
                {
                    return RecommendationList.Count(item => item.RecommendationType.Id == Type);
                }
                else
                {
                    return RecommendationList.Count(item => item.RecommendationGrade.Id == Grade && item.RecommendationType.Id == Type);
                }
            }
        }

        public Dictionary<(string, string), List<OlSubsection>> GetRecommendationsByGroup()
        {
            Dictionary<(string, string), List<OlSubsection>> result = new Dictionary<(string, string), List<OlSubsection>>();

            foreach (OlSubsection recommendation in RecommendationList)
            {
                List<OlSubsection> buffer;

                if (result.ContainsKey((recommendation.RecommendationGrade.Id, recommendation.RecommendationType.Id)))
                {
                    buffer = result[(recommendation.RecommendationGrade.Id, recommendation.RecommendationType.Id)];
                    buffer.Add(recommendation);
                }
                else
                {
                    buffer = new List<OlSubsection>();
                    buffer.Add(recommendation);
                    result.Add((recommendation.RecommendationGrade.Id, recommendation.RecommendationType.Id), buffer);
                }               
            }

            return result;
        }

        //Fehlend:
        //"coordination_editiorial": []
    }
}
