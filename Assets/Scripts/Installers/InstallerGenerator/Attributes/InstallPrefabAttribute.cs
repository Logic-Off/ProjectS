using System;

public class InstallPrefabAttribute : Attribute {
	public EAdditionalPrefabParameters AdditionalParameters;
	public string Comment;
	public string Name;
	public int Order;

	public InstallPrefabAttribute(
		string name,
		EAdditionalPrefabParameters additionalParameters = EAdditionalPrefabParameters.None,
		int order = 100000,
		string comment = ""
	) {
		Name = name;
		AdditionalParameters = additionalParameters;
		Order = order;
		Comment = comment;
	}

	public InstallPrefabAttribute(
		string name,
		int order = 100000,
		string comment = ""
	) {
		Name = name;
		AdditionalParameters = EAdditionalPrefabParameters.None;
		Order = order;
		Comment = comment;
	}

	public InstallPrefabAttribute(string name) {
		Name = name;
		AdditionalParameters = EAdditionalPrefabParameters.None;
		Order = 100000;
		Comment = string.Empty;
	}
}

public enum EAdditionalPrefabParameters {
	None,
	FromComponentInNewPrefab,
	FromComponentInNewPrefabAsTransient
}