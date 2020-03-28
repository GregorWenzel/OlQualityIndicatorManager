using OlQualityIndicatorManager.Infrastructure.Domain;
using OlQualityIndicatorManager.Infrastructure.Events;
using OlQualityIndicatorManager.Services.Exports;
using OlQualityIndicatorManager.Services.Repositories;
using OlQualityIndicatorManager.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity;

namespace OlQualityIndicatorManager.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        IEventAggregator eventAggregator;
        IUnityContainer container;
        IRegionManager regionManager;

        private IEnumerable<OlGuideline> GuidelineList;
        private IEnumerable<OlQualityIndicator> QualityIndicatorList;

        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand InitializedCommand
        {
            get
            {
                return new DelegateCommand(OnInitialized);
            }
        }

        public MainWindowViewModel(IUnityContainer container, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.container = container;
            this.eventAggregator = eventAggregator;
        }

        private void OnInitialized()
        {
            regionManager.RegisterViewWithRegion("SelectionRegion", typeof(ProgressView));

            eventAggregator.GetEvent<GuidelinesLoadedEvent>().Subscribe(OnGuidelinesLoaded, false);

            container.Resolve<IOlRepository>().GetGuidelineList();

/*
            MySqlRepository repo = new MySqlRepository();
            var guidelines = repo.GetGuidelines();
            regionManager.RequestNavigate("SelectionRegion", new Uri("GuidelineGridView", UriKind.Relative));
            regionManager.RequestNavigate("DetailRegion", new Uri("RecommendationPieChartView", UriKind.Relative));

            QualityIndicatorExcelExporter exporter = new QualityIndicatorExcelExporter();
            //exporter.ExportAllQualityIndicators(guidelines);

            eventAggregator.GetEvent<GuidelinesLoadedEvent>().Publish(guidelines);
            eventAggregator.GetEvent<GuidelinesSelectedEvent>().Publish(guidelines);
*/
        }

        private void OnGuidelinesLoaded(IEnumerable<OlGuideline> olGuidelines)
        {
            GuidelineList = olGuidelines;
            eventAggregator.GetEvent<GuidelinesLoadedEvent>().Unsubscribe(OnGuidelinesLoaded);
            eventAggregator.GetEvent<QualityIndicatorsLoadedEvent>().Subscribe(OnQualityIndicatorsLoaded, false);
            container.Resolve<IOlRepository>().GetQualityIndicatorList();            
        }

        private void OnQualityIndicatorsLoaded(IEnumerable<OlQualityIndicator> olQualityIndicators)
        {
            QualityIndicatorList = olQualityIndicators;
            eventAggregator.GetEvent<QualityIndicatorsLoadedEvent>().Unsubscribe(OnQualityIndicatorsLoaded);

            regionManager.RequestNavigate("SelectionRegion", new Uri("GuidelineSelectionView", UriKind.Relative));
            eventAggregator.GetEvent<GuidelinesLoadedEvent>().Publish(GuidelineList);
            eventAggregator.GetEvent<QualityIndicatorsLoadedEvent>().Publish(QualityIndicatorList);

            //regionManager.RequestNavigate("SelectionRegion", new Uri("GuidelineSelectionView", UriKind.Relative));
            //eventAggregator.GetEvent<GuidelinesLoadedEvent>().Publish(GuidelineList);

            //eventAggregator.GetEvent<QualityIndicatorsLoadedEvent>().Unsubscribe(OnQualityIndicatorsLoaded);
            //eventAggregator.GetEvent<QualityIndicatorsLoadedEvent>().Publish(olQualityIndicators);
        }
    }
}
