using UnityEngine;

namespace Ui {
	public interface ICanvasParent {
		Canvas Canvas { get; }
		Canvas DisabledCanvas { get; }
	}
}