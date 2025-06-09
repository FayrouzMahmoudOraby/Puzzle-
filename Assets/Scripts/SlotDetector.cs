using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlotDetector : MonoBehaviour
{
    [Tooltip("The tag of the correct cube (Red, Green, or Blue)")]
    public string acceptedTag;

    [Header("Audio Feedback")]
    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource audioSource;

    private bool isOccupied = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource if missing
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log($"{gameObject.name}: AudioSource was missing, added one at runtime.");
        }
        else
        {
            Debug.Log($"{gameObject.name}: AudioSource found.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied)
        {
            Debug.Log($"{gameObject.name}: Already occupied. Ignoring {other.name}");
            return;
        }

        Debug.Log($"{gameObject.name}: Detected {other.name} entering trigger.");

        if (other.CompareTag(acceptedTag))
        {
            Debug.Log($"{gameObject.name}: {other.name} has the correct tag ({acceptedTag}).");

            // Snap and lock cube
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                Debug.Log($"{other.name}: Rigidbody found and set to isKinematic = true.");
            }

            XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                Destroy(grab);
                Debug.Log($"{other.name}: XRGrabInteractable removed.");
            }

            isOccupied = true;

            if (correctSound != null)
            {
                audioSource.PlayOneShot(correctSound);
                Debug.Log($"{gameObject.name}: Played correct sound.");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: Correct sound is not assigned!");
            }

            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.NotifyCubePlaced();
                Debug.Log($"{gameObject.name}: Notified PuzzleManager of placement.");
            }
            else
            {
                Debug.LogError("PuzzleManager.Instance is NULL!");
            }
        }
        else
        {
            Debug.Log($"{gameObject.name}: {other.name} has WRONG tag (Expected: {acceptedTag}, Found: {other.tag})");

            if (wrongSound != null)
            {
                audioSource.PlayOneShot(wrongSound);
                Debug.Log($"{gameObject.name}: Played wrong sound.");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: Wrong sound is not assigned!");
            }
        }
    }
}
