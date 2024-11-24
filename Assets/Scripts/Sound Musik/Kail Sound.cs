using UnityEngine;

public class HookSound : MonoBehaviour
{
    public AudioClip hookSound; // Masukkan AudioClip dari Inspector
    private AudioSource audioSource;

    private void Start()
    {
        // Tambahkan AudioSource ke GameObject ini jika belum ada
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hookSound;
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Pastikan objek bawah laut memiliki tag "Bait"
        if (other.CompareTag("Bait"))
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (audioSource != null && hookSound != null)
        {
            audioSource.Play();
        }
    }
}
