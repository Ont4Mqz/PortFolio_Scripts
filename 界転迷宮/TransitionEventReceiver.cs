using UnityEngine;

public class TransitionEventReceiver : MonoBehaviour
{
    [SerializeField] private CameraSwitcher cameraSwitcher;

    // Animation Event から呼ばれる
    public void EndTransition()
    {
        if (cameraSwitcher != null)
            cameraSwitcher.EndTransition();
    }
}
