using UnityEngine;

namespace Battles.Attack{
	[RequireComponent(typeof(AudioSource))]
	public class AttackSoundPlayer :IAttackTool{
		private AudioSource sound;

		private void Awake(){
			sound = this.GetComponent<AudioSource>();
		}

		public override void On(){
			sound.Play();	
		}

		public override void Off(bool cancel = false){
			sound.Stop();
		}
	}
}