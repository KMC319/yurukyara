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
	[SerializeField]private GameObject[] cameras;
	private GameObject child;

	public Phase NowPhase { get; private set; }
	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player").Select(i => i.transform).ToArray();
		NowPhase = Phase.P3D;
		child = transform.Find("child").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		PointMove();
		switch (NowPhase) {
			case Phase.P3D:
				cameras[1].SetActive(true);
				cameras[2].SetActive(true);
				break;
			case Phase.P2D:
				cameras[1].SetActive(false);
				cameras[2].SetActive(false);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.O)) NowPhase = (Phase) ((int) (NowPhase + 1) % 2);
	}

	private void LateUpdate() {
		transform.LookAt(transform.position + players[0].Find("LookTarget").right * 5);
	}

	void PointMove() {
		var avePoint = players.Aggregate(new Vector3(), (current, player) => current + player.position) / players.Length;
		transform.position = avePoint + transform.forward * (Mathf.Clamp(Vector3.Distance(players[0].position, players[1].position),5f,1000f)) + new Vector3(0, 2, 0);
		child.transform.position = new Vector3(avePoint.x, 1, avePoint.z);
	}
	
}
