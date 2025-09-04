using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FireController : MonoBehaviour
{
    [SerializeField] private FireExtinguisherController fireExtinguisherController;
    [Header("Value")]
    public float maximumHealth = 100f;
    public float drainRate = 2f;
    [SerializeField] private float currentHealth;

    public int collidersHit = 0;

    [Header("References")]
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private GameObject fireObject;

    [Header("Events")]
    [SerializeField] private UnityEvent onFirePullOut;

    private Vector3 fireInitialScale;
    public bool isDraining = false;
    private bool isWetChemicalColliding = false;

    void Start()
    {
        InitializeFire();
    }

    void Update()
    {
        if (isWetChemicalColliding && fireExtinguisherController.IsSpraying)
        {
            DrainFireHealth();
        }
    }

    private void InitializeFire()
    {
        currentHealth = maximumHealth;
        healthBarSlider.maxValue = maximumHealth;
        healthBarSlider.value = maximumHealth;
        fireInitialScale = fireObject.transform.localScale;
    }

    private void DrainFireHealth()
    {
        if (currentHealth > 0)
        {
            currentHealth = Mathf.Max(currentHealth - Time.deltaTime * drainRate, 0);
            healthBarSlider.value = currentHealth;

            AdjustFireScale();

            if (currentHealth <= 0)
            {
                ExtinguishFire();
            }
        }
    }

    private void AdjustFireScale()
    {
        float healthPercentage = currentHealth / maximumHealth;
        fireObject.transform.localScale = fireInitialScale * healthPercentage;
    }

    private void ExtinguishFire()
    {
        fireObject.SetActive(false);
        isWetChemicalColliding = false;
        onFirePullOut?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WetChemical"))
        {
            isWetChemicalColliding = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WetChemical"))
        {
            isWetChemicalColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WetChemical"))
        {
            isWetChemicalColliding = false;
        }
    }
}
