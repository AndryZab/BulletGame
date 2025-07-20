using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private SoundData soundData;

    private Dictionary<AudioSource, SoundType> activeSoundSources = new Dictionary<AudioSource, SoundType>();

    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        PlayMusic(SoundType.MusicGameLevel);
    }

    public void PlayOneShot(SoundType type)
    {
        float volume;
        AudioClip clip = soundData.GetClipByType(type, out volume);
        if (clip != null)
        {
            GameObject go = new GameObject($"OneShot_{type}");
            go.transform.SetParent(transform);

            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.Play();

            activeSoundSources.Add(source, type);

            StartCoroutine(DestroyAfterPlay(source));
        }
    }
   
    public void PlayMusic(SoundType type)
    {
        float volume;
        AudioClip clip = soundData.GetClipByType(type, out volume);
        if (clip == null)
            return;

        if (musicSource == null)
        {
            GameObject go = new GameObject("MusicSource");
            go.transform.SetParent(transform);
            musicSource = go.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            Destroy(musicSource.gameObject);
            musicSource = null;
        }
    }

    public void DestroySoundTypeSource(SoundType type)
    {
        List<AudioSource> sourcesToRemove = new List<AudioSource>();

        foreach (var kvp in activeSoundSources)
        {
            if (kvp.Value == type)
                sourcesToRemove.Add(kvp.Key);
        }

        foreach (var src in sourcesToRemove)
        {
            if (src != null)
            {
                src.Stop();
                Destroy(src.gameObject);
            }
            activeSoundSources.Remove(src);
        }
    }

    private IEnumerator DestroyAfterPlay(AudioSource source)
    {
        while (source != null && source.isPlaying)
        {
            yield return null;
        }

        if (source != null)
        {
            if (activeSoundSources.ContainsKey(source))
                activeSoundSources.Remove(source);

            Destroy(source.gameObject);
        }
    }
}
