using Ecs.Character;
using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class PlayerCheats : ACheat {
		public override string Name => "Player";
		public override int Order => 0;

		private readonly CharacterContext _character;

		public PlayerCheats(CharacterContext character) => _character = character;

		public override void Create() {
			var player = _character.PlayerEntity;

			AddButton("Получить 10 урона", () => player.AddModifier(new StatModifier(ECharacterStat.Health, EStatModifierType.Damage, -10)));
			AddButton("Умереть", () => player.AddModifier(new StatModifier(ECharacterStat.Health, EStatModifierType.Damage, float.MinValue)));
			AddButton(
				"Много здоровья",
				() => {
					player.AddModifier(new StatModifier(ECharacterStat.MaxHealth, EStatModifierType.None, 5_000_000));
					player.AddModifier(new StatModifier(ECharacterStat.Health, EStatModifierType.Heal, 5_000_000));
				}
			);
		}
	}
}