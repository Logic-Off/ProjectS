// using System;
//
// namespace Utopia {
// 	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
// 	public class InstallerGeneratorAttribute : Attribute {
// 		public string InstallerName;
// 		public string InstallType;
// 		public string BindType;
// 		public int Order;
//
// 		public InstallerGeneratorAttribute(string installerName) {
// 			InstallerName = installerName;
// 			Order = 100_000;
// 			InstallType = nameof(EInstallType.None);
// 			BindType = nameof(EBindType.BindInterfacesAndSelfTo);
// 		}
//
// 		public InstallerGeneratorAttribute(string installerName, int order) {
// 			InstallerName = installerName;
// 			Order = order;
// 			InstallType = nameof(EInstallType.None);
// 			BindType = nameof(EBindType.BindInterfacesAndSelfTo);
// 		}
//
// 		public InstallerGeneratorAttribute(string installerName, int order, string installType) {
// 			InstallerName = installerName;
// 			Order = order;
// 			InstallType = installType.ToString();
// 			BindType = nameof(EBindType.BindInterfacesAndSelfTo);
// 		}
//
// 		public InstallerGeneratorAttribute(string installerName, int order, string installType, string bindType) {
// 			InstallerName = installerName;
// 			Order = order;
// 			InstallType = installType.ToString();
// 			BindType = bindType.ToString();
// 		}
// 	}
//
// 	public enum EInstallType {
// 		None,
// 		NonLazy
// 	}
//
// 	public enum EBindType {
// 		BindInterfacesAndSelfTo,
// 		BindInterfacesTo
// 	}
// }