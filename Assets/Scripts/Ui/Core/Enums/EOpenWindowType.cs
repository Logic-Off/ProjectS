namespace Ui {
	public enum EOpenWindowType {
		Default,

		/// <summary>
		/// Удаляет текущее окно
		/// </summary>
		RemoveCurrent,

		/// <summary>
		/// Чистит полностью стек
		/// </summary>
		ClearStack,
		
		/// <summary>
		/// Очередь, открывается на HUD
		/// </summary>
		Queue
	}
}