using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class CollisionSound : MonoBehaviour
{
    [Tooltip("The layers that cause the sound to play")]
    [SerializeField] private LayerMask collisionTriggers = ~0;

    [Tooltip("Audio source to play from")]
    [SerializeField] private AudioSource source;

    [Tooltip("Optional override clip (if null, uses AudioSource.clip)")]
    [SerializeField] private AudioClip clip;

    [Header("Sound Settings")]
    [Tooltip("Map collision velocity to volume")]
    [SerializeField] private AnimationCurve velocityVolumeCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField] private float volumeAmp = 0.8f;
    [SerializeField] private float velocityAmp = 0.5f;

    [Tooltip("Cooldown between sounds (seconds)")]
    [SerializeField] private float soundRepeatDelay = 0.2f;

    private Rigidbody body;
    private bool canPlaySound = true;
    private Coroutine playSoundRoutine;

    private void Start()
    {
        body = GetComponent<Rigidbody>();

        if (source == null)
        {
            source = GetComponent<AudioSource>();
            if (source == null)
                Debug.LogWarning($"{name} has no AudioSource assigned for CollisionSound.");
        }

        // Prevent sound spam at start if object spawns with velocity
        StartCoroutine(SoundPlayBuffer(1f));
    }

    private void OnDisable()
    {
        if (playSoundRoutine != null)
            StopCoroutine(playSoundRoutine);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (body == null)
            return;

        // Check if collision layer is allowed
        if ((collisionTriggers.value & (1 << collision.gameObject.layer)) == 0)
            return;

        if (!canPlaySound || source == null || !source.enabled)
            return;

        // Extra check: only play if hitting something with meaningful mass
        if (collision.collider.attachedRigidbody == null || collision.collider.attachedRigidbody.mass > 0.0000001f)
        {
            float velocityMag = collision.relativeVelocity.magnitude * velocityAmp;
            float volume = velocityVolumeCurve.Evaluate(velocityMag) * volumeAmp;

            if (clip != null)
                source.PlayOneShot(clip, volume);
            else if (source.clip != null)
                source.PlayOneShot(source.clip, volume);

            if (playSoundRoutine != null)
                StopCoroutine(playSoundRoutine);
            playSoundRoutine = StartCoroutine(SoundPlayBuffer(soundRepeatDelay));
        }
    }

    private IEnumerator SoundPlayBuffer(float time)
    {
        canPlaySound = false;
        yield return new WaitForSeconds(time);
        canPlaySound = true;
        playSoundRoutine = null;
    }
}
