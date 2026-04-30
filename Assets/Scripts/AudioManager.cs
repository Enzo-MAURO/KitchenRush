using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;

    public AudioClip fryerSound;
    public AudioClip steakSound;
    public AudioClip drinkSound;

    void Awake()
    {
        instance = this;
    }

    public void PlayFryer()
    {
        audioSource.PlayOneShot(fryerSound);
    }

    public void PlaySteak()
    {
        audioSource.PlayOneShot(steakSound);
    }

    public void PlayDrink()
    {
        audioSource.PlayOneShot(drinkSound);
    }
}