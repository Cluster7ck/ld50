using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStuff : MonoBehaviour
{
    private static AudioStuff instance;
    public static AudioStuff Instance {
        get
        {
            return instance;
        }
    }

    [SerializeField] private AudioSource oneShotSource;
    [SerializeField] private AudioSource oneShotSourceLoud;
    [SerializeField] private AudioSource musicAudioSource;

    [SerializeField] private AudioClip splat;
    [SerializeField] private AudioClip munch1;
    [SerializeField] private AudioClip munch2;
    [SerializeField] private AudioClip slurp;

    [SerializeField] private AudioClip[] hitSounds;

    private Queue<AudioClip> soundQueue  = new Queue<AudioClip>();

    [SerializeField] private float splatCooldown;
    private float currentSplatTime;
    private int splats;

    private void Awake()
    {
        instance = this;
    }

    public void MuteSfx()
    {
        oneShotSource.mute = !oneShotSource.mute;
        oneShotSourceLoud.mute = !oneShotSourceLoud.mute;
    }

    public void MuteMusic()
    {
        musicAudioSource.mute = !musicAudioSource.mute;
    }

    public void PlaySplat()
    {
        // for every consecutive hit raise the pitch by a semi tone
        oneShotSource.pitch = Mathf.Pow(1.05946f, splats);
        oneShotSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
        currentSplatTime = splatCooldown;
        splats++;
    }

    public void PlayMunch()
    {
        oneShotSourceLoud.PlayOneShot(Random.value > 0.5f ? munch1 : munch2);
    }

    public void PlaySlurp()
    {
        oneShotSourceLoud.PlayOneShot(slurp);
    }

    private void Update()
    {
        currentSplatTime = Mathf.Clamp(currentSplatTime - Time.deltaTime,0,1);
        
        if(currentSplatTime == 0)
        {
            splats = 0;
        }
    }
}
