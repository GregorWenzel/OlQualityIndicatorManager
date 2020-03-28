using OlQualityIndicatorManager.Infrastructure.Domain;
using OlQualityIndicatorManager.Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Grid.ViewModels
{
    public class GridViewModel : BindableBase
    {
        IRegionManager regionManager;

        private ObservableCollection<OlQualityIndicator> qualityIndicatorList;
        public ObservableCollection<OlQualityIndicator> QualityIndicatorList
        {
            get => qualityIndicatorList;
            set => SetProperty(ref qualityIndicatorList, value);
        }

        public GridViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<QualityIndicatorsLoadedEvent>().Subscribe(OnQualityIndicatorsLoaded, true);
        }

        private void OnQualityIndicatorsLoaded(IEnumerable<OlQualityIndicator> olQualityIndicators)
        {
            QualityIndicatorList = new ObservableCollection<OlQualityIndicator>(olQualityIndicators);
        }
    }
}
