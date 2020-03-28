using OlQualityIndicatorManager.Grid;
using OlQualityIndicatorManager.Grid.Views;
using OlQualityIndicatorManager.Infrastructure;
using OlQualityIndicatorManager.Plots.Views;
using OlQualityIndicatorManager.Services;
using OlQualityIndicatorManager.Services.Repositories;
using OlQualityIndicatorManager.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;

namespace OlQualityIndicatorManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            RegionManager.SetRegionManager(MainWindow, Container.Resolve<IRegionManager>());
            RegionManager.UpdateRegions();

            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IOlRepository, OlRepository>();
            containerRegistry.Register<object, MainWindow>("MainWindow");
            containerRegistry.Register<object, ProgressView>("ProgressView");
            containerRegistry.Register<object, GridView>("GridView");
            containerRegistry.Register<object, GuidelineGridView>("GuidelineGridView");
            containerRegistry.Register<object, GuidelineSelectionView>("GuidelineSelectionView");
            containerRegistry.Register<object, RecommendationPieChartView>("RecommendationPieChartView");

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule<ServicesModule>();
            moduleCatalog.AddModule<GridModule>();
            moduleCatalog.AddModule<InfrastructureModule>();
        }
    }
}
