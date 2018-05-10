using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

	public Pilot pilot;

	private void Start() {
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;

		Screen.orientation = ScreenOrientation.Portrait;
	}
	/*private void OnMouseDown() {
		pilot.SwitchGyro();
	}*/
}
