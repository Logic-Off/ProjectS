using Ecs.Ability;
using Ecs.Animations;
using Ecs.Character;
using Ecs.Common;
using Ecs.Game;
using UnityEngine;
using Zenject;

namespace Installers {
	[CreateAssetMenu(menuName = "Installers/LocalDatabasesInstaller", fileName = "LocalDatabasesInstaller")]
	public sealed class LocalDatabasesInstaller : ScriptableObjectInstaller {
		[SerializeField] private PrefabsDatabaseAsset _prefabsDatabase;
		[SerializeField] private CharacterDatabaseAsset _characterDatabaseAsset;
		[SerializeField] private GameCollisionDatabaseAsset _collisionDatabaseAsset;
		[SerializeField] private AbilitiesDatabaseAsset _abilitiesDatabaseAsset;
		[SerializeField] private AnimationsDatabaseAsset _animationsDatabaseAsset;
		

		public override void InstallBindings() {
			Container.BindInstance(_prefabsDatabase).AsSingle();
			Container.BindInstance(_characterDatabaseAsset).AsSingle();
			Container.BindInstance(_collisionDatabaseAsset).AsSingle();
			Container.BindInstance(_abilitiesDatabaseAsset).AsSingle();
			Container.BindInstance(_animationsDatabaseAsset).AsSingle();
		}
	}
}