using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMananger : MonoBehaviour
{
    public static SoundMananger instance;

    // Start is called before the first frame update
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip jumpAudio, hurtAudio, cherryAudio;

    private void Awake()
    {
        instance = this;
    }
    public void JumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void HurtAudio()
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }

    public void CherryAudio()
    {
        audioSource.clip = cherryAudio;
        audioSource.Play();
    }

    public void StopAudio()
    {
        GetComponent<AudioSource>().enabled = false;
    }
   
}
