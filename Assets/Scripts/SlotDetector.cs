using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlotDetector : MonoBehaviour
{
    [Tooltip("The tag this slot accepts (e.g., Red, Green, Blue)")]
    public string acceptedTag;

    [Header("Audio Feedback")]
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private AudioSource audioSource;
    private bool isOccupied = false;

    private void Start()
    {
        // Add or get AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log($"{gameObject.name}: AudioSource added at runtime.");
        }
        else
        {
            Debug.Log($"{gameObject.name}: AudioSource already exists.");
        }

        // Basic audio setup
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D sound
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied) return;

        Debug.Log($"{gameObject.name}: Triggered by {other.name} (Tag: {other.tag})");

        if (other.CompareTag(acceptedTag))
        {
            Debug.Log($"{gameObject.name}: Correct cube detected!");

            // Snap cube to slot
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;

            // Disable movement
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
            if (grab != null) Destroy(grab);

            isOccupied = true;

            // Play correct sound
            if (correctSound != null)
                audioSource.PlayOneShot(correctSound);
            else
                Debug.LogWarning($"{gameObject.name}: Correct sound not assigned.");

            // Notify PuzzleManager
            if (PuzzleManager.Instance != null)
                PuzzleManager.Instance.NotifyCubePlaced();
        }
        else
        {
            Debug.Log($"{gameObject.name}: Wrong cube detected! Expected '{acceptedTag}', got '{other.tag}'");

            // Play wrong sound
            if (wrongSound != null)
                audioSource.PlayOneShot(wrongSound);
            else
                Debug.LogWarning($"{gameObject.name}: Wrong sound not assigned.");
        }
    }
}
