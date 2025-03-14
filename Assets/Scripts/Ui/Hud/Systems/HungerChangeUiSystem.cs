using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public sealed class HungerChangeUiSystem : ReactiveSystem<CharacterEntity> {
		private readonly StatusPresenter _presenter;
		public HungerChangeUiSystem(CharacterContext character, StatusPresenter presenter) : base(character) => _presenter = presenter;

		protected override ICollector<CharacterEntity> GetTrigger(IContext<CharacterEntity> context)
			=> context.CreateCollector(CharacterMatcher.Hunger.Added());

		protected override bool Filter(CharacterEntity entity)
			=> entity.IsPlayer && entity.HasHunger;

		protected override void Execute(List<CharacterEntity> entities) {
			foreach (var entity in entities)
				_presenter.HungerAmount.Value = entity.Hunger.Value / entity.MaxHunger.Value;
		}
	}
}