using System.Collections;
using System.Linq;
using UnityEngine;

namespace Battles.Attack {
    public class Beam : Bullet {
        [SerializeField] private float speed;
        [SerializeField] private float chargeTime;
        [SerializeField] private float lifeTime;
        private Vector3 targetPos;
        private BeamFactory mom;

        private ParticleSystem beamParticle;
        private ParticleSystem chargeParticle;
        private LineRenderer lineRenderer;
        private CapsuleCollider col;

        public void Setup(BeamFactory factory, Vector3 pos) {
            mom = factory;
            targetPos = pos;
            transform.LookAt(targetPos);
        }

        private void Awake() {
            var temp = GetComponentsInChildren<ParticleSystem>();
            beamParticle = temp.First();
            chargeParticle = temp.Last();
            lineRenderer = GetComponent<LineRenderer>();
            col = GetComponent<CapsuleCollider>();
        }

        private IEnumerator Start() {
            chargeParticle.Play();
            yield return new WaitForSeconds(chargeTime);
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
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            mom.Hit(other);
        }
    }
}
