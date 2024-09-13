using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SliceAndDicePrototype.MatchMaking
{
    public abstract class CompetitorBehaviour : ScriptableObject
    {
        public abstract Task WaitToAction(CancellationToken cancellationToken);
    }
}