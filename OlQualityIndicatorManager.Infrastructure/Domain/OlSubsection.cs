using Newtonsoft.Json;
using OlQualityIndicatorManager.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public class OlSubsection: OlBase
    {
        public long GuidelineId { get; set; }

        private List<OlSubsection> referenceRecommendationList;
        [JsonProperty(PropertyName = "reference_recommendations")]
        public List<OlSubsection> ReferenceRecommendationList
        {
            get => referenceRecommendationList;
            set => SetProperty(ref referenceRecommendationList, value);
        }

        private string numerator;
        [JsonProperty(PropertyName = "enumerator")]
        public string Numerator
        {
            get => numerator;
            set => SetProperty(ref numerator, HelperFunctions.GetPlainTextFromHtml(value));
        }

        private string denominator;
        [JsonProperty(PropertyName = "denominator")]
        public string Denominator
        {
            get => denominator;
            set => SetProperty(ref denominator, HelperFunctions.GetPlainTextFromHtml(value));
        }

        private string evidenceBasis;
        [JsonProperty(PropertyName = "evidence_basis")]
        public string EvidenceBasis
        {
            get => evidenceBasis;
            set => SetProperty(ref evidenceBasis, value);
        }

        private string annotations;
        [JsonProperty(PropertyName = "annotations")]
        public string Annotations
        {
            get => annotations;
            set => SetProperty(ref annotations, HelperFunctions.GetPlainTextFromHtml(value));
        }

        private OlQualityIndicatorType indicatorType;
        public OlQualityIndicatorType IndicatorType
        {
            get => indicatorType;
            set
            {
                SetProperty(ref indicatorType, value);
                RaisePropertyChanged("IndicatorTypeName");
            }
        }

        private int typePosition;
        [JsonProperty(PropertyName = "type_position")]
        public int TypePosition
        {
            get => typePosition;
            set => SetProperty(ref typePosition, value);
        }

        private int position;
        [JsonProperty(PropertyName = "position")]
        public int Position
        {
            get => position;
            set => SetProperty(ref position, value);
        }

        private List<OlSubsection> subsectionList;
        [JsonProperty(PropertyName = "subsections")]
        public List<OlSubsection> SubsectionList
        {
            get => subsectionList;
            set => SetProperty(ref subsectionList, value);
        }
        
        private DateTime creationDate;
        [JsonProperty(PropertyName = "recommendation_creation_date")]
        public DateTime CreationDate
        {
            get => creationDate;
            set => SetProperty(ref creationDate, value);
        }

        private OlConsensusStrength consensusStrength;
        [JsonProperty(PropertyName = "strength_of_consensus")]
        public OlConsensusStrength ConsensusStrength
        {
            get => consensusStrength;
            set => SetProperty(ref consensusStrength, value);
        }

        private string textEmpty;
        [JsonProperty(PropertyName = "text_empty")]
        public string TextEmpty
        {
            get => textEmpty;
            set => SetProperty(ref textEmpty, value);
        }

        private string number;
        [JsonProperty(PropertyName = "number")]
        public string Number
        {
            get => number;
            set => SetProperty(ref number, value);
        }

        private string editStateText;
        [JsonProperty(PropertyName = "edit_state_text")]
        public string EditStateText
        {
            get => editStateText;
            set => SetProperty(ref editStateText, value);
        }

        private OlRecommendationGrade recommendationGrade;
        [JsonProperty(PropertyName = "recommendation_grade")]
        public OlRecommendationGrade RecommendationGrade
        {
            get => recommendationGrade;
            set => SetProperty(ref recommendationGrade, value);
        }

        private string totalVote;
        [JsonProperty(PropertyName = "total_vote_in_percentage")]
        public string TotalVote
        {
            get => totalVote;
            set => SetProperty(ref totalVote, value);
        }

        private OlRecommendationType recommendationType;
        [JsonProperty(PropertyName = "type_of_recommendation")]
        public OlRecommendationType RecommendationType
        {
            get => recommendationType;
            set => SetProperty(ref recommendationType, value);
        }

        private string text;
        [JsonProperty(PropertyName = "text")]
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        private bool isExpertOpinion;
        [JsonProperty(PropertyName = "expert_opinion")]
        public bool IsExpertOpinion
        {
            get => isExpertOpinion;
            set => SetProperty(ref isExpertOpinion, value);
        }

        private OlRecommendationEditState editState;
        [JsonProperty(PropertyName = "edit_state")]
        public OlRecommendationEditState EditState
        {
            get => editState;
            set => SetProperty(ref editState, value);
        }

        private List<OlRecommendationLevelOfEvidence> levelOfEvidenceList;
        [JsonProperty(PropertyName = "level_of_evidences")]
        public List<OlRecommendationLevelOfEvidence> LevelOfEvidenceList
        {
            get => levelOfEvidenceList;
            set => SetProperty(ref levelOfEvidenceList, value);
        }

        public string ParentSubsectionUid { get; set; }
        
        [JsonProperty(PropertyName = "url_certification")]
        public string UrlCertification { get; set; }

        public void GetRecommendationList(ref List<OlSubsection> RecommendationList, int TypePosition, ref int runningIndex)
        {
            List<OlSubsection> subsections = SubsectionList.Where(item => item.Type == "RecommendationCT").OrderBy(item => item.TypePosition).ToList();

            foreach (OlSubsection subsection in subsections)
            {
                subsection.Number = $"{TypePosition}.{runningIndex}";
                RecommendationList.Add(subsection);
                if (subsection.RecommendationType.Id.ToLower().Contains("recommendation") && subsection.RecommendationGrade.Name == null)
                {
                    string text = HelperFunctions.GetPlainTextFromHtml(subsection.Text);
                    if ((Regex.Match(text, @"\bsoll\b", RegexOptions.IgnoreCase).Success) || (Regex.Match(text, @"\bsollen\b", RegexOptions.IgnoreCase).Success))
                    {
                        subsection.recommendationGrade.Id = "a";
                    }
                    else if ((Regex.Match(text, @"\bsollte\b", RegexOptions.IgnoreCase).Success) || (Regex.Match(text, @"\bsollten\b", RegexOptions.IgnoreCase).Success))
                    {
                        subsection.RecommendationGrade.Id = "b";
                    }
                    else if ((Regex.Match(text, @"\bkann\b", RegexOptions.IgnoreCase).Success) || (Regex.Match(text, @"\bkönnen\b", RegexOptions.IgnoreCase).Success))
                    {
                        subsection.RecommendationGrade.Id = "0";
                    }
                    else
                    {

                    }
                }
                runningIndex += 1;
            }

            subsections = SubsectionList.Where(item => item.Type == "ChapterCT").OrderBy(item => item.TypePosition).ToList();

            foreach (OlSubsection subsection in SubsectionList)
            {
                subsection.GetRecommendationList(ref RecommendationList, TypePosition, ref runningIndex);
            }
        }

        public void GetQualityIndicatorList(ref List<OlSubsection> QualityIndicatorList)
        {
            List<OlSubsection> subsections = SubsectionList.Where(item => item.Type == "QualityIndicatorCT").ToList();

            foreach (OlSubsection subsection in subsections)
            {
                subsection.ParentSubsectionUid = this.Uid;
                QualityIndicatorList.Add(subsection);
            }

            subsections = SubsectionList.Where(item => item.Type == "ChapterCT").ToList();

            foreach (OlSubsection subsection in SubsectionList)
            {
                subsection.ParentSubsectionUid = this.Uid;
                subsection.GetQualityIndicatorList(ref QualityIndicatorList);
            }
        }

    }
}
