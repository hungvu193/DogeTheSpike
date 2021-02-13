using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{


  public AudioClip audioClip;

  public string name;

  [Range(0f, 1f)]
  public float volume;
  [Range(.1f, 3f)]
  public float pitch;
  // Start is called before the first frame update

  public bool isLoop;
  [HideInInspector]
  public AudioSource audioSource;
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
