using System.Collections.Generic;
using System.Threading.Tasks;
using PortScannerDetectorInstrument.Entities;

namespace PortScannerDetectorInstrument.Reporters
{
    public interface IReporter
    {
         Task Report(List<SuspiciousSource> sources);
    }
}