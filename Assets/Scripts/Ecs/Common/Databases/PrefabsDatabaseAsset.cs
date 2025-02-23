using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Ecs.Common {
	
	[CreateAssetMenu(menuName = "Databases/PrefabsDatabase", fileName = "PrefabsDatabase")]
	public sealed class PrefabsDatabaseAsset : ScriptableObjectInstaller {
		public List<PrefabData> All;
		public override void InstallBindings() => Container.BindInstance(this);
	}
}