using UnityEngine;

public class FirePopSFXPlayer : MonoBehaviour
{
    ParticleSystem ps;
    AudioSource source;
    bool played = false;
    System.Random rand = new System.Random();

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ps.particleCount > 1 && !played)
        {
            double pitch = 0.7 + rand.NextDouble() * (1.5 - 0.7); // Random between  0.7 and 1.5
            source.pitch = (float)pitch;
            source.Play();
            played = true;
        }
        else if (ps.particleCount <= 1)
        {
            played = false;
        }
    }
}
