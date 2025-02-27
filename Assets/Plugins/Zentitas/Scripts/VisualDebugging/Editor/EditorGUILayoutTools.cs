/*
MIT License

Copyright (c) Andrey Abramkin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using UnityEditor;
using UnityEngine;
using Zentitas.Editor;

namespace Zentitas.VisualDebugging.Editor {
	/// <summary>
	/// Helper methods for <see cref="EditorGUILayout"/>.
	/// </summary>
	public static class EditorGUILayoutTools {
		public static bool MiniButton(string c) {
			return MiniButton(c, EditorStyles.miniButton);
		}

		private static bool MiniButton(string c, GUIStyle style) {
			GUILayoutOption[] guiLayoutOptionArray1;
			if (c.Length != 1) {
				guiLayoutOptionArray1 = new GUILayoutOption[0];
			} else {
				guiLayoutOptionArray1 = new GUILayoutOption[1] {
					GUILayout.Width(19f)
				};
			}

			var guiLayoutOptionArray2 = guiLayoutOptionArray1;
			var num = GUILayout.Button(c, style, guiLayoutOptionArray2) ? 1 : 0;
			if (num == 0) {
				return num != 0;
			}

			GUI.FocusControl(null);
			return num != 0;
		}

		public static bool DrawSectionHeaderToggle(string header, bool value) => GUILayout.Toggle(value, header, ZentitasStyles.Header);

		public static void BeginSectionContent() => EditorGUILayout.BeginVertical(ZentitasStyles.Content);

		public static void EndSectionContent() => EditorGUILayout.EndVertical();
		
	}
}