using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Asteroid : MonoBehaviour {

	private void OnCollisionEnter2D(Collision2D collision) {
		//Play destruction sound / animation
		Destroy(this.gameObject);
	}
}
