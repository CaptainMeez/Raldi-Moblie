using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoulsonEngine.UI
{
    public class FPSCounter : MonoBehaviour
    {
        public UnityEngine.UI.Text fpsDisplay;

		int fps;
		int countedFrames;
		float timer;

        void Update() 
        {
			timer += Time.unscaledDeltaTime;

			if (timer >= 1.0f)
			{
				fps = countedFrames;
				countedFrames = 0;
				timer = 0.0f;
			}

			countedFrames++;
            fpsDisplay.text = Mathf.RoundToInt(fps) + "fps"; 
        }
    }
}
