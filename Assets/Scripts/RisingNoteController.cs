using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Handles rising pitch piano notes that play during each tile passed in Maze, along with final jingle.

    The original concept was supposed to be how satisfying it sounds when those sorting algorithm videos finish their final pass:
    https://www.youtube.com/watch?v=kPRA0W1kECg

    Ended up sounding awful... Maybe due due to trying to use a piano sfx? Maybe Pitch formula not entirely acurrate?
    Maybe a smoother instrument would sound better pitching notes between semitones?
    Keeping in since it could be an interesting idea to revisit, but I really needed to cut my losses on this due to time constraints */

public class RisingNoteController : MonoBehaviour
{
    //Audio Clips
    public AudioClip[] Scale;   //2 octave scale with 12 notes split every other semitone. 1 whole step = 1.05946f^2 pitch
    public AudioClip VictoryJingle;

    //Pitch & Volume variables
    const float frequencyConst = 1.05946f;  //Value used to calculate exact music note pitch by applying as a power (ex. 1.05946^2)
    private float stepPitch;
    private int count;
    private int noteCount = 0;
    private int totalSteps;
    private float startingVolume = 1;

    public AudioSource[] audioSources;  //Need 2 audio sources to prevent jarring sound of notes cutting off prematurely during their waveform

    void Start()
    {
        startingVolume = audioSources[0].volume;
    }

    public void CreatePitchArray(int steps){ //See how many spaces are passed over to solve the maze, and determine how many pitches are needed between low G & high G
        count = 0;
        noteCount = 0;
        totalSteps = steps;

        audioSources[0].clip = Scale[0];
        audioSources[1].clip = Scale[0];

        stepPitch = (24 / (float)steps);
    }

    public void PlayNextNote(){ //Alternate every other Audio Source when changing pitch & clip, called by MazeMan every empty space trigger
        int nextCall = Mathf.Abs(count%2);

        while (Mathf.Pow(frequencyConst, (stepPitch * count)) - Mathf.Pow(frequencyConst, noteCount) > frequencyConst){    //Check if current pitch is over next clip's frequency
            noteCount++;
            audioSources[0].clip = Scale[noteCount];
            audioSources[1].clip = Scale[noteCount];
        }

        audioSources[nextCall].pitch = (Mathf.Pow(frequencyConst, stepPitch * count) - Mathf.Pow(frequencyConst, noteCount) + 1);
        audioSources[nextCall].Play();

        count++;
    }

    public void PlayVictoryJingle(){
        int nextCall = Mathf.Abs(count%2);

        audioSources[nextCall].clip = VictoryJingle;
        audioSources[nextCall].pitch = 1;
        audioSources[nextCall].Play();
    }

    public void MuteAudio(bool mute){  //Turn on/off audio, called by GameManager Toggle Button
        float newVolume;
        if (mute){
            newVolume = 0;
        } else {
            newVolume = startingVolume;
        }

        audioSources[0].volume = newVolume;
        audioSources[1].volume = newVolume;
    }
}
