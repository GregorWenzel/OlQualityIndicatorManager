using Newtonsoft.Json;
using OlQualityIndicatorManager.Infrastructure.Converters;
using OlQualityIndicatorManager.Infrastructure.Helpers;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Domain
{
    public enum OlQualityIndicatorType
    {
        NONE,
        STRUCTURAL,
        PROCEDURAL,
        OUTCOME
    };

    [JsonConverter(typeof(JsonPathConverter))]
    public class OlQualityIndicator : OlBase
    {
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

        private OlGuideline guideline;
        [JsonProperty(PropertyName = "guideline")]
        public OlGuideline Guideline
        {
            get => guideline;
            set => SetProperty(ref guideline, value);
        }

        [JsonProperty(PropertyName = "guideline.uid")]
        public string GuidelineUid {get;set;}

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

        public string IndicatorTypeName
        {
            get
            {
                switch (IndicatorType)
                {
                    case OlQualityIndicatorType.OUTCOME:
                        return "Ergebnisindikator";
                    case OlQualityIndicatorType.PROCEDURAL:
                        return "Prozessindikator";
                    case OlQualityIndicatorType.STRUCTURAL:
                        return "Strukturindiaktor";
                    default:
                        return "N/A";
                }
            }

        }

        public string GuidelineName
        {
            get => Guideline.Title;
        }


    }
}
