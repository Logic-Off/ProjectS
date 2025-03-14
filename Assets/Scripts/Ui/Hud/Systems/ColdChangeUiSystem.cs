using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public sealed class ColdChangeUiSystem : ReactiveSystem<CharacterEntity> {
		private readonly StatusPresenter _presenter;
		public ColdChangeUiSystem(CharacterContext character, StatusPresenter presenter) : base(character) => _presenter = presenter;

		protected override ICollector<CharacterEntity> GetTrigger(IContext<CharacterEntity> context)
			=> context.CreateCollector(CharacterMatcher.Cold.Added());

		protected override bool Filter(CharacterEntity entity)
			=> entity.IsPlayer && entity.HasCold;

		protected override void Execute(List<CharacterEntity> entities) {
			foreach (var entity in entities)
				_presenter.ColdAmount.Value = entity.Cold.Value / entity.MaxCold.Value;
		}
	}
}