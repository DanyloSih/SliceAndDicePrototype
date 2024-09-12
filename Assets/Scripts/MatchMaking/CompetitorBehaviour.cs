using System.Threading;
using System.Threading.Tasks;

namespace SliceAndDicePrototype.MatchMaking
{
    public abstract class CompetitorBehaviour
    {
        public abstract Task WaitToAction(CancellationToken cancellationToken);
    }
}