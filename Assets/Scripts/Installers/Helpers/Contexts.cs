using System;
using System.Collections.Generic;
using System.Linq;
using JCMG.EntitasRedux;
using JCMG.EntitasRedux.VisualDebugging;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public partial class Contexts : IDisposable {
	private readonly string _name;

	[Inject]
	public Contexts(
		string name,
		GameContext game,
		InputContext input,
		InventoryContext inventory,
		StructureContext structure,
		SharedContext shared
	) {
		_name = name;
		Game = game;
		Input = input;
		Inventory = inventory;
		Structure = structure;
		Shared = shared;
		
		var postConstructors = GetType().GetMethods().Where(method => Attribute.IsDefined(method, typeof(PostConstructorAttribute)));

		foreach (var postConstructor in postConstructors)
			postConstructor.Invoke(this, null);

		var afterPostConstructors = GetType().GetMethods().Where(method => Attribute.IsDefined(method, typeof(AfterPostConstructorAttribute)));

		foreach (var postConstructor in afterPostConstructors)
			postConstructor.Invoke(this, null);
	}

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

	private static float _colorHue = 0.1357f * 5f;

	private readonly List<GameObject> _garbage = new();
	private Transform _header;

	[PostConstructor]
	public void AddHeader() {
		_colorHue = (_colorHue + 0.1357f) % 1f;

		var header = new GameObject();
		Object.DontDestroyOnLoad(header);
		header.name = $"--- {_name} ---";
		_header = header.transform;
		_garbage.Add(header);
	}

	[AfterPostConstructor]
	public void AddMarks() {
		foreach (var context in AllContexts) {
			var observer = context.FindContextObserver();
			if (observer == null)
				continue;
			_garbage.Add(observer.gameObject);
			observer.transform.SetParent(_header);
			observer.transform.SetAsFirstSibling();
		}
	}

	public void Dispose() {
		foreach (var gameObject in _garbage)
			Object.Destroy(gameObject);
		_garbage.Clear();
		_header = null;
	}

#else
	public void Dispose() {}
#endif
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class AfterPostConstructorAttribute : Attribute { }