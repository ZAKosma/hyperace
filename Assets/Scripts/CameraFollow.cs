using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float speed;
	public bool snapToTarget;
	public float height;

	private void Update() {
		MoveTowardsTarget();
	}

	Vector3 MoveTowardsTarget(){
		Vector3 newPos = new Vector3(0, 0, -height);
		newPos.y = Mathf.Lerp(transform.position.y,
								target.position.y,
								speed);
		this.transform.position = newPos;
		return this.transform.position;
	}
}
