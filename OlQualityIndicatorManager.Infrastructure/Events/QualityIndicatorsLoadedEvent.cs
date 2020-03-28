﻿using OlQualityIndicatorManager.Infrastructure.Domain;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlQualityIndicatorManager.Infrastructure.Events
{
    public class QualityIndicatorsLoadedEvent : PubSubEvent<IEnumerable<OlQualityIndicator>> { }
}
