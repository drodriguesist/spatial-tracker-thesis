using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Track
{
    public AudioClip clip;

    [Range(.1f, 3f)]
    public float pitch;
    [Range(0f,1f)]
    public float volume;

    public string name;

    public AudioSource source;

    public bool loop;
    
    
}
