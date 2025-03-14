using Ui;
using UnityEngine.AddressableAssets;

namespace Zenject {
	public static class UiZenjectExtensions {
		public static void BindUiViewReference<TBuilder>(this DiContainer container, AssetReference view)
			where TBuilder : IPanelBuilder {
			container.BindInterfacesAndSelfTo<TBuilder>().AsSingle();
			container.BindInterfacesAndSelfTo<AsyncPanelFactory>().WithArguments(view).WhenInjectedInto<TBuilder>();
		}
	}
}