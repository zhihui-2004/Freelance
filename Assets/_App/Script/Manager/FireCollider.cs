using UnityEngine;

public class FireCollider : MonoBehaviour
{
    // public FireController fireController;
    // public float currentSectionHp;
    // public float maxSectionHp;
    // public bool isInteracted = false;
    // private int activeHits = 0;

    // void Start()
    // {
    //     maxSectionHp = fireController.maximumHealth * 0.1f;
    //     currentSectionHp = maxSectionHp;
    // }

    // void Update()
    // {
    //     if (activeHits > 0)
    //     {
    //         DrainSectionHp();
    //     }
    // }

    // public void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("WetChemical"))
    //     {
    //         if (!isInteracted)
    //         {
    //             fireController.collidersHit += 1;
    //             isInteracted = true;

    //             if (fireController.collidersHit >= 4)
    //             {
    //                 ResetColliders();
    //             }
    //         }

    //         if (currentSectionHp <= 0f)
    //             return;

    //         activeHits++;
    //         fireController.isDraining = true;
    //     }
    // }

    // public void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("WetChemical"))
    //     {
    //         activeHits = Mathf.Max(activeHits - 1, 0);
    //         checkShouldStopDrain();
    //     }
    // }

    // private void DrainSectionHp()
    // {
    //     if (currentSectionHp > 0f)
    //     {
    //         currentSectionHp -= Time.deltaTime * fireController.drainRate;
    //     }

    //     if (currentSectionHp <= 0f)
    //     {
    //         activeHits = 0;
    //         checkShouldStopDrain();
    //     }
    // }

    // private void checkShouldStopDrain()
    // {
    //     if (activeHits == 0)
    //     {
    //         fireController.isDraining = false;
    //     }
    // }

    // private void ResetColliders()
    // {
    //     foreach (FireCollider collider in fireController.fireColliders)
    //     {
    //         collider.isInteracted = false;
    //         collider.currentSectionHp = collider.maxSectionHp;
    //     }
    // }
}
