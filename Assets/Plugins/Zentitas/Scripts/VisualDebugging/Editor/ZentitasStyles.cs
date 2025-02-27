using UnityEngine;

namespace Zentitas.VisualDebugging.Editor {
	public static class ZentitasStyles {

		private static GUIStyle _header;
		private static GUIStyle _content;
		public static GUIStyle Header {
			get {
				if (_header == null)
					_header = new GUIStyle("OL Title");

				return _header;
			}
		}

		public static GUIStyle Content {
			get {
				if (_content == null) {
					_content = new GUIStyle("OL Box");
					_content.stretchHeight = false;
				}

				return _content;
			}
		}
	}
}