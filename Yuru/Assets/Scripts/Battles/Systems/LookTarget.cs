using Battles.Players;
using UnityEngine;

namespace Battles.Systems{
	public class LookTarget : MonoBehaviour {
		[SerializeField] public GameObject target;
		private Vector3 targetDistance;

		private PlayerRoot root;
		// Use this for initialization
		void Start () {
			root = transform.parent.GetComponent<PlayerRoot>();
		}

		private void Update() {
			transform.position = new Vector3(transform.position.x,0,transform.position.z);
		}

		// Update is called once per frame
		void LateUpdate () {
			if (!root.CurrentMoveCotroll.InJumping && transform.parent.position.y-target.transform.parent.position.y < 0.5f) {
				targetDistance = target.transform.position - transform.position;
			}
			transform.LookAt(transform.position + targetDistance);
		}
	
	}
}
