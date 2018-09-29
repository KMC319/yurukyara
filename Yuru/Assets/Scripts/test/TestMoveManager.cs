using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battles.Systems;
using UnityEngine;

public class TestMoveManager : IChangePhase {
	public int nowNum;
	private TestMove[] scripts;
	private PhaseManager phaseManager;
	
	// Use this for initialization
	void Start () {
		scripts = GameObject.FindGameObjectsWithTag("Player").Select(i => i.GetComponent<TestMove>()).ToArray();
		phaseManager = PhaseManager.Instance;
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

		

	}

	public void ChangePhase(Phase changedPhase) {
		switch (changedPhase) {
			case Phase.P3D:
				Array.ForEach(scripts.Where((script, index) => index != nowNum).ToArray(), i => i.Move3D(0));
				if(nowNum >= scripts.Length) return;
				scripts[nowNum].Move3D();
				break;
			case Phase.P2D:
				Array.ForEach(scripts.Where((script, index) => index != nowNum).ToArray(), i => i.Move2D(0));
				if(nowNum >= scripts.Length) return;
				scripts[nowNum].Move2D();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}
