using System;
using System.Collections.Generic;

namespace Ecs.Character {
	[Serializable]
	public abstract class ABuff {
		public BuffId Id;
		public abstract List<Effect> Effects { get; }
		public abstract List<TriggerEffect> Triggers { get; }
		public abstract EBuffType Type { get; }
		public abstract double EndTime { get; }
		public bool IsDisable { get; set; }
	}
}