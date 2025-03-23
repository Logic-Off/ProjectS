using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

public sealed class IdDropdownDrawer : AdvancedDropdown {
	private string _name;
	public string[] Items;
	public SerializedProperty Property;

	public IdDropdownDrawer(AdvancedDropdownState state, string name) : base(state) => _name = name;

	protected override AdvancedDropdownItem BuildRoot() {
		var root = new AdvancedDropdownItem(_name);

		foreach (var item in Items)
			root.AddChild(new AdvancedDropdownItem(item));

		return root;
	}

	protected override void ItemSelected(AdvancedDropdownItem item) {
		if (item == null)
			return;
		Property.stringValue = item.name;
		Property.serializedObject.ApplyModifiedProperties();
	}
}