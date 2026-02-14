using UnityEngine;

// Going to be specifically for looping an audio track that has a diffrent starting track
public class AudioLooper : MonoBehaviour
{
    AudioSource source;
    [SerializeField] AudioClip clip;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (source == null)
            return;

        if (source.isPlaying)
            return;

        if (source.clip == clip)
            return;

        source.clip = clip;
        source.loop = true;
        source.Play();
    }
}
