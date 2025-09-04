using UnityEngine;
using UnityEngine.Events;

public class StartIntroController : MonoBehaviour
{
    [SerializeField] private Canvas openingCanvas;
    [SerializeField] private float delaySeconds = 2f;
    [SerializeField] private UnityEvent OnStartDelayFinished;

    private void Start()
    {
        openingCanvas.gameObject.SetActive(true);
        openingCanvas.enabled = false;

        StartCoroutine(StartAfterDelay());
    }

    private System.Collections.IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(delaySeconds);
        openingCanvas.enabled = true;
        OnStartDelayFinished?.Invoke();
    }
}
