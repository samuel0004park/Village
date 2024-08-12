using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//serializable enables class to be shown in inspector 
[System.Serializable]
public class Sound
{
    //holds the audio itself and has volume, clip, loop variables
    private AudioSource source;

    //variables for showing in the inspector
    public string name;
    public AudioClip clip;
    public float volume;
    public bool loop;

    //method to set the diff classes for ea clip
    public void SetSource(AudioSource _source) {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
    }

    //method to change values
    public void SetVolume()
    {
        source.volume = volume;
    }
    public void Play()
    {
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }
    public void SetLoop()
    {
        source.loop = true;
    }
    public void CancelLoop()
    {
        source.loop = false;
    }
}
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;

    static public AudioManager instance;

    private void Awake()
    {
        //keep singleton property
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    void Start()
    {
        //for all clips, make instances of Sound class
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("Name of sound file: " + i + " = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }
    public void SetVolume(string _name, float _volume)
    {
        //set volumes for the clip with corresponding name
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].volume = _volume;
                sounds[i].SetVolume();
                return;
            }

        }
    }
    public void Play(string _name)
    {
        //play set clip
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }

        }
    }
    public void Stop(string _name)
    {
        //stop clip
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }

        }
    }

    public void SetLoop(string _name)
    {
        //set true for looping a clip
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].SetLoop();
                return;
            }

        }
    }

    public void CancelLoop(string _name)
    {
        //cancel loop
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].CancelLoop();
                return;
            }

        }
    }
}
