using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartPanelController : MonoBehaviour
{
    [SerializeField] private string[] titleTexts;
    [SerializeField][TextArea(5, 10)] private string[] descriptionTexts;
    [SerializeField] private Canvas openingCanvas;
    [SerializeField] private TextMeshProUGUI titlePanelText;
    [SerializeField] private TextMeshProUGUI descriptionPanelText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private ParticleSystem[] fires;
    [SerializeField] private FireExtinguisherController extinguisherController;

    [Header("Fire Alarm Sound Effect")]
    [SerializeField] private GameObject fireAlamSound;

    [SerializeField] private UnityEvent onEndOfCavas;

    [Header("Objects To Toggle")]
    [SerializeField] private GameObject fireExtinguisher;
    [SerializeField] private GameObject table;

    [Header("List index configuration")]
    [SerializeField] private int startTextIndex = 0;

    [Header("Panel Durations (seconds)")]
    [SerializeField] private float[] panelDurations;

    [Tooltip("Default duration if panelDurations is too short or value is invalid")]
    [SerializeField] private float defaultDuration = 2f;

    [Header("Timer Display")]
    [SerializeField] private TextMeshProUGUI timerText;

    private int currentIndex = 0;
    private bool isPickupChecked = false;
    private bool isPinChecked = false;

    void Start()
    {
        fireAlamSound.SetActive(false);
        foreach (var fire in fires)
        {
            fire.Stop();
        }

        if (fireExtinguisher != null) fireExtinguisher.SetActive(false);
        if (table != null) table.SetActive(false);
        if (timerText != null) timerText.text = "";
    }

    public void StartShowingText()
    {
        if (startTextIndex < 0 || startTextIndex >= titleTexts.Length || startTextIndex >= descriptionTexts.Length)
        {
            Debug.LogError("startTextIndex is out of range for titleTexts or descriptionTexts arrays.");
            return;
        }

        currentIndex = startTextIndex;
        ShowDescription(currentIndex);
        StartCoroutine(AutoAdvancePanel());
    }

    private IEnumerator AutoAdvancePanel()
    {
        float duration = defaultDuration;

        if (currentIndex >= 0 && currentIndex < panelDurations.Length)
        {
            if (panelDurations[currentIndex] > 0f)
                duration = panelDurations[currentIndex];
        }

        if (timerText != null)
        {
            timerText.text = duration.ToString("F1") + "s";
            for (float timer = duration; timer > 0; timer -= Time.deltaTime)
            {
                timerText.text = timer.ToString("F1") + "s";
                yield return null;
            }
            timerText.text = "0.0s";
        }
        NextPanel();
    }

    private void ShowDescription(int index)
    {
        if (index >= 0 && index < descriptionTexts.Length && index < titleTexts.Length)
        {
            titlePanelText.text = titleTexts[index];
            descriptionPanelText.text = descriptionTexts[index];
        }
        else
        {
            Debug.LogWarning("Index out of range for titleTexts or descriptionTexts.");
        }
    }

    private void NextPanel()
    {
        currentIndex++;
        int adjustedIndex = startTextIndex + currentIndex;

        if (adjustedIndex == startTextIndex + 1)
        {
            PlayFireEffect();
            ShowDescription(adjustedIndex);
            StartCoroutine(AutoAdvancePanel());
            return;
        }

        if (adjustedIndex == startTextIndex + 2)
        {
            StartCoroutine(ActiveAndShowNarration());
            return;
        }

        if (adjustedIndex == startTextIndex + 3)
        {
            if (!isPickupChecked)
            {
                StartCoroutine(CheckForPickup());
                return;
            }
        }

        if (adjustedIndex == startTextIndex + 4)
        {
            if (!isPinChecked)
            {
                StartCoroutine(CheckForPinRemoved());
                return;
            }
        }

        if (adjustedIndex < titleTexts.Length && adjustedIndex < descriptionTexts.Length)
        {
            ShowDescription(adjustedIndex);
            StartCoroutine(AutoAdvancePanel());
        }
        else
        {
            DeactivateCanvas();
        }
    }

    private void PreviousPanel()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowDescription(currentIndex);
        }
    }

    private void DeactivateCanvas()
    {
        openingCanvas.enabled = false;
        if (previousButton != null) previousButton.gameObject.SetActive(false);
        onEndOfCavas?.Invoke();
    }

    private void PlayAlarmSound()
    {
        fireAlamSound.SetActive(true);
    }

    private void PlayFireEffect()
    {
        foreach (var fire in fires)
        {
            if (fire != null)
            {
                fire.Play();
            }
            else
            {
                Debug.LogWarning("Fire particle system is not assigned.");
            }
        }

        this.Wait(2f, PlayAlarmSound);
    }

    private IEnumerator CheckForPickup()
    {
        while (!extinguisherController.HasBeenPickedUp)
            yield return null;

        isPickupChecked = true;
        ShowDescription(currentIndex);
        StartCoroutine(AutoAdvancePanel());
    }

    private IEnumerator CheckForPinRemoved()
    {
        while (!extinguisherController.IsPinRemoved)
            yield return null;

        isPinChecked = true;
        ShowDescription(currentIndex);
        yield return new WaitForSeconds(1.5f);
        DeactivateCanvas();
    }

    private IEnumerator ActiveAndShowNarration()
    {
        yield return StartCoroutine(ActiveFireExtinguisherAndTable());
        yield return new WaitForSeconds(1f);
        ShowDescription(currentIndex);
        StartCoroutine(AutoAdvancePanel());
    }

    public void DeactiveTable()
    {
        if (table != null) table.SetActive(false);
    }

    private IEnumerator ActiveFireExtinguisherAndTable()
    {
        if (fireExtinguisher != null) fireExtinguisher.SetActive(true);
        if (table != null) table.SetActive(true);
        yield return null;
    }

    private void InvokeAfter(float delay, Action action)
    {
        StartCoroutine(InvokeAfterRoutine(delay, action));
    }

    private IEnumerator InvokeAfterRoutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}