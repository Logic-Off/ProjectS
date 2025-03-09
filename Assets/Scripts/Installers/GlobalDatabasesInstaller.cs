using Ecs.Shared;
using UnityEngine;
using Zenject;

namespace Installers {
	[CreateAssetMenu(menuName = "Installers/GlobalDatabasesInstaller", fileName = "GlobalDatabasesInstaller")]
	public sealed class GlobalDatabasesInstaller : ScriptableObjectInstaller {
		[SerializeField] private ScenesDatabaseAsset _scenesDatabase;

		public override void InstallBindings() {
			Container.BindInstance(_scenesDatabase).AsSingle();
		}
	}
}