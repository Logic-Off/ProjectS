using Utopia;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public class StatusInteractor {
		private readonly StatusPresenter _presenter;
		private readonly CharacterContext _character;

		public StatusInteractor(StatusPresenter presenter, CharacterContext character) {
			_presenter = presenter;
			_character = character;
		}

		public void Initialize() {
			if (!_character.IsPlayer)
				return;
			var player = _character.PlayerEntity;

			_presenter.HealthAmount.Fire(player.Health.Value / player.MaxHealth.Value);
			_presenter.HungerAmount.Fire(player.Hunger.Value / player.MaxHunger.Value);
			_presenter.ThirstAmount.Fire(player.Thirst.Value / player.MaxThirst.Value);
			_presenter.PsycheAmount.Fire(player.Psyche.Value / player.MaxPsyche.Value);
			_presenter.ColdAmount.Fire(player.Cold.Value / player.MaxCold.Value);
			_presenter.RadiationAmount.Fire(player.Radiation.Value / player.MaxRadiation.Value);
		}
	}
}