using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SliceAndDicePrototype.MatchMaking
{
    [CreateAssetMenu(
       fileName = nameof(WaitForSecondsCompetitorBehaviour),
       menuName = "SliceAndDicePrototype/MatchMaking/" + nameof(WaitForSecondsCompetitorBehaviour))]
    public class WaitForSecondsCompetitorBehaviour : CompetitorBehaviour
    {
        [SerializeField] private float _waitSeconds;

        public async override Task WaitToAction(CancellationToken cancellationToken)
        {
            await UniTask.WaitForSeconds(_waitSeconds, cancellationToken: cancellationToken);
        }
    }
}