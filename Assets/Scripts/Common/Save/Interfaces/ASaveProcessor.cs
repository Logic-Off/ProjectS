using System;
using Zenject;

namespace Ecs.Save {
	/// <summary>
	/// Базовый класс сохранений
	/// </summary>
	public abstract class ASaveProcessor : IInitializable, IDisposable, ISaveListener {
		public abstract int Order { get; }
		[Inject] private ISaveFacade _saveFacade;
		public abstract void Save();

		public void Initialize() => _saveFacade.AddListener(this);

		public void Dispose() => _saveFacade.RemoveListener(this);
	}
}