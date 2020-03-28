using OlQualityIndicatorManager.Infrastructure.Domain;
using System.Collections.Generic;

namespace OlQualityIndicatorManager.Services.Repositories
{
    public interface IOlRepository
    {
        void GetQualityIndicatorList();
        void GetGuidelineList();
        OlGuideline GetGuidelineSync(string Id);
    }
}