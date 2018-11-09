using System.Collections;
using System.Linq;
using Battles.Systems;
using doma;
using UnityEngine;

namespace Battles.Attack {
    public class Beam : Bullet {
        [SerializeField] private float speed;
        [SerializeField] private float chargeTime;
        [SerializeField] private float lifeTime;
        private Vector3 targetPos;
        private BulletFactory mom;

        private ParticleSystem beamParticle;
        private ParticleSystem chargeParticle;
        private LineRenderer lineRenderer;
        private CapsuleCollider col;

        private IEnumerator myProcess;
        
        public override void Setup(BulletFactory factory, GameObject targetObj) {
            mom = factory;
            targetPos = targetObj.transform.position + new Vector3(0, 1, 0);
            transform.LookAt(targetPos);
            myProcess = StartProcess();
            StartCoroutine(myProcess);
            Initialized = true;
        }

        private void Awake() {
            var temp = GetComponentsInChildren<ParticleSystem>();
            beamParticle = temp.First();
            chargeParticle = temp.Last();
            lineRenderer = GetComponent<LineRenderer>();
            col = GetComponent<CapsuleCollider>();
        }

        private IEnumerator StartProcess() {
            chargeParticle.Play();
            var charge_count = 0.01f;
            while (charge_count<=chargeTime){//クソ程強引なポーズ対応
                yield return new WaitForSeconds(0.01f);
                charge_count += 0.01f;
            }
            chargeParticle.Stop();
            beamParticle.Play();
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            var nowPos = transform.position;
            while (Vector3.Distance(nowPos, targetPos) > speed * Time.deltaTime) {
                lineRenderer.SetPosition(1, nowPos + transform.forward * speed * Time.deltaTime);
                nowPos += transform.forward * speed * Time.deltaTime;
                col.center = new Vector3(0, 0, Vector3.Distance(nowPos, transform.position) / 2f);
                col.height = Vector3.Distance(nowPos, transform.position);
                yield return null;
            }

            col.center = new Vector3(0, 0, Vector3.Distance(targetPos, transform.position) / 2f);
            col.height = Vector3.Distance(targetPos, transform.position);
            lineRenderer.SetPosition(1, targetPos);
            var life_count = 0.01f;
            while (life_count<=lifeTime){
                yield return new WaitForSeconds(0.01f);
                life_count += 0.01f;
            }
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            if (!Initialized) return;
            mom.Hit(other);
        }

        public override void Pause(){
            StopCoroutine(myProcess);
        }

        public override void Resume(){
            StartCoroutine(myProcess);
        }
    }
}
