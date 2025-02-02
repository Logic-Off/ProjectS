using Common;
using LogicOff.FavoriteObjects.Entries;

namespace LogicOff.FavoriteObjects.Presenters {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public sealed class FavoriteObjectsEditorPresenter {
		public IEventProperty<FavoriteObjectItemEntry[]> FavoriteItems =
			new EventProperty<FavoriteObjectItemEntry[]>(new FavoriteObjectItemEntry[0]);

		public ISignal Initialize = new Signal();
		public IEventProperty<int> OnSelection = new EventProperty<int>();
		public ISignal OnSetDatabase = new Signal();
	}
}