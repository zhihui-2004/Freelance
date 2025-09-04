using UnityEngine;

public class PinDetacher : MonoBehaviour
{
    [Tooltip("Whether the pin is currently detached.")]
    private bool isDetached = false;

    public void OnPinRemoved()
    {
        if (!isDetached)
        {
            transform.parent = null;
            isDetached = true;
        }
    }
}