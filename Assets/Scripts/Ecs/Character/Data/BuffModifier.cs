using System;

namespace Ecs.Character {
	[Serializable]
	public class BuffModifier {
		public ABuff Buff;
		public EBuffModifier Modifier;

		public BuffModifier(ABuff buff, EBuffModifier modifier) {
			Buff = buff;
			Modifier = modifier;
		}

		public override string ToString() => $"Buff[{Buff.Effects.Count}]. {Modifier}";
	}
}