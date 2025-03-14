using Ui;
using UnityEngine;
using Zenject;

namespace Installers {
	[CreateAssetMenu(menuName = "Installers/GamePrefabsInstaller", fileName = "GamePrefabsInstaller")]
	public sealed class GamePrefabsInstaller : ScriptableObjectInstaller {
		[SerializeField] private CanvasParent _canvasParent;

		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<CanvasParent>().FromComponentInNewPrefab(_canvasParent).AsSingle();
		}
	}
}