using UnityEngine;

public class StartGameController : MonoBehaviour
{
    public AudioClip keydownAC;
    AudioSource startAS;
    void Start()
    {
        startAS = GetComponent<AudioSource>();
    }

    public void OnKeyDown()
    {
        startAS.PlayOneShot(keydownAC);
    } 
}
