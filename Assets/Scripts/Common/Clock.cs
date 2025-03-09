using System;
using Utopia;

namespace Common {
	[InstallerGenerator(InstallerId.Project)]
	public class Clock : IClock {
		public float Time => UnityEngine.Time.realtimeSinceStartup;
		public long Timestamp => DateTimeOffset.Now.ToUnixTimeSeconds();
	}
}