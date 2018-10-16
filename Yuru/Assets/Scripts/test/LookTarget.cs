using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;

public class LookTarget : MonoBehaviour {
	[SerializeField] public GameObject target;
	private Vector3 targetDistance;

	private PlayerRootControll rootControll;
	// Use this for initialization
	void Start () {
		rootControll = transform.parent.GetComponent<PlayerRootControll>();
	}

	private void Update() {
		transform.position = new Vector3(transform.position.x,0,transform.position.z);
	}

	// Update is called once per frame
	void LateUpdate () {
		if (!rootControll.currentPlayerMove.InJumping) {
			targetDistance = target.transform.position - transform.position;
		}
		transform.LookAt(transform.position + targetDistance);
	}
	
}
