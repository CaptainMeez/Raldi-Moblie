using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public FunkinCharacter chara;
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public KeyCode secondaryInput;
    public GameObject notePrefab;
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    int inputIndex = 0;

    public bool opponentControlled;
    public SpriteRenderer staticArrow;

    public Sprite arrow;
    public Sprite arrowPressed;

    public SingDirection direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        { 
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Math.Abs(audioTime - timeStamp) < marginOfError && opponentControlled)
            {
                if (chara != null)
                    chara.Sing(direction);
                    
                Destroy(notes[inputIndex].gameObject);
                inputIndex++;
            }

            if ((Input.GetKey(input) || Input.GetKey(secondaryInput)) && !opponentControlled)
            {
                staticArrow.transform.localScale = new Vector3(staticArrow.transform.localScale.x, 0.15f, staticArrow.transform.localScale.z);
                staticArrow.color = Color.grey;

                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                    staticArrow.color = Color.white;
            }
            else
            {
                staticArrow.transform.localScale = new Vector3(staticArrow.transform.localScale.x, 0.2f, staticArrow.transform.localScale.z);
                staticArrow.color = Color.white;
            }

            if (Input.GetKeyDown(input) || Input.GetKeyDown(secondaryInput))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
            }

            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                inputIndex++;
            }
        }       
    
    }
    private void Hit()
    {
        ScoreManager.Hit();

        if (chara != null)
            chara.Sing(direction);
    }
    private void Miss()
    {
        if (!opponentControlled)
            ScoreManager.Miss();
    }
}
