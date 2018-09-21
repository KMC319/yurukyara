using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour {
    private Rigidbody rigid;
    private Transform lookTarget;
    private Animator anim;

    [SerializeField] private float speed;

    // Use this for initialization
    void Start() {
        rigid = GetComponent<Rigidbody>();
        lookTarget = transform.Find("LookTarget");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Move3D() {
        var z = lookTarget.forward * Input.GetAxis("Vertical") * speed;
        var x = lookTarget.right * Input.GetAxis("Horizontal") * speed;
        if (Vector3.Magnitude(z + x) == 0) {
            Move3D(0);
            return;
        }
        rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + z + x;
        transform.LookAt(transform.position + rigid.velocity);
        anim.SetBool("Runflg", true);
    }

    public void Move3D(float zero) {
        rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
        rigid.angularVelocity = Vector3.zero;
        transform.rotation = lookTarget.rotation;
        anim.SetBool("Runflg", false);
    }
    public void Move2D() {
        var z = lookTarget.forward * Input.GetAxis("Horizontal") * speed;
        if (Vector3.Magnitude(z) == 0) {
            Move2D(0);
            return;
        }
        rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + z;
        transform.LookAt(transform.position + rigid.velocity);
        anim.SetBool("Runflg", true);
    }
    public void Move2D(float zero) {
        rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
        rigid.angularVelocity = Vector3.zero;
        transform.rotation = lookTarget.rotation;
        anim.SetBool("Runflg", false);
    }
}
