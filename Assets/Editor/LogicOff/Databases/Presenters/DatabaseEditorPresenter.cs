using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace LogicOff.Databases {
	/// <summary>
	/// </summary>
	public sealed class DatabaseEditorPresenter {
		public IEventProperty<Dictionary<string, Dictionary<string, Object>>> MainObjects = new EventProperty<Dictionary<string, Dictionary<string, Object>>>(new Dictionary<string, Dictionary<string, Object>>());

		public IEventProperty<List<string>> PrimeListElements = new EventProperty<List<string>>(new List<string>());
		public IEventProperty<int> SelectedPrimeIndex = new EventProperty<int>(-1);
		public IEventProperty<int> SelectedSubIndex = new EventProperty<int>(-1);
		public IEventProperty<object> SelectedPrimeObject = new EventProperty<object>();
		public IEventProperty<object> SelectedSubObject = new EventProperty<object>();

		public IEventProperty<Object> CurrentDatabase = new EventProperty<Object>();
		public IEventProperty<List<Object>> Databases = new EventProperty<List<Object>>(new List<Object>());
		public IEventProperty<ICustomEditor> CurrentEditor = new EventProperty<ICustomEditor>();

		public IEventProperty<VisualElement> InfoContainer = new EventProperty<VisualElement>();

		public ISignal OnRedraw = new Signal();
	}
}