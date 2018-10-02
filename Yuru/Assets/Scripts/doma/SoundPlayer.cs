using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace doma{
	public class SoundPlayer : MonoBehaviour{
		private List<AudioSource> sounds;

		private void Awake(){
			sounds=transform.GetComponentsInChildren<AudioSource>().ToList();
		}

		public void SoundPlay(int num){
			sounds[num].PlayOneShot(sounds[num].clip);
		}

		public void SoundStop(int num){
			sounds[num].Stop();
		}
	}
}