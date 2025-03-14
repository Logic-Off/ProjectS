using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public sealed class PsycheChangeUiSystem : ReactiveSystem<CharacterEntity> {
		private readonly StatusPresenter _presenter;
		public PsycheChangeUiSystem(CharacterContext character, StatusPresenter presenter) : base(character) => _presenter = presenter;

		protected override ICollector<CharacterEntity> GetTrigger(IContext<CharacterEntity> context)
			=> context.CreateCollector(CharacterMatcher.Psyche.Added());

		protected override bool Filter(CharacterEntity entity)
			=> entity.IsPlayer && entity.HasPsyche;

		protected override void Execute(List<CharacterEntity> entities) {
			foreach (var entity in entities)
				_presenter.PsycheAmount.Value = entity.Psyche.Value / entity.MaxPsyche.Value;
		}
	}
}