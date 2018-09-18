using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour {
    private Rigidbody rigid;
    private Transform lookTarget;
    
    [SerializeField] private float speed;

    // Use this for initialization
    void Start() {
        rigid = GetComponent<Rigidbody>();
        lookTarget = transform.Find("LookTarget");
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Move() {
        var z = lookTarget.forward * Input.GetAxis("Vertical") * speed;
        var x = lookTarget.right * Input.GetAxis("Horizontal") * speed;
        rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + z + x;
        transform.LookAt(transform.position + rigid.velocity);
    }

    public void Move(float zero) {
        rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
        rigid.angularVelocity = Vector3.zero;
    }
}
