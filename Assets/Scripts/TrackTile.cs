using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable, RequireComponent(typeof(PathCreator))]
public class TrackTile : MonoBehaviour {

	int endpoint;
	int startpoint;

	public int width;
	public int height;

	public string seed;
	public bool randomizeSeed;
	

	//Meteor/Block Generation
	int[,] blockMap;
	[Range(0, 100)]
	public int blockFillPercent;

	void GenerateBlockMap(){
		blockMap = new int[width, height];
		if(randomizeSeed){
			seed = Time.time.ToString();
		}

		System.Random pRado = new System.Random(seed.GetHashCode());
		
		for(int x = 0; x < width; x++){
			for (int y = 0; y < width; y++) {
			if(x == 0 || x == width - 1 || y == 0 || y == height -1){
				blockMap[x, y] = 0;
			}
				blockMap[x, y] = (pRado.Next(0, 100) < blockFillPercent) ? 1 : 0;
			}
		}
	}
}
