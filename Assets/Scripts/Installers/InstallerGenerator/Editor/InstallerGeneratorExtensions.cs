using UnityEditor;

namespace Installers {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public class InstallerGeneratorExtensions {
		[MenuItem("Tools/Generate All Installers")]
		public static void Generate() {
			InstallerGenerator.Generate();
			InstallerPrefabGenerator.Generate();
			InstallerUiPrefabGenerator.Generate();
			AssetDatabase.SaveAssets();
		}
	}
}