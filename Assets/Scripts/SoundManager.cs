using UnityEngine.Audio;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
  public Sound[] sounds;
  public static SoundManager instance;
  // Start is called before the first frame update  
  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
      return;
    }
    DontDestroyOnLoad(gameObject);
    foreach (Sound s in sounds)
    {
      s.audioSource = gameObject.AddComponent<AudioSource>();
      s.audioSource.clip = s.audioClip;
      s.audioSource.volume = s.volume;
      s.audioSource.pitch = s.pitch;
      s.audioSource.loop = s.isLoop;
    }
  }

  public void Play(string name)
  {
    Sound s = Array.Find(sounds, sound => sound.name == name);
    if (s == null) return;
    s.audioSource.Play();
  }
}
