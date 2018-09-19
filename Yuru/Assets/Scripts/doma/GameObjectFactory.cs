using UnityEngine;
using Zenject;

namespace doma{
	/// <summary>
	/// https://qiita.com/snowhork/items/95dcf75dd4d4608e924e
	/// </summary>
	///
	public class GameObjectFactory : IFactory<GameObject>
	{
		DiContainer _container;
		protected UnityEngine.Object _prefab;

		[Inject]
		public void Construct(
			UnityEngine.Object prefab,
			DiContainer container)
		{
			_container = container;
			_prefab = prefab;
		}

		public GameObject Create()
		{
			return _container.InstantiatePrefab(_prefab);
		}
	}
}