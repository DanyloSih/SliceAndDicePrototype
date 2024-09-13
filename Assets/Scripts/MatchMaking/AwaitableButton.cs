using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SliceAndDicePrototype.MatchMaking
{
    public class AwaitableButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public async Task WaitForClick(CancellationToken cancellationToken)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            UnityAction listner = null;
            listner = () => {
                taskCompletionSource.SetResult(true);
                _button.onClick.RemoveListener(listner);
            };

            _button.onClick.AddListener(listner);

            using (cancellationToken.Register(() => { 
                taskCompletionSource.TrySetCanceled(); _button.onClick.RemoveListener(listner); 
            }))
            {          
                await taskCompletionSource.Task;
            }
        }

        public async Task WaitForClick()
        {
            await WaitForClick(destroyCancellationToken);
        }
    }
}