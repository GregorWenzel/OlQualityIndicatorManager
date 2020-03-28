using OlQualityIndicatorManager.Infrastructure.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.ViewModels
{
    public class ProgressViewModel: BindableBase
    {
        private double progress;
        public double Progress
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }

        private string groupLabel;
        public string GroupLabel
        {
            get => groupLabel;
            set => SetProperty(ref groupLabel, value);
        }

        private string itemLabel;
        public string ItemLabel
        {
            get => itemLabel;
            set => SetProperty(ref itemLabel, value);
        }
        

        public ProgressViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<DownloadProgressChangedEvent>().Subscribe(OnProgressChanged, true);
        }

        private void OnProgressChanged(DownloadProgressEventArgs newProgress)
        {
            Progress = newProgress.Progress;
            GroupLabel = newProgress.GroupName;
            ItemLabel = newProgress.ItemName;
        }
    }
}
