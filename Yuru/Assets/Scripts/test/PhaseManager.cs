using System;
using System.Linq;
using Cinemachine;
using UnityEngine;

public enum Phase {
	P3D,P2D
}
[DefaultExecutionOrder(10)]
public class PhaseManager : MonoBehaviour {
	private Transform[] players;
	private CinemachineVirtualCamera[] cameras;
	private GameObject child;

	public Phase NowPhase { get; private set; }
	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player").Select(i => i.transform).ToArray();
		cameras = GameObject.FindGameObjectsWithTag("Camera").Select(i => i.GetComponent<CinemachineVirtualCamera>()).ToArray();
		NowPhase = Phase.P3D;
		child = transform.Find("child").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		PointMove();
		switch (NowPhase) {
			case Phase.P3D:
				cameras[1].enabled = true;
				break;
			case Phase.P2D:
				cameras[1].enabled = false;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		if (Input.GetKeyDown(KeyCode.P)) NowPhase = (Phase) ((int) (NowPhase + 1) % 2);
	}

	private void LateUpdate() {
		transform.LookAt(transform.position + players[0].Find("LookTarget").right * 5);
	}

	void PointMove() {
		var sumPoint = players.Aggregate(new Vector3(), (current, player) => current + player.position);
		transform.position = sumPoint / players.Length + transform.forward * (Mathf.Clamp(Vector3.Distance(players[0].position, players[1].position),5f,1000f)) + new Vector3(0, 1, 0);
		child.transform.position = sumPoint / players.Length;
	}
	
}
