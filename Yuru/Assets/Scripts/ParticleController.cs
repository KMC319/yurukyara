using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
	private ParticleSystem myParticle;
	void Awake () {
		myParticle = GetComponent<ParticleSystem>();
	}

	public void Playparticle(Color particleColor, float playTime) {
		ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient {mode = ParticleSystemGradientMode.Color, color = particleColor};
		ParticleSystem.MainModule main = myParticle.main;
		main.startColor = color;
		myParticle.Play();
		Invoke(nameof(StopParticle), playTime);
	}

	private void StopParticle() {
		myParticle.Stop();
	}
}
