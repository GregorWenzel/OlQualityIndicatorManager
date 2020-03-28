using OlQualityIndicatorManager.Infrastructure.Domain;
using OlQualityIndicatorManager.Infrastructure.Events;
using OlQualityIndicatorManager.Services.Repositories;
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
using System.Windows.Input;
using Unity;

namespace OlQualityIndicatorManager.Grid.ViewModels
{
    public class GuidelineSelectionViewModel : BindableBase
    {
        IRegionManager regionManager;
        IUnityContainer container;

        public ICommand UpdateCommand
        {
            get => new DelegateCommand<ObservableCollection<object>>(OnUpdate);

        }

        private ObservableCollection<OlGuideline> guidelineList;
        public ObservableCollection<OlGuideline> GuidelineList
        {
            get => guidelineList;
            set => SetProperty(ref guidelineList, value);
        }

        private ObservableCollection<OlQualityIndicator> qualityIndicatorList;
        public ObservableCollection<OlQualityIndicator> QualityIndicatorList
        {
            get => qualityIndicatorList;
            set => SetProperty(ref qualityIndicatorList, value);
        }

        public GuidelineSelectionViewModel(IUnityContainer container, IEventAggregator eventAggregator)
        {
            this.container = container;
            eventAggregator.GetEvent<GuidelinesLoadedEvent>().Subscribe(OnGuidelinesLoaded, true);
            eventAggregator.GetEvent<QualityIndicatorsLoadedEvent>().Subscribe(OnQualityIndicatorsLoaded, true);
        }

        private void OnGuidelinesLoaded(IEnumerable<OlGuideline> guidelineList)
        {
            GuidelineList = new ObservableCollection<OlGuideline>(guidelineList);
        }

        private void OnQualityIndicatorsLoaded(IEnumerable<OlQualityIndicator> qiList)
        {
            QualityIndicatorList = new ObservableCollection<OlQualityIndicator>(qiList);
        }

        private void OnUpdate(ObservableCollection<object> GuidelineList)
        {
            foreach(object guideline in GuidelineList)
            {
                Console.WriteLine((guideline as OlGuideline).Title);
                OlGuideline newGuideline = container.Resolve<IOlRepository>().GetGuidelineSync((guideline as OlGuideline).Id);                
            }
        }

    }
}
