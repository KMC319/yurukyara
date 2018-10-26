using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Battles.Systems;

//http://wordpress.notargs.com/blog/blog/2015/01/31/unity%E6%9C%80%E3%82%82%E3%82%B7%E3%83%B3%E3%83%97%E3%83%AB%E3%81%AA%E3%83%9D%E3%83%BC%E3%82%BA%E5%87%A6%E7%90%86/

/// <summary>
/// Rigidbodyの速度を保存しておくクラス
/// </summary>
public class RigidbodyVelocity
{
	public Vector3 velocity;
	public Vector3 angularVeloccity;
	public RigidbodyVelocity(Rigidbody rigidbody)
	{
		velocity = rigidbody.velocity;
		angularVeloccity = rigidbody.angularVelocity;
	}
}

public class Pausable : MonoBehaviour {

	/// <summary>
	/// 現在Pause中か？
	/// </summary>
	[SerializeField]private bool pausing;

	/// <summary>
	/// 無視するGameObject
	/// </summary>
	public GameObject[] ignoreGameObjects;

	/// <summary>
	/// Rigidbodyのポーズ前の速度の配列
	/// </summary>
	RigidbodyVelocity[] rigidbodyVelocities;

	/// <summary>
	/// ポーズ中のRigidbodyの配列
	/// </summary>
	Rigidbody[] pausingRigidbodies;

	/// <summary>
	/// ポーズ中のMonoBehaviourの配列
	/// </summary>
	MonoBehaviour[] pausingMonoBehaviours;

	[SerializeField] private GameObject pauseMenu;
	
	
	private void Start(){
	}

	/// <summary>
	/// アクセスポイント
	/// </summary>
	public void Submit(){
		if (pausing){
			Resume();
		}else{
			Pause();
		}

		pausing = !pausing;
	}

	/// <summary>
	/// 中断
	/// </summary>
	void Pause() {
		// Rigidbodyの停止
		// 子要素から、スリープ中でなく、IgnoreGameObjectsに含まれていないRigidbodyを抽出
		Predicate<Rigidbody> rigidbodyPredicate = 
			obj => !obj.IsSleeping() && 
				   Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
		pausingRigidbodies = Array.FindAll(transform.GetComponentsInChildren<Rigidbody>(), rigidbodyPredicate);
		rigidbodyVelocities = new RigidbodyVelocity[pausingRigidbodies.Length];
		for(int i = 0; i < pausingRigidbodies.Length; i++)
		{
			// 速度、角速度も保存しておく
			rigidbodyVelocities[i] = new RigidbodyVelocity(pausingRigidbodies[i]);
			pausingRigidbodies[i].Sleep ();
		}

		// MonoBehaviourの停止
		// 子要素から、有効かつこのインスタンスでないもの、IgnoreGameObjectsに含まれていないMonoBehaviourを抽出
		Predicate<MonoBehaviour> monoBehaviourPredicate = 
			obj => obj.enabled && 
				   obj != this && 
				   Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
		pausingMonoBehaviours = Array.FindAll(transform.GetComponentsInChildren<MonoBehaviour>(), monoBehaviourPredicate);
		foreach(var monoBehaviour in pausingMonoBehaviours)
		{
			monoBehaviour.enabled = false;
		}

		foreach (var item in transform.GetComponentsInChildren<IPauseObserver>()){
			item.Pause();
		}
		pauseMenu.SetActive(true);
	}

	/// <summary>
	/// 再開
	/// </summary>
	void Resume() {
		pauseMenu.SetActive(false);
		// Rigidbodyの再開
		for(int i = 0; i < pausingRigidbodies.Length; i++)
		{
			pausingRigidbodies[i].WakeUp();
			pausingRigidbodies[i].velocity = rigidbodyVelocities[i].velocity;
			pausingRigidbodies[i].angularVelocity = rigidbodyVelocities[i].angularVeloccity;
		}

		// MonoBehaviourの再開
		foreach(var monoBehaviour in pausingMonoBehaviours)
		{
			monoBehaviour.enabled = true;
		}
		
		foreach (var item in transform.GetComponentsInChildren<IPauseObserver>()){
			item.Resume();
		}
	}
}