using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pilot : MonoBehaviour {

	public float turnSpeed = 1f;
	public float speed = 1f;
	public float maxAccelerationMultiplier = 1.2f;
	public float minAccelerationMultiplier = 0.8f;
	public int tapMistakeLives = 3;
	public float fallControlSpeed = 1f;

	int rTapMistakeCount = 0;
	int lTapMistakeCount = 0;

	float rCSpeed = 0f; //Right current speed
	float lCSpeed = 0f; //Left current speed

	float rTimeLastTapped = -1;
	float rAccelerationLimit = 1;


	float lTimeLastTapped = -1;
	float lAccelerationLimit = 1;

	

	bool spinningOut = false;

	Gyroscope gyro;

	Rigidbody2D rb;

	private Vector3 startEulerAngles;
	private Vector3 startGyroAttitudeToEuler;

	private void Start() {
		rb = this.GetComponent<Rigidbody2D>();
		rTapMistakeCount = 0;

		gyro = Input.gyro;
		if (!gyro.enabled) {
			gyro.enabled = true;
		}
		startEulerAngles = transform.eulerAngles;
		startGyroAttitudeToEuler = Input.gyro.attitude.eulerAngles;
	}

	public bool IsSpinning(){
		return spinningOut;
	}

	bool TapStrike(){
		if(rTapMistakeCount < tapMistakeLives--){
			rTapMistakeCount++;
			return false;
		}
		rTapMistakeCount = 0;
		return true;
	}

	public bool Tap(bool isRightEngine){
		if (isRightEngine) {
			if (rTimeLastTapped != -1) {
				float timeBetweenTaps = Time.time - rTimeLastTapped;
				if (timeBetweenTaps > rAccelerationLimit * maxAccelerationMultiplier) {
					if (TapStrike()) {
						SpinOut();
					}
					else {
						//rCSpeed -= speed;
						//rAccelerationLimit = timeBetweenTaps * 2;
					}
					return false;
				}
				/* Obselete due to no minimum thrust punishment
				if(timeBetweenTaps < rAccelerationLimit * minAccelerationMultiplier){
					if (TapStrike()) {
						SpinOut();
					}
					else {
						rCSpeed -= speed;
					}
					return false;
				}*/
				if(rTapMistakeCount > 0){ rTapMistakeCount--; }
				rAccelerationLimit = timeBetweenTaps;
				rCSpeed += speed;
				rTimeLastTapped = Time.time;
				return true;
			} //No previous tap
			rTapMistakeCount = 0;
			rAccelerationLimit = 1;
			rCSpeed += speed;
			rTimeLastTapped = Time.time;
			return true;
		}
		else{
			if (lTimeLastTapped != -1) {
				float timeBetweenTaps = Time.time - lTimeLastTapped;
				if (timeBetweenTaps > lAccelerationLimit * maxAccelerationMultiplier) {
					if (TapStrike()) {
						SpinOut();
					}
					else {
						//lCSpeed -= speed;
					}
					return false;
				}
				if (lTapMistakeCount > 0) { lTapMistakeCount--; }
				lAccelerationLimit = timeBetweenTaps;
				lCSpeed += speed;
				lTimeLastTapped = Time.time;
				return true;
			} //No previous tap
			lTapMistakeCount = 0;
			lAccelerationLimit = 1;
			lCSpeed += speed;
			lTimeLastTapped = Time.time;
			return true;
		}
	}

	public float GetRawSpeed(){
		return rCSpeed + lCSpeed;
	}

	public float GetSpeed(){
		return (rCSpeed + lCSpeed) / 2;
	}

	void SpinOut(){
		spinningOut = true;
		rb.gravityScale = 1;
		StartCoroutine(RecoverTimer(GetSpeed()));
		rCSpeed = 0;
		lCSpeed = 0;
	}

	IEnumerator RecoverTimer(float speed){
		Debug.Log("Recovery Time:" + speed * 0.2f);
		yield return new WaitForSeconds(speed * 0.2f);
		Recover();
	}

	void Recover(){
		if (spinningOut) {
			spinningOut = false;
			rTimeLastTapped = -1;
			rAccelerationLimit = 10;
			lTimeLastTapped = -1;
			lAccelerationLimit = 10;
			rb.gravityScale = 0.2f;
		}
	}

	/*public void SwitchGyro(){ //Gyro is Obselete
		startGyroAttitudeToEuler = Input.gyro.attitude.eulerAngles;
	}*/
	
	public float TurnModifier(){
		float i = rCSpeed - lCSpeed;
		i = i / 2;
		return i;
	}

	private void FixedUpdate() {
		/*Vector3 deltaEulerAngles = Input.gyro.attitude.eulerAngles - startGyroAttitudeToEuler;
		deltaEulerAngles.x = 0.0f;
		deltaEulerAngles.y = 0.0f;

		transform.eulerAngles = startEulerAngles - deltaEulerAngles;
	
		if(spinningOut){
			if (transform.eulerAngles.z > 0.1f){
				Move(transform.up * fallControlSpeed);
			}
			else if (transform.eulerAngles.z < 0.1f){
				Move(transform.up * fallControlSpeed);
			}
		}*/
		transform.Rotate(transform.forward, TurnModifier());

		if(spinningOut){
			Move(transform.up * fallControlSpeed * -1);
		}
		else{
			Move(transform.up * GetSpeed());
		}
	}

	void Move(Vector2 movement) {
		rb.MovePosition(rb.position + movement * Time.deltaTime);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		GameObject other = collision.otherCollider.gameObject;
		if(other.tag == "Obstacle"){
			SpinOut();
		}
		if (other.tag == "Ship") {
			SpinOut();
		}
		if (other.tag == "RecoveryPoint"){
			Recover();
		}
	}
}
