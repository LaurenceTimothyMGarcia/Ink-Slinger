using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // the sounds in sfx can be played using the PlaySFX function
    // eventually I wanna change this to use PlayOneShot
    // so that multiple instances of the same sound can be queued quickly
    // and can overlap itself
    public Sound[] sfx;

    // the sounds in music can be played using the PlayMusic function
    // this 
    public Sound[] music;
    string currentSong = null;

    public static AudioManager instance;

    void Awake() {

        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in sfx) {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;


        }

        foreach(Sound s in music) {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;


        }
    }

    void Start(){
        PlayMusic("MainTheme");
    }

    // IEnumerator testSounds() {
    //     Debug.LogWarning("hi");
    //     PlayMusic("testLoop");
    //     yield return new WaitForSeconds(5);
    //     Debug.LogWarning("hi again");
    //     PlayMusic("testLoop2");
    //     yield return new WaitForSeconds(2);
    //     Debug.Log("playing shark noise...");
    //     PlaySFX("sharkDetect");
    //     yield return new WaitForSeconds(2);
    //     Debug.Log("playing death noise...");
    //     PlaySFX("enemyDeath");
    //     yield return new WaitForSeconds(2);
    //     Debug.Log("playing angler noise...");
    //     PlaySFX("anglerBite");
    // }

    public void PlaySFX(string name) {
        Sound s = Array.Find(sfx, sound => sound.name == name);
        if(s == null) {
            Debug.LogWarning("Sound effect not found: " + name + "; check your spelling");
            return;
        }
        s.source.Play();
    }

    public void PlayMusic(string name) {
        Sound s = Array.Find(music, sound => sound.name == name);
        // verify that the requested song exists
        if(s == null) {
            Debug.LogWarning("Song not found: " + name + "; check your spelling");
            return;
        }

        // stop the current playing song if there is one
        if(currentSong != null) {
            Debug.Log("another song is playing..!");
            Sound currentlyPlayingSong = Array.Find(music, sound => sound.name == currentSong);
            // for now this should sound super abrupt; that is because i am doing nothing to fade out the song
            currentlyPlayingSong.source.Stop();
        }

        // and then play the next song
        s.source.Play();
        currentSong = name;
    }
}
