using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource musicSource;
    void Start()
    {
        stopMusic();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playMusic()
    {
        musicSource.Play();
    }
    public void pauseMusic()
    {
        musicSource.Pause();
    }
    public void stopMusic()
    {
        musicSource.Stop();
    }
}
