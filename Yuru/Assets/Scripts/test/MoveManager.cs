using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveManager : MonoBehaviour {
	public int nowNum;
	private TestMove[] scripts;
	
	// Use this for initialization
	void Start () {
		scripts = GameObject.FindGameObjectsWithTag("Player").Select(i => i.GetComponent<TestMove>()).ToArray();
		nowNum = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			nowNum = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			nowNum = 1;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			nowNum = 2;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			nowNum = 3;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			nowNum = 4;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			nowNum = 5;
		}

		foreach (var script in scripts) {
			script.Move(0);
		}
		if(nowNum >= scripts.Length) return;
		scripts[nowNum].Move();

	}
}
