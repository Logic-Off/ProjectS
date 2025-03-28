//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts : JCMG.EntitasRedux.IContexts
{
	#if UNITY_EDITOR && !ENTITAS_REDUX_NO_SHARED_CONTEXT

	static Contexts()
	{
		UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
	}

	/// <summary>
	/// Invoked when the Unity Editor has a <see cref="UnityEditor.PlayModeStateChange"/> change.
	/// </summary>
	private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange playModeStateChange)
	{
		// When entering edit-mode, reset all static state so that it does not interfere with the
		// next play-mode session.
		if (playModeStateChange == UnityEditor.PlayModeStateChange.EnteredEditMode)
		{
			_sharedInstance = null;
		}
	}

	#endif

	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	/// <summary>
	/// A globally-accessible singleton instance of <see cref="Contexts"/>. Instantiated
	/// the first time its <see langword="get"/> property is used.
	/// </summary>
	/// <remarks>
	/// If your project forbids global singletons like this one, add a <c>#define</c> named <c>ENTITAS_REDUX_NO_SHARED_CONTEXT</c>
	/// to its build settings. Doing so will remove this property to prevent accidental use.
	/// </remarks>
	public static Contexts SharedInstance
	{
		get
		{
			if (_sharedInstance == null)
			{
				_sharedInstance = new Contexts();
			}

			return _sharedInstance;
		}
		set	{ _sharedInstance = value; }
	}

	static Contexts _sharedInstance;
	#endif

	public GameContext Game { get; set; }
	public InputContext Input { get; set; }
	public InventoryContext Inventory { get; set; }
	public SharedContext Shared { get; set; }
	public StructureContext Structure { get; set; }

	public JCMG.EntitasRedux.IContext[] AllContexts { get { return new JCMG.EntitasRedux.IContext [] { Game, Input, Inventory, Shared, Structure }; } }

	public Contexts()
	{
		Game = new GameContext();
		Input = new InputContext();
		Inventory = new InventoryContext();
		Shared = new SharedContext();
		Structure = new StructureContext();

		var postConstructors = System.Linq.Enumerable.Where(
			GetType().GetMethods(),
			method => System.Attribute.IsDefined(method, typeof(JCMG.EntitasRedux.PostConstructorAttribute))
		);

		foreach (var postConstructor in postConstructors)
		{
			postConstructor.Invoke(this, null);
		}
	}

	public void Reset()
	{
		var contexts = AllContexts;
		for (int i = 0; i < contexts.Length; i++)
		{
			contexts[i].Reset();
		}
	}
}

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts {

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

	[JCMG.EntitasRedux.PostConstructor]
	public void InitializeContextObservers() {
		try {
			CreateContextObserver(Game);
			CreateContextObserver(Input);
			CreateContextObserver(Inventory);
			CreateContextObserver(Shared);
			CreateContextObserver(Structure);
		} catch(System.Exception) {
		}
	}

	public void CreateContextObserver(JCMG.EntitasRedux.IContext context) {
		if (UnityEngine.Application.isPlaying) {
			var observer = new JCMG.EntitasRedux.VisualDebugging.ContextObserver(context);
			UnityEngine.Object.DontDestroyOnLoad(observer.GameObject);
		}
	}

#endif
}
