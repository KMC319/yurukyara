using System;
using System.Collections;
using System.Collections.Generic;
using doma;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animations{
	public class PlayAbleController : MonoBehaviour{	
		[SerializeField]private float transtime;

		[SerializeField]private AnimBox currentAnim;
		
		[SerializeField]private AnimBox defoultAnim;

		private AnimationMixerPlayable mixer;
		
		private AnimationClipPlayable prePlayable; 
		private AnimationClipPlayable currentPlayable;		
		
		private PlayableGraph graph;


		private void Awake(){
			graph = PlayableGraph.Create ();	
			var anim = GetComponent<Animator>();


			currentAnim=defoultAnim ;
			var output = AnimationPlayableOutput.Create (graph, "output", anim);
			mixer = AnimationMixerPlayable.Create(graph, 2);
			output.SetSourcePlayable(mixer);
			graph.Play ();

			ConnectProcess(defoultAnim, true);
		}

		private void Update(){
			if (isPlayFinish(transtime)){
				if (currentAnim.loop){
					currentPlayable.SetTime(0);
				} else{
					if (currentAnim.autoAdvance != null&&currentAnim.autoAdvance.Length > 0){
						if (currentAnim.autoAdvance[0].clip != null){
							TransAnimation(currentAnim.autoAdvance[0]);
						}	
					}
				}
			}
		}
		
		public bool isPlayFinish(float transT){
			if (!currentPlayable.IsValid()){
				return false;
			}

			if (currentPlayable.GetTime()+transT> currentPlayable.GetAnimationClip().length){
				return true;
			}
			return false;
		}

		private void OnDestroy(){
			graph.Destroy();
		}
	

		/*
		//<public Methods>
		*/
		public void TransAnimation(AnimBox anim_box){
			if (ConnectProcess(anim_box)){
				StartCoroutine(Trans());
			}
		}

		public void ForcePauseAnimation(){
			currentPlayable.Pause();
		}
		/*
		//</public Methods>
		*/

	
		/*
		//<private Methods>
		*/
		
		private bool ConnectProcess(AnimBox anim_box,bool force=false){
			if (anim_box ==  currentAnim&&!force){
				//DebugLogger.LogWarning(anim_box.clip+" is play now!!");
				return false;
			}
			graph.Disconnect(mixer, 0);
			graph.Disconnect(mixer, 1);
			if (prePlayable.IsValid()){
				prePlayable.Destroy();
			}
			prePlayable = currentPlayable;
			currentAnim=anim_box;
			currentPlayable = AnimationClipPlayable.Create(graph, anim_box.clip);
			mixer.ConnectInput(1, prePlayable, 0);
			mixer.ConnectInput(0, currentPlayable, 0);
			return true;
		}
		
		/*
		//</private Methods>
		*/


		private IEnumerator Trans(){
			var wait_time = Time.timeSinceLevelLoad + transtime;
			yield return new WaitWhile(() =>{
				var diff = wait_time - Time.timeSinceLevelLoad;
				if (diff <= 0){
					mixer.SetInputWeight(1, 0);
					mixer.SetInputWeight(0, 1);
					return false;
				}else{
					var rate = Mathf.Clamp01(diff / transtime);
					mixer.SetInputWeight(1, rate);
					mixer.SetInputWeight(0, 1 - rate);
					return true;
				}
			});
		}

	}
}