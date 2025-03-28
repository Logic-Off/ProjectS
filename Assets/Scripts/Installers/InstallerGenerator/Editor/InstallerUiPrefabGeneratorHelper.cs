﻿using System.Collections.Generic;
using System.Text;

namespace Installers {
	public partial class InstallerUiPrefabGenerator {
		private static readonly StringBuilder _builder = new();
		private static readonly List<string> _namespaces = new();
		private static readonly Dictionary<string, string> _files = new();

		private static void GenerateFile(string name, List<Entry> entries) {
			_builder.Append(
				@"//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool InstallerGenerator.
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

"
			);
			foreach (var entry in entries) {
				var typeNamespace = entry.BuilderType.Namespace;
				if (!_namespaces.Contains(typeNamespace))
					_namespaces.Add(typeNamespace);
			}

			_builder.Append("using Zenject;\n");
			_builder.Append("using LogicOff.Windows;\n");
			_builder.Append("using UnityEngine;\n");
			_builder.Append("using UnityEngine.AddressableAssets;\n");
			foreach (var ns in _namespaces)
				_builder.Append($"using {ns};\n");
			_builder.Append("\n");
			_builder.Append("[CreateAssetMenu(menuName = \"Installers/")
				.Append($"{name}UiPrefabsInstaller")
				.Append("\", fileName = \"")
				.Append($"{name}UiPrefabsInstaller\")]\n");
			_builder.Append(@"public class ").Append(name).Append("UiPrefabsInstaller : ScriptableObjectInstaller {\n");

			foreach (var entry in entries)
				_builder.Append(@"	[SerializeField] private AssetReference")
					// .Append(entry.ViewType.Name)
					.Append(" ")
					.Append(LowercaseFirst(entry.BuilderType.Name.Replace("Builder", ""), "_"))
					.Append(";\n");

			_builder.Append("\n");
			_builder.Append(@"	public override void InstallBindings() => Install();").Append("\n\n");
			_builder.Append(@"	public void Install() {").Append("\n");
			foreach (var entry in entries)
				_builder.Append(@"		Container.BindUiViewReference<")
					.Append(entry.BuilderType.Name)
					.Append(">(")
					.Append(LowercaseFirst(entry.BuilderType.Name.Replace("Builder", ""), "_"))
					.Append(")")
					.Append(";\n");

			_builder.Append("	}\n"); // Скобка закрывающая метод Install
			_builder.Append("}\n"); // Скобка закрывающая инсталлер

			_files[name] = _builder.ToString();
			_builder.Clear();
			_namespaces.Clear();
		}

		public static string LowercaseFirst(string str, string prefix)
			=> string.IsNullOrEmpty(str) ? str : prefix + char.ToLower(str[0]) + str.Substring(1);
	}
}