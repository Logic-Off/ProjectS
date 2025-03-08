using Ecs.Character;
using UnityEngine;
using Zenject;

namespace Installers {
	[CreateAssetMenu(menuName = "Installers/LocalDatabasesInstaller", fileName = "LocalDatabasesInstaller")]
	public sealed class LocalDatabasesInstaller : ScriptableObjectInstaller {
		[SerializeField] private CharacterDatabaseAsset _characterDatabaseAsset;

		public override void InstallBindings() {
			Container.BindInstance(_characterDatabaseAsset).AsSingle();
		}
	}
}