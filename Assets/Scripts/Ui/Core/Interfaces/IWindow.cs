using Cysharp.Threading.Tasks;

namespace Ui {
	public interface IWindow {
		EWindowName Name { get; }
		bool InProcessLoaded { get; }
		void OnOpen();
		UniTaskVoid OnShow();
		void OnHide();
		void Initialize();
		void Dispose();
	}
}