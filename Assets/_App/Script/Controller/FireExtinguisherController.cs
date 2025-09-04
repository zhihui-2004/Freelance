using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[DisallowMultipleComponent]
public class FireExtinguisherController : MonoBehaviour
{
    [Header("XR Components")]
    [SerializeField] private XRGrabInteractable extinguisher;
    [SerializeField] private XRGrabInteractable pin;
    [SerializeField] private XRGrabInteractable nozzle;

    [Header("Chemical Spray")]
    [SerializeField] private ParticleSystem chemical;

    [Header("Input Actions (XRI)")]
    [SerializeField] private InputActionReference sprayAction;

    [Header("Events")]
    [SerializeField] private UnityEvent onPinRemoved;
    [SerializeField] private UnityEvent onSprayStart;
    [SerializeField] private UnityEvent onSprayStop;

    // 状态
    public bool HasBeenPickedUp { get; private set; } = false;
    public bool IsPinRemoved { get; private set; } = false;
    public bool IsSpraying { get; private set; } = false;

    private IXRSelectInteractor handleHoldingHand = null;
    private IXRSelectInteractor nozzleHoldingHand = null;

    private void OnEnable()
    {
        chemical?.Stop();

        if (extinguisher != null)
        {
            extinguisher.selectEntered.AddListener(OnExtinguisherGrab);
            extinguisher.selectExited.AddListener(OnExtinguisherRelease);
        }

        if (pin != null)
        {
            pin.selectEntered.AddListener(OnPinGrab);
            pin.selectExited.AddListener(OnPinRelease);
        }

        if (nozzle != null)
        {
            nozzle.selectEntered.AddListener(OnNozzleGrab);
            nozzle.selectExited.AddListener(OnNozzleRelease);
        }

        if (sprayAction.action != null)
        {
            sprayAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (extinguisher != null)
        {
            extinguisher.selectEntered.RemoveListener(OnExtinguisherGrab);
            extinguisher.selectExited.RemoveListener(OnExtinguisherRelease);
        }

        if (pin != null)
        {
            pin.selectEntered.RemoveListener(OnPinGrab);
            pin.selectExited.RemoveListener(OnPinRelease);
        }

        if (nozzle != null)
        {
            nozzle.selectEntered.RemoveListener(OnNozzleGrab);
            nozzle.selectExited.RemoveListener(OnNozzleRelease);
        }

        if (sprayAction.action != null)
        {
            sprayAction.action.Disable();
        }
    }

    private void Update()
    {
        if (IsPinRemoved && extinguisher.isSelected && handleHoldingHand != null)
        {
            if (sprayAction.action != null)
            {
                float triggerValue = sprayAction.action.ReadValue<float>();
                bool pressed = triggerValue > 0.5f || sprayAction.action.ReadValue<bool>();
                if (pressed && !IsSpraying)
                {
                    StartSpraying();
                }
                else if (!pressed && IsSpraying)
                {
                    StopSpraying();
                }
            }
        }
    }

    private void OnExtinguisherGrab(SelectEnterEventArgs args)
    {
        if (!HasBeenPickedUp)
        {
            HasBeenPickedUp = true;
        }

        if (!IsPinRemoved && pin != null)
        {
            pin.enabled = true;
        }

        handleHoldingHand = args.interactorObject;
        if (handleHoldingHand != null)
        {
            Component interactorComponent = handleHoldingHand as Component;
            if (interactorComponent != null && interactorComponent.gameObject != null)
            {
                Debug.Log($"Extinguisher grabbed by: {interactorComponent.gameObject.name}");
            }
            else
            {
                Debug.Log("Extinguisher grabbed, but interactor name unavailable.");
            }
        }
    }

    private void OnExtinguisherRelease(SelectExitEventArgs args)
    {
        if (pin != null)
        {
            pin.enabled = false;
        }

        if (handleHoldingHand == args.interactorObject)
        {
            handleHoldingHand = null;
        }
    }

    private void OnPinGrab(SelectEnterEventArgs args)
    {
        if (IsPinRemoved)
        {
            Debug.Log("Pin already removed.");
        }
        else
        {
            Debug.Log("Pin grabbed, pull to remove.");
        }
    }

    private void OnPinRelease(SelectExitEventArgs args)
    {
        if (!IsPinRemoved)
        {
            IsPinRemoved = true;
            pin.enabled = false;
            onPinRemoved?.Invoke();
            PinDetacher detacher = pin.GetComponent<PinDetacher>();
            detacher?.OnPinRemoved();
        }
    }

    private void OnNozzleGrab(SelectEnterEventArgs args)
    {
        if (handleHoldingHand != null && args.interactorObject == handleHoldingHand)
        {
            args.interactorObject.transform.GetComponent<XRBaseInteractor>()?.interactionManager.SelectExit(args.interactorObject, nozzle);
        }
        else
        {
            nozzleHoldingHand = args.interactorObject;
            // 获取交互器的名称
            if (nozzleHoldingHand != null)
            {
                Component interactorComponent = nozzleHoldingHand as Component;
                if (interactorComponent != null && interactorComponent.gameObject != null)
                {
                    Debug.Log($"Nozzle grabbed by: {interactorComponent.gameObject.name}");
                }
                else
                {
                    Debug.Log("Nozzle grabbed, but interactor name unavailable.");
                }
            }
        }
    }

    private void OnNozzleRelease(SelectExitEventArgs args)
    {
        if (nozzleHoldingHand == args.interactorObject)
        {
            nozzleHoldingHand = null;
        }
    }

    public void StartSpraying()
    {
        if (IsPinRemoved && !IsSpraying)
        {
            IsSpraying = true;
            chemical?.Play();
            onSprayStart?.Invoke();
            AudioManager.Instance.PlaySFX("sprayStart", true);
        }
        else
        {
            Debug.Log($"Spraying not started. IsPinRemoved: {IsPinRemoved}, IsSpraying: {IsSpraying}");
        }
    }

    public void StopSpraying()
    {
        if (IsSpraying)
        {
            IsSpraying = false;
            chemical?.Stop();
            onSprayStop?.Invoke();
            AudioManager.Instance.StopSFX();
        }
    }
}