using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public StrumLine extraLine;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds
    public AudioClip failSound;
    private bool dead = false;

    public int inputDelayInMilliseconds;

    public bool noDeath = false;
    
    public Animator baldi;
    public Animator player;

    public Slider healthBar;
    public float health = 50f;

    public string fileLocation;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;
    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static MidiFile midiFile;

    public UnityEvent onComplete;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }

        extraLine.Init();
        GetComponent<CoulsonEngine.Sound.Music.MusicBeat>().beatHit.AddListener(Dance);
    }

    public void Dance()
    {
        baldi.Play("BaldiFNFIdle");
        player.Play("PlayerFNFIdle");
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);

        IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            GameControllerScript.current.CollectItem(GameControllerScript.current.rewarditems[UnityEngine.Random.Range(0, GameControllerScript.current.rewarditems.Length - 1)]);
            onComplete.Invoke();
        }

        StartCoroutine(WaitTime());
    }

    public void StartSong()
    {
        audioSource.Play();
    }
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    void Update()
    {
        healthBar.value = health;

        if (health < 0f && !dead && !noDeath)
        {
            dead = true;
            audioSource.Stop();
            audioSource.PlayOneShot(failSound);
            foreach(Lane lane in FindObjectsOfType<Lane>()) {lane.enabled = false;}

            IEnumerator Return()
            {
                yield return new WaitForSeconds(failSound.length + 0.5f);
                onComplete.Invoke();
            }
            
            StartCoroutine(Return());
        }

        if (health > 100)
            health = 100;
    }
}
