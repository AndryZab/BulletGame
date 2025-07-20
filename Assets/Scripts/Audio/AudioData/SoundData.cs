using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SoundEntry
{
    public SoundType type;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
}

[CreateAssetMenu(fileName = "AudioData", menuName = "Audio/AudioData")]
public class SoundData : ScriptableObject
{
    public List<SoundEntry> sounds = new List<SoundEntry>();

    public AudioClip GetClipByType(SoundType type, out float volume)
    {
        var entry = sounds.FirstOrDefault(s => s.type == type);
        if (entry != null)
        {
            volume = entry.volume;
            return entry.clip;
        }
        volume = 1f;
        return null;
    }

}
