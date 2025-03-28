﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Installers {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public partial class InstallerGenerator {
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
				var typeNamespace = entry.Type.Namespace;
				if (!_namespaces.Contains(typeNamespace))
					_namespaces.Add(typeNamespace);
			}

			_builder.Append("using Zenject;\n");
			foreach (var ns in _namespaces)
				_builder.Append($"using {ns};\n");
			_builder.Append("\n");
			_builder.Append(@"public static class ").Append(name).Append("Installer {\n");
			_builder.Append(@"	public static void Install(DiContainer container) {").Append("\n");
			var entryOrder = -1;
			foreach (var entry in entries) {
				if (entryOrder != entry.Attribute.Order) {
					entryOrder = entry.Attribute.Order;
					_builder.Append("\n").Append(@"		").Append("// ").Append(entryOrder).Append("\n");
				}

				_builder.Append(@"		container.")
					.Append(GetBindType(entry.Attribute.BindType))
					.Append("<")
					.Append(entry.Type.Name)
					.Append(">()")
					.Append(GetScopeType(entry.Attribute.ScopeType))
					.Append(GetInstallType(entry.Attribute.Type))
					.Append(";")
					.Append("\n");
			}

			_builder.Append("	}\n"); // Скобка закрывающая метод Install
			_builder.Append("}"); // Скобка закрывающая неймспейс

			_files[name] = _builder.ToString();
			_builder.Clear();
			_namespaces.Clear();
		}

		private static string GetScopeType(EScopeTypes scopeType) {
			switch (scopeType) {
				case EScopeTypes.Transient:
					return ".AsTransient()";
				case EScopeTypes.Singleton:
					return ".AsSingle()";
				default:
					return string.Empty;
			}
		}

		private static string GetInstallType(EInstallType type) {
			switch (type) {
				case EInstallType.NonLazy:
					return ".NonLazy()";
				default:
					return string.Empty;
			}
		}

		private static string GetBindType(EBindType type) {
			switch (type) {
				case EBindType.BindInterfacesAndSelfTo:
					return "BindInterfacesAndSelfTo";
				case EBindType.BindInterfacesTo:
					return "BindInterfacesTo";
				case EBindType.Bind:
					return "Bind";
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}