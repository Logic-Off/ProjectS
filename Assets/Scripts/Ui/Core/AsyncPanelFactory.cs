using Cysharp.Threading.Tasks;
using Ui.Interfaces;
using UnityEngine.AddressableAssets;

namespace Ui {
	public class AsyncPanelFactory {
		private IPanelView _view;

		private readonly AssetReference _reference;
		private readonly ICanvasParent _parent;

		public AsyncPanelFactory(AssetReference reference, ICanvasParent parent) {
			_reference = reference;
			_parent = parent;
		}

		public async UniTask<IPanelView> GetView() {
			if (_view != null)
				return _view;

			var gameObject = await _reference.InstantiateAsync(_parent.Canvas.transform).Task;
			gameObject.SetActive(false);
			_view = gameObject.GetComponent<IPanelView>();
			return _view;
		}
	}
}