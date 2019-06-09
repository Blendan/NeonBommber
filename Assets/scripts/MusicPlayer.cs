using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] music;

    private AudioSource audioSource;
    private int last = -1;
    private static bool isMuted = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if (!isMuted)
        {
            last = (int)Random.Range(0, music.Length - 1);
            audioSource.clip = music[last];
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying&& !isMuted)
        {
            PlayAudio();
        }

        if(Input.GetKeyDown(KeyCode.M)||Input.GetKeyDown("joystick button 3"))
        {
            isMuted = !isMuted;

            if (isMuted)
            {
                audioSource.Stop();
            }
            else
            {
                PlayAudio();
            }
        }
    }

    private void PlayAudio()
    {
        int id = -1;
        while ((id = (int)Random.Range(0, music.Length - 1)) == last)
        {
            //shuld only prodie fi its a different song
        }
        audioSource.clip = music[id];
        audioSource.Play();
        last = id;
    }
}
