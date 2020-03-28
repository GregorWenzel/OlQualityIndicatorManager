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
    public class GuidelineGridViewModel : BindableBase
    {
        IRegionManager regionManager;

        private ObservableCollection<OlGuideline> guidelineList;
        public ObservableCollection<OlGuideline> GuidelineList
        {
            get => guidelineList;
            set => SetProperty(ref guidelineList, value);
        }

        public GuidelineGridViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<GuidelinesLoadedEvent>().Subscribe(OnGuidelinesLoaded, true);
        }

        private void OnGuidelinesLoaded(IEnumerable<OlGuideline> guidelineList)
        {
            GuidelineList = new ObservableCollection<OlGuideline>(guidelineList);
        }
    }
}
