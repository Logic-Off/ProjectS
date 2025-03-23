using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common {
	[Serializable]
	public struct IconData {
		[HideLabel] public string Id;

		[PreviewField(ObjectFieldAlignment.Left, Height = 100), HideLabel]
		public Sprite Sprite;
	}
}