using System;

public class InstallUiPrefabAttribute : Attribute {
	public string Comment;
	public string Name;
	public int Order;
	public EScopeTypes ScopeType;

	public InstallUiPrefabAttribute(
		string name,
		EScopeTypes scopeType = EScopeTypes.Singleton,
		int order = 100000,
		string comment = ""
	) {
		Name = name;
		ScopeType = scopeType;
		Order = order;
		Comment = comment;
	}

	public InstallUiPrefabAttribute(
		string name,
		int order = 100000,
		string comment = ""
	) {
		Name = name;
		ScopeType = EScopeTypes.Singleton;
		Order = order;
		Comment = comment;
	}

	public InstallUiPrefabAttribute(string name) {
		Name = name;
		ScopeType = EScopeTypes.Singleton;
		Order = 100000;
		Comment = string.Empty;
	}
}