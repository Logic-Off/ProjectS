using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public sealed class RadiationChangeUiSystem : ReactiveSystem<CharacterEntity> {
		private readonly StatusPresenter _presenter;
		public RadiationChangeUiSystem(CharacterContext character, StatusPresenter presenter) : base(character) => _presenter = presenter;

		protected override ICollector<CharacterEntity> GetTrigger(IContext<CharacterEntity> context)
			=> context.CreateCollector(CharacterMatcher.Radiation.Added());

		protected override bool Filter(CharacterEntity entity)
			=> entity.IsPlayer && entity.HasRadiation;

		protected override void Execute(List<CharacterEntity> entities) {
			foreach (var entity in entities)
				_presenter.RadiationAmount.Value = entity.Radiation.Value / entity.MaxRadiation.Value;
		}
	}
}