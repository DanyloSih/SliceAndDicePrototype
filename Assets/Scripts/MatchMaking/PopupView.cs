using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SliceAndDicePrototype.MatchMaking
{
    public class PopupView : MonoBehaviour
    {
        [Tooltip("{0} is placeholder for passed level number, {1} is placeholder for levels count.")]
        [SerializeField] private string _popupTextFormat = "You pass level {0}/{1}";
        [SerializeField] private TextMeshProUGUI _popupText;
        [SerializeField] private AwaitableButton _awaitableButton;

        public void Initialize(int currentLevelNumber, int levelsCount)
        {
            _popupText.text = string.Format(_popupTextFormat, currentLevelNumber, levelsCount);
        }

        public async Task WaitForPlayerResponse(CancellationToken cancellationToken)
        {
            await _awaitableButton.WaitForClick(cancellationToken);
        }
    }
}