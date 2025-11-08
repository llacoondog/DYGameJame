using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<SoundManager>();
            }
            return instance;
        }
    }
    static SoundManager instance;
    Queue<AudioSource> audioSources = new Queue<AudioSource>();

    void Awake()
    {
        if(instance != null) Destroy(this.gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Enqueue(audioSource);
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        StartCoroutine(AudioSourceCoroutine(audioClip));
    }
    IEnumerator AudioSourceCoroutine(AudioClip audioClip)
    {
        if(audioSources.Count == 0) yield break;
        AudioSource audioSource = audioSources.Dequeue();
        
        audioSource.clip = audioClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSources.Enqueue(audioSource);
    }
    
}
