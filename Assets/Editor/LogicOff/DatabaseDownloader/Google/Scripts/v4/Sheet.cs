using System;
using System.Collections.Generic;

namespace LogicOff.DatabaseDownloader.Google {
	[Serializable]
	public class Sheet {
		public Properties properties;
		public List<Merge> merges;
	}
}