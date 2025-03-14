using System.Collections.Generic;
using Ecs.Character;

namespace Ecs.Item {
	public interface IEquipment {
		List<EffectEntry> Effects { get; }
	}
}