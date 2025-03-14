using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public sealed class ThirstChangeUiSystem : ReactiveSystem<CharacterEntity> {
		private readonly StatusPresenter _presenter;
		public ThirstChangeUiSystem(CharacterContext character, StatusPresenter presenter) : base(character) => _presenter = presenter;

		protected override ICollector<CharacterEntity> GetTrigger(IContext<CharacterEntity> context)
			=> context.CreateCollector(CharacterMatcher.Thirst.Added());

		protected override bool Filter(CharacterEntity entity)
			=> entity.IsPlayer && entity.HasThirst;

		protected override void Execute(List<CharacterEntity> entities) {
			foreach (var entity in entities)
				_presenter.ThirstAmount.Value = entity.Thirst.Value / entity.MaxThirst.Value;
		}
	}
}