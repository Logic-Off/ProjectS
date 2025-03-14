using Cysharp.Threading.Tasks;

namespace Ui {
	public interface IPrebuildWindow {
		EWindowName Name { get; }
		UniTask Prebuild();
	}
}