using UnityEngine;

namespace SliceAndDicePrototype.Utilities
{
    public class FPSUnlocker : MonoBehaviour
    {
        public void Start()
        {        
            int refreshRate = (int)Screen.currentResolution.refreshRateRatio.value;
            Application.targetFrameRate = refreshRate;
            QualitySettings.vSyncCount = 1;
        }
    }
}
