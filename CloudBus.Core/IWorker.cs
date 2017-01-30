using System.Threading;
using System.Threading.Tasks;

namespace CloudBus.Core
{
    public interface IWorker
    {
        Task Start(CancellationToken token);
    }
}
