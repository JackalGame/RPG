using UnityEngine;

namespace RPG.SFX
{
    public class SFXRandomiser : MonoBehaviour
    {
        [SerializeField] AudioClip[] sounds;
        [SerializeField] AudioSource audioSource;

        public void PlayRandomSFX()
        {
            audioSource.clip = sounds[Random.Range(0, sounds.Length)];
            audioSource.Play();
        }
    }
}
