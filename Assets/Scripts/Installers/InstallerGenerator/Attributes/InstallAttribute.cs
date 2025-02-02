using System;
using Installers;

public class InstallAttribute : Attribute {
	public string Comment;
	public readonly EInstallType Type;
	public string Name;
	public int Order;
	public EScopeTypes ScopeType;
	public EBindType BindType;

	public InstallAttribute(
		string name,
		EScopeTypes scopeType = EScopeTypes.Singleton,
		int order = 100000,
		string comment = "",
		EInstallType type = EInstallType.None,
		EBindType bindType = EBindType.BindInterfacesAndSelfTo
	) {
		Name = name;
		ScopeType = scopeType;
		Order = order;
		Comment = comment;
		Type = type;
		BindType = bindType;
	}

	public InstallAttribute(
		string name,
		int order,
		string comment,
		EInstallType type = EInstallType.None,
		EBindType bindType = EBindType.BindInterfacesAndSelfTo
	) {
		Name = name;
		ScopeType = EScopeTypes.Singleton;
		Order = order;
		Comment = comment;
		Type = type;
		BindType = bindType;
	}

	public InstallAttribute(
		string name,
		EInstallType type = EInstallType.None,
		EBindType bindType = EBindType.BindInterfacesAndSelfTo
	) {
		Name = name;
		ScopeType = EScopeTypes.Singleton;
		Order = 100000;
		Comment = string.Empty;
		Type = type;
		BindType = bindType;
	}

	public InstallAttribute(
		string name,
		int order = 100_000,
		EInstallType type = EInstallType.None,
		EBindType bindType = EBindType.BindInterfacesAndSelfTo
	) {
		Name = name;
		ScopeType = EScopeTypes.Singleton;
		Order = order;
		Comment = string.Empty;
		Type = type;
		BindType = bindType;
	}

	public InstallAttribute(
		string name,
		EBindType bindType = EBindType.BindInterfacesAndSelfTo
	) {
		Name = name;
		ScopeType = EScopeTypes.Singleton;
		Order = 100000;
		Comment = string.Empty;
		Type = EInstallType.None;
		BindType = bindType;
	}

	public InstallAttribute(string name) {
		Name = name;
		ScopeType = EScopeTypes.Singleton;
		Order = 100000;
		Comment = string.Empty;
		Type = EInstallType.None;
		BindType = EBindType.BindInterfacesAndSelfTo;
	}
}