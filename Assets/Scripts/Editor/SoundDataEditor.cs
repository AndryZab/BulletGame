using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(SoundData))]
public class SoundDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Auto Generate Sounds"))
        {
            AutoFill();
        }
    }

    private void AutoFill()
    {
        SoundData soundData = (SoundData)target;

        int added = 0;

        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("SFX");
        added += AddClipsToSoundData(soundData, sfxClips);
        AudioClip[] MusicClips = Resources.LoadAll<AudioClip>("Music");
        added += AddClipsToSoundData(soundData, MusicClips);

        EditorUtility.SetDirty(soundData);
    }

    private int AddClipsToSoundData(SoundData soundData, AudioClip[] clips)
    {
        int added = 0;

        foreach (AudioClip clip in clips)
        {
            foreach (SoundType type in Enum.GetValues(typeof(SoundType)))
            {
                if (clip.name.Equals(type.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    bool exists = soundData.sounds.Any(s => s.type == type && s.clip == clip);

                    if (!exists)
                    {
                        soundData.sounds.Add(new SoundEntry
                        {
                            type = type,
                            clip = clip,
                            volume = 0.5f,
                        });
                        added++;
                    }
                    break;
                }
            }
        }

        return added;
    }
}
