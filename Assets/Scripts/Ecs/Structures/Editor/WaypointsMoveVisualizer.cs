#if UNITY_EDITOR

using System.Collections.Generic;
using Ecs.Game;
using UnityEditor;
using UnityEngine;

namespace Ecs.Structures {
	[CustomEditor(typeof(WaypointStructureSubBehaviour), true)]
	public class WaypointsMoveVisualizer : Editor {
		private WaypointStructureSubBehaviour _target;
		private List<WaypointEntry> _waypoints;
		private static Color _removeSphereColor = new(0.3679245f, 0f, 0f, 1);
		private static bool _isRemoveMode;

		protected void OnEnable() {
			if (target is not WaypointStructureSubBehaviour)
				return;
			_target = (WaypointStructureSubBehaviour) target;
			_waypoints = _target.Waypoints;
		}

		protected void OnDisable() {
			_target = null;
			_waypoints = null;
		}

		private void OnSceneGUI() {
			if (_target == null || _waypoints.Count == 0)
				return;

			var transform = _target.transform;
			// var matrix = transform.localToWorldMatrix;
			// BehaviourGizmos.RadiusAtRoot(serializedObject, matrix);
			Handles.color = Color.cyan;
			GUI.color = Color.cyan;
			var isRemoveHotKeyPressed = Event.current.control;
			for (var index = 0; index < _waypoints.Count; index++) {
				OnDrawLine(index);
				OnCreateNewPoint(index);
				OnDrawDiskAndLabel(index);
				if (isRemoveHotKeyPressed || _isRemoveMode)
					OnRemovePoint(index);
				else
					OnChangePosition(index);
			}

			Handles.color = Color.white;
			serializedObject.ApplyModifiedProperties();
		}

		private void OnDrawDiskAndLabel(int index) {
			var waypoint = _waypoints[index];
			Handles.color = Color.yellow;
			Handles.DrawWireDisc(waypoint.Position, Vector3.up, waypoint.Radius);
			Handles.Label(
				waypoint.Position,
				$"Element {index.ToString()}\n{waypoint.Position}",
				new GUIStyle("Label") {alignment = TextAnchor.MiddleCenter}
			);
			Handles.color = Color.cyan;
		}

		private void OnDrawLine(int index) {
			var nextIndex = index + 1 >= _waypoints.Count ? 0 : index + 1;
			Handles.DrawLine(_waypoints[index].Position, _waypoints[nextIndex].Position);
		}

		private void OnChangePosition(int index) {
			EditorGUI.BeginChangeCheck();
			var waypoint = _waypoints[index];
			var newPosition = Handles.PositionHandle(waypoint.Position, Quaternion.identity);

			var isChanged = EditorGUI.EndChangeCheck();
			if (!isChanged)
				return;
			waypoint.Position = newPosition;
			_waypoints[index] = waypoint;

			EditorUtility.SetDirty(target);
		}

		private void OnRemovePoint(int index) {
			var waypoint = _waypoints[index];

			Handles.color = _removeSphereColor;

			var isChanged = false;
			if (Handles.Button(waypoint.Position, Quaternion.identity, .8f, .5f, Handles.SphereHandleCap)) {
				_waypoints.RemoveAt(index);
				isChanged = true;
			}

			Handles.color = Color.cyan;
			if (!isChanged)
				return;
			EditorUtility.SetDirty(target);
		}

		private void OnCreateNewPoint(int index) {
			var nextIndex = index + 1 >= _waypoints.Count ? 0 : index + 1;
			var center = Vector3.Lerp(_waypoints[index].Position, _waypoints[nextIndex].Position, 0.5f);
			Handles.color = Color.green;

			var isChanged = false;
			if (Handles.Button(center, Quaternion.identity, .3f, .3f, Handles.SphereHandleCap)) {
				_waypoints.Insert(nextIndex, new WaypointEntry() {Position = center});
				isChanged = true;
			}

			Handles.color = Color.cyan;
			if (!isChanged)
				return;
			EditorUtility.SetDirty(target);
		}
	}
}
#endif