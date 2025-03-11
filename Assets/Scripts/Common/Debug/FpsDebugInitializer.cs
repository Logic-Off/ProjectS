using System.Diagnostics;
using UnityEngine;
using Utopia;
using Zenject;

namespace Common {
	[InstallerGenerator(InstallerId.Project)]
	public class FpsDebugInitializer : IInitializable {
		public void Initialize() => OnCreateFpsBehaviour();

		[Conditional("DEBUG")]
		private void OnCreateFpsBehaviour() {
			var gameObject = new GameObject("Fps");
			gameObject.AddComponent<FpsBehaviour>();
		}
	}
}