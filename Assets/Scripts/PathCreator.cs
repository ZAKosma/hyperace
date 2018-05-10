using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EdgeCollider2D))]
public class PathCreator : MonoBehaviour {

	[HideInInspector]
	public Path path;

	EdgeCollider2D edge;

	public int pathDiameter;

	public void CreatePath() {
		path = new Path(transform.position);
	}

	private void Awake() {
		edge = this.gameObject.GetComponent<EdgeCollider2D>();
		edge.points = path.GetPointsInSegment(0);
		edge.edgeRadius = pathDiameter / 2; 
	}
}