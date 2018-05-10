using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerButton : MonoBehaviour {

	public Pilot ship;

	public bool isRight;

	private void OnMouseDown() {
		if (!ship.IsSpinning()) {
			Debug.Log(ship.Tap(isRight) + " " + ship.GetSpeed());

		}
	}
}
