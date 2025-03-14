using System;
using UnityEngine;
using UnityEngine.Events;

namespace Common {
	[System.Serializable]
	public class Sequence {
		public string AtTime;
		public float StartTime;

		public bool TriggerStart = false; //This automatically change to false immedietely after becoming true
		public bool TriggerEnd = false; //This automatically change to false immedietely after becoming true

		public float PropertyRectHeight;
		public float PropertyRectY;

		public SequenceType SequenceType = SequenceType.Animation;
		public EaseType EaseType = EaseType.Out;
		public EasePower EasePower = EasePower.Quart;

		public SequenceObjectType TargetType = SequenceObjectType.Automatic;

		public Component TargetComp;

		public float Duration = 0.5f;
		public UnityEvent<float> EventDynamic;
		public bool IsUnfolded = true;
		public bool IsDone = false;

		[Serializable]
		public class VectorEntry {
			public Vector3 Start;
			public Vector3 End;
		}

		[Serializable]
		public class FloatEntry {
			public float Start;
			public float End;
		}

		[Serializable]
		public class ColorEntry {
			public Color Start;
			public Color End;
		}

		#region SetActiveALlInput

		// public bool IsActivating = true;

		#endregion SetActiveALlInput

		#region SetActive

		public GameObject Target;
		public bool IsActivating = true;

		#endregion SetActive

		#region SFX

		public SequenceSFXMethod PlaySFXBy = SequenceSFXMethod.File;
		public AudioClip SFXFile;
		public int SFXIndex;

		#endregion SFX

		#region LoadScene

		public string SceneToLoad = "";

		#endregion LoadScene

		#region UnityEvent

		public UnityEvent Event;

		#endregion UnityEvent

		#region RectTransform

		public SequenceRectTransformTask TargetRtTask = SequenceRectTransformTask.AnchoredPosition;

		public SequenceState AnchoredPositionState = SequenceState.Before;
		public Vector3 AnchoredPositionStart;
		public Vector3 AnchoredPositionEnd;

		public SequenceState LocalScaleState = SequenceState.Before;
		public Vector3 LocalScaleStart;
		public Vector3 LocalScaleEnd;

		public SequenceState LocalEulerAnglesState = SequenceState.Before;
		public Vector3 LocalEulerAnglesStart;
		public Vector3 LocalEulerAnglesEnd;

		public SequenceState SizeDeltaState = SequenceState.Before;
		public Vector3 SizeDeltaStart;
		public Vector3 SizeDeltaEnd;

		public SequenceState AnchorMinState = SequenceState.Before;
		public Vector3 AnchorMinStart;
		public Vector3 AnchorMinEnd;

		public SequenceState AnchorMaxState = SequenceState.Before;
		public Vector3 AnchorMaxStart;
		public Vector3 AnchorMaxEnd;

		public SequenceState PivotState = SequenceState.Before;
		public Vector3 PivotStart;
		public Vector3 PivotEnd;

		#endregion RectTransform

		#region Transform

		public SequenceState TransState = SequenceState.Before;

		public SequenceTransformTask TargetTransTask = SequenceTransformTask.LocalPosition;

		public SequenceState LocalPositionState = SequenceState.Before;
		public Vector3 LocalPositionStart;
		public Vector3 LocalPositionEnd;

		// public Vector3 LocalScaleStart;
		// public Vector3 LocalScaleEnd;
		//
		// public Vector3 LocalEulerAnglesStart;
		// public Vector3 LocalEulerAnglesEnd;

		#endregion Transform

		#region Image

		public SequenceState ImgState = SequenceState.Before;

		public SequenceImageTask TargetImgTask = SequenceImageTask.Color;

		public SequenceState ColorState = SequenceState.Before;
		public Color ColorStart;
		public Color ColorEnd;

		public SequenceState FillAmountState = SequenceState.Before;
		[Range(0, 1)] public float FillAmountStart;
		[Range(0, 1)] public float FillAmountEnd;

		#endregion Image

		#region CanvasGroup

		[System.Flags]
		public enum CgTask {
			None = 0,
			Alpha = 1 << 0,
		}

		public CgTask TargetCgTask = CgTask.Alpha;

		public SequenceState AlphaState = SequenceState.Before;
		[Range(0, 1)] public float AlphaStart;
		[Range(0, 1)] public float AlphaEnd;

		#endregion CanvasGroup

		#region Camera

		public SequenceCameraTask TargetCamTask = SequenceCameraTask.BackgroundColor;

		public SequenceState BackgroundColorState = SequenceState.Before;
		public Color BackgroundColorStart;
		public Color BackgroundColorEnd;

		public SequenceState OrthographicSizeState = SequenceState.Before;
		public float OrthographicSizeStart;
		public float OrthographicSizeEnd;

		#endregion Camera

		#region TextMeshPro

		public SequenceTextMeshProTask TargetTextMeshProTask = SequenceTextMeshProTask.Color;

		public SequenceState TextMeshProColorState = SequenceState.Before;
		public Color TextMeshProColorStart;
		public Color TextMeshProColorEnd;

		public SequenceState MaxVisibleCharactersState = SequenceState.Before;
		public int MaxVisibleCharactersStart;
		public int MaxVisibleCharactersEnd;

		#endregion Camera

		public Ease.Function EaseFunction = Ease.OutQuart;

		public void Init() => EaseFunction = Ease.GetEase(EaseType, EasePower);
	}

	[System.Flags]
	public enum SequenceTextMeshProTask {
		None = 0,
		Color = 1 << 0,
		MaxVisibleCharacters = 2 << 0,
	}

	[System.Flags]
	public enum SequenceCameraTask {
		None = 0,
		BackgroundColor = 1 << 0,
		OrthographicSize = 2 << 0,
	}

	[System.Flags]
	public enum SequenceImageTask {
		None = 0,
		Color = 1 << 0,
		FillAmount = 1 << 1,
	}

	[System.Flags]
	public enum SequenceTransformTask {
		None = 0,
		LocalPosition = 1 << 0,
		LocalScale = 1 << 1,
		LocalEulerAngles = 1 << 2,
	}

	public enum SequenceObjectType {
		// Only for Animation
		Automatic,
		RectTransform,
		Transform,
		Image,
		CanvasGroup,
		Camera,
		TextMeshPro,
		UnityEventDynamic
	}

	public enum SequenceSFXMethod {
		File,
		Index
	}

	[System.Flags]
	public enum SequenceRectTransformTask {
		None = 0,
		AnchoredPosition = 1 << 0,
		LocalScale = 1 << 1,
		LocalEulerAngles = 1 << 2,
		SizeDelta = 1 << 3,
		AnchorMin = 1 << 4,
		AnchorMax = 1 << 5,
		Pivot = 1 << 6
	}

	public enum SequenceState {
		// For snapping
		Before,
		During,
		After
	}

	public enum SequenceType {
		Animation,
		Wait,
		SetActiveAllInput,
		SetActive,
		SFX,
		LoadScene,
		UnityEvent
	}

	public enum ESequenceTaskType {
		LocalPosition,
		LocalEulerAngles,
		LocalScale,
		AnchoredPosition,
		SizeDelta,
		AnchorMin,
		AnchorMax,
		Pivot,
		File,
		Index,
		Color,
		FillAmount,
		BackgroundColor,
		OrthographicSize,
		MaxVisibleCharacters,
		Alpha
	}
}