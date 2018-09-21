using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTarget : MonoBehaviour {
	[SerializeField] private GameObject target;
	// Use this for initialization
	void Start () {
		
	}

	private void Update() {
		transform.LookAt(target.transform);
	}

	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt(target.transform);
	}
	
}
