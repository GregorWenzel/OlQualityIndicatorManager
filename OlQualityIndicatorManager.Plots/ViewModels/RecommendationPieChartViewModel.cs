using OlQualityIndicatorManager.Infrastructure.Domain;
using OlQualityIndicatorManager.Infrastructure.Events;
using OlQualityIndicatorManager.Plots.Domain;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Plots.ViewModels
{
    public class RecommendationPieChartViewModel : BindableBase
    {
        Dictionary<string, string> recommendationsByGradeDict = new Dictionary<string, string>()
        {
            { "Starke Empfehlungen", "a" },
            { "Schwache Empfehlungen", "b" },
            { "Offene Empfehlungen", "0" },
        };

        IEventAggregator eventAggregator;

        private ObservableCollection<PieChartDataItem> pieChartData;
        public ObservableCollection<PieChartDataItem> PieChartData
        {
            get => pieChartData;
            set => SetProperty(ref pieChartData, value);
        }

        public RecommendationPieChartViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<GuidelinesSelectedEvent>().Subscribe(OnGuidelinesSelected, true);
        }

        private void OnGuidelinesSelected(IEnumerable<OlGuideline> guidelineList)
        {
            List<PieChartDataItem> buffer = new List<PieChartDataItem>();
            
            foreach (KeyValuePair<string, string> keyValuePair in recommendationsByGradeDict)
            {
                PieChartDataItem item = new PieChartDataItem();
                item.Name = keyValuePair.Key;
                item.Value = guidelineList.Select(gItem => gItem.RecommendationList).Sum(rItem => rItem.Count(sItem => sItem.RecommendationGrade.Id == keyValuePair.Value));
                buffer.Add(item);
            }

            PieChartData = new ObservableCollection<PieChartDataItem>(buffer);
        }
    }
}
