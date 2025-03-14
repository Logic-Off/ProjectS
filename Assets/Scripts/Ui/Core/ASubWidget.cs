using UnityEngine;

namespace Ui {
	public abstract class ASubWidget : MonoBehaviour {
		public abstract void Build(UiContext context, UiEntity parent, UiEntity element);
	}
}