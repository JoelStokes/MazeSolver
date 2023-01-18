using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Handle Music & SFX
public class AudioManager : MonoBehaviour
{
    //Audio Source Management
    public AudioSource MusicSource;
    public AudioSource SFXSource;
    public float musicVolume;
    public float sfxVolume;

    //SFX
    public AudioClip[] footstepSFXs;
    public AudioClip cheerSFX;
    public AudioClip errorSFX;

    //Footstep 
    private int footCounter;
    private int footLim = 2;    //Only play footstep sounds every 3rd tile reveal. Sounds better & prevents

    void Start()
    {
        footCounter = footLim+1;
        MuteVolume(false);
    }

    public void PrepareFootstep(){  //Called , primes footstep by incrementing the counter until footstep plays
        footCounter++;
        if (footCounter > footLim){
            PlaySFX(footstepSFXs[Random.Range(0,footstepSFXs.Length)]);
            footCounter -= footLim;
        }
    }

    public void ResetFootCount(){
        footCounter = footLim+1;
    }

    public void PrepareCheer(){
        PlaySFX(cheerSFX);
    }

    public void PrepareError(){
        PlaySFX(errorSFX);
    }

    private void PlaySFX(AudioClip newClip){
        SFXSource.clip = newClip;
        SFXSource.Play();
    }

    public void MuteVolume(bool mute){ //Toggle all sounds on/off
        if (mute){
            MusicSource.volume = 0;
            SFXSource.volume = 0;
        } else {
            MusicSource.volume = musicVolume;
            SFXSource.volume = sfxVolume;
        }
    }
}
