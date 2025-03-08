using Ecs.Common;
using Ecs.Shared;
using UnityEngine;
using Zenject;

namespace Installers {
	[CreateAssetMenu(menuName = "Installers/GlobalDatabasesInstaller", fileName = "GlobalDatabasesInstaller")]
	public sealed class GlobalDatabasesInstaller : ScriptableObjectInstaller {
		[SerializeField] private ScenesDatabaseAsset _scenesDatabase;
		[SerializeField] private PrefabsDatabaseAsset _prefabsDatabase;

		public override void InstallBindings() {
			Container.BindInstance(_scenesDatabase).AsSingle();
			Container.BindInstance(_prefabsDatabase).AsSingle();
		}
	}
}