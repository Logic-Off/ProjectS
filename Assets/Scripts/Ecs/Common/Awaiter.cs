using System.Threading;
using UnityEngine;

namespace Ecs.Common {
	public static class Awaiter {
		public static async Awaitable NextFrameAsync(float frames, CancellationToken cancellationToken = default(CancellationToken)) {
			for (int i = 0; i < frames; i++)
				await Awaitable.NextFrameAsync();
		}
	}
}