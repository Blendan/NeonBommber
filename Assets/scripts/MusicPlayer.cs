using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] music;

    private AudioSource audioSource;
    private int last = -1;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        last = (int)Random.Range(0, music.Length - 1);
        audioSource.clip = music[last];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            int id = -1;
            while((id = (int)Random.Range(0, music.Length - 1))==last)
            {
                //äh ja //TODO ?
            }
            audioSource.clip = music[id];
            audioSource.Play();
            last = id;
        }
    }
}
