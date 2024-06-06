using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace CoulsonEngine.Sound.Music
{
	public class MusicBeat : MonoBehaviour
	{
		public float BPM;
		private float _lastSyncTime = 0;
		private uint _beatsSinceSync = 0;
		public UnityEvent beatHit;

		void Start() 
		{
			syncBPM();
		}

		void Update() 
		{
			if (Time.time > _lastSyncTime + (beatsPerMinuteToDelay(BPM) * _beatsSinceSync))
				beat();
		}

		private static float beatsPerMinuteToDelay(float beatsPerMinute) 
		{
			return 1.0f / (beatsPerMinute / 60.0f);
		}

		private void beat() 
		{
			_beatsSinceSync++;
			beatHit.Invoke();
		}

		public void syncBPM() 
		{
			_lastSyncTime = Time.time;
			_beatsSinceSync = 0;
			beat();
		}
	}
}

