using UnityEngine;

[CreateAssetMenu(menuName = "Audio Object")]
public class AudioInfo : ScriptableObject
{
    public AudioClip clip;

    public AudioSource PlayAudio()
    {
        var go = new GameObject("Audio Source - " + name);
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = false;
        source.spatialize = false;
        source.pitch = Random.Range(0.9f, 1.1f);
        source.volume = Random.Range(0.8f, 1.2f);
        
        source.Play();
        return source;

    }
}