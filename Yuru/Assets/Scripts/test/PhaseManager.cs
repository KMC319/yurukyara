using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum Phase {
	P3D,P2D
}

public class PhaseManager : MonoBehaviour {
	private Transform[] players;
	private GameObject[] cameras;

	private Phase nowPhase;
	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player").Select(i => i.transform).ToArray();
		cameras = GameObject.FindGameObjectsWithTag("Camera");
		nowPhase = Phase.P3D;
	}
	
	// Update is called once per frame
	void Update () {
		PointMove();
		switch (nowPhase) {
			case Phase.P3D:
				cameras[1].SetActive(true);
				cameras[0].SetActive(false);
				break;
			case Phase.P2D:
				cameras[1].SetActive(false);
				cameras[0].SetActive(true);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		transform.LookAt(transform.position + players[0].Find("LookTarget").right * 5);
		if (Input.GetKeyDown(KeyCode.P)) nowPhase = (Phase) ((int) (nowPhase + 1) % 2);
	}

	void PointMove() {
		var sumPoint = players.Aggregate(new Vector3(), (current, player) => current + player.position);
		transform.position = sumPoint / players.Length;
	}
	
}
