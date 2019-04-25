using System;
using System.Linq;
using UnityEngine;

namespace Battles.Effects {
    [RequireComponent(typeof(AudioSource))]
    public class SoundEffectControl : MonoBehaviour {
        private AudioSource audioSource;
        [SerializeField] private SoundStatus[] sounds;

        private void Start() {
            audioSource = GetComponent<AudioSource>();
        }

        public void Play(AudioClip clip) {
            audioSource.PlayOneShot(clip);
        }

        public void Play(ESoundType soundType) {
            audioSource.PlayOneShot(sounds.First(i => i.Type == soundType).Clip);
        }
        
        [Serializable]
        private struct SoundStatus {
            public AudioClip Clip;
            public ESoundType Type;
        }
    }
}
