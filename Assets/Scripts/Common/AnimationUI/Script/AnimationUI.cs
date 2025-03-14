using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common {
#if UNITY_EDITOR
	[ExecuteInEditMode]
#endif
	public class AnimationUI : MonoBehaviour {
		[HideInInspector] public float TotalDuration = 0; //Value automatically taken care of by AnimationUIInspector
		public Sequence[] AnimationSequence;
		[HideInInspector] public bool PlayOnStart = false;
		[HideInInspector] public bool Looped = false;
		private bool _isSequencesInitialized = false;

		private List<Coroutine> _coroutines = new();

		public Action<float> UpdateSequence;

		public Action OnAnimationEnded;
		private List<Action> atTimeEvents = new();
		private List<float> atTimes = new();

		public static event Action<bool> OnSetActiveAllInput;
		public static event Action<AudioClip> OnPlaySoundByFile;
		public static event Action<int> OnPlaySoundByIndex;

		[HideInInspector] public float CurrentTime = 0; // Don't forget this variable might be in build
		[HideInInspector] public bool IsPlayingInEditMode = false;
		private float _startTime = 0;

		void Awake() {
#if UNITY_EDITOR
			if (Application.isPlaying)
#endif
				InitializeSequences();
		}

		void Start() {
#if UNITY_EDITOR
			if (Application.isPlaying)
#endif
				if (PlayOnStart)
					Play();
		}

		/// <summary>
		/// Initialize all sequences
		/// </summary>
		void InitializeSequences() {
			foreach (var sequence in AnimationSequence)
				sequence.Init();
		}

		/// <summary>
		/// Play the animation
		/// </summary>
		[ContextMenu("Play Animation")]
		public void Play() {
			if (!_isSequencesInitialized) {
				InitializeSequences();
				_isSequencesInitialized = true;
			}

			Stop();
			if (!gameObject.activeInHierarchy)
				return;
			if (Looped)
				_coroutines.Add(StartCoroutine(LoopedAnimation()));
			else
				_coroutines.Add(StartCoroutine(PlayAnimation()));
		}

		/// <summary>
		/// Play the animation in reverse
		/// </summary>
		public void PlayReversed() {
			if (!_isSequencesInitialized) {
				InitializeSequences();
				_isSequencesInitialized = true;
			}

			Stop();
			if (!gameObject.activeInHierarchy)
				return;
			if (Looped)
				_coroutines.Add(StartCoroutine(LoopedReversedAnimation()));
			else
				_coroutines.Add(StartCoroutine(PlayAnimation(true)));
		}

		private IEnumerator LoopedAnimation() {
			while (true)
				yield return PlayAnimation();
		}

		private IEnumerator LoopedReversedAnimation() {
			while (true)
				yield return PlayAnimation(true);
		}

		[ContextMenu("Stop Animation")]
		public void Stop() {
			foreach (var coroutine in _coroutines) {
				if (coroutine == null)
					continue;
				StopCoroutine(coroutine);
			}

			_coroutines.Clear();
		}

		private IEnumerator PlayAnimation(bool isReversed = false) {
			if (isReversed) {
				Array.Reverse(AnimationSequence);
				ReverseArrayAtTime(TotalDuration);
			}

			for (var i = 0; i < atTimeEvents.Count; i++)
				StartCoroutine(AtTimeEvent(atTimeEvents[i], atTimes[i])); //Function to call at time

			foreach (Sequence sequence in AnimationSequence) {
				if (sequence.SequenceType == SequenceType.Animation) {
					if (sequence.TargetComp == null) {
						Debug.Log("Please assign Target for Sequence at " + sequence.StartTime.ToString() + "s");
						continue;
					}

					switch (sequence.TargetType) {
						case SequenceObjectType.RectTransform: {
							if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchoredPosition))
								OnAnchoredPosition(sequence, sequence.AnchoredPositionStart, sequence.AnchoredPositionEnd, isReversed);
							if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.LocalEulerAngles))
								OnLocalEulerAngles(sequence, sequence.LocalEulerAnglesStart, sequence.LocalEulerAnglesEnd, isReversed);
							if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.LocalScale))
								OnLocalScale(sequence, sequence.LocalScaleStart, sequence.LocalScaleEnd, isReversed);
							if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchorMax))
								OnAnchorMax(sequence, sequence.AnchorMaxStart, sequence.AnchorMaxEnd, isReversed);
							if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchorMin))
								OnAnchorMin(sequence, sequence.AnchorMinStart, sequence.AnchorMinEnd, isReversed);
							if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.SizeDelta))
								OnSizeDelta(sequence, sequence.SizeDeltaStart, sequence.SizeDeltaEnd, isReversed);
							if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.Pivot))
								OnPivot(sequence, sequence.PivotStart, sequence.PivotEnd, isReversed);
							break;
						}
						case SequenceObjectType.Transform: {
							if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalPosition))
								OnLocalPosition(sequence, sequence.LocalPositionStart, sequence.LocalPositionEnd, isReversed);
							if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalEulerAngles))
								OnLocalEulerAngles(sequence, sequence.LocalEulerAnglesStart, sequence.LocalEulerAnglesEnd, isReversed);
							if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalScale))
								OnLocalScale(sequence, sequence.LocalScaleStart, sequence.LocalScaleEnd, isReversed);
							break;
						}
						case SequenceObjectType.Image: {
							if (sequence.TargetImgTask.HasFlag(SequenceImageTask.Color))
								OnColor(sequence, sequence.ColorStart, sequence.ColorEnd, isReversed);
							if (sequence.TargetImgTask.HasFlag(SequenceImageTask.FillAmount))
								OnFillAmount(sequence, sequence.FillAmountStart, sequence.FillAmountEnd, isReversed);
							break;
						}
						case SequenceObjectType.CanvasGroup: {
							if (sequence.TargetCgTask.HasFlag(Sequence.CgTask.Alpha))
								OnAlpha(sequence, sequence.AlphaStart, sequence.AlphaEnd, isReversed);
							break;
						}
						case SequenceObjectType.Camera: {
							if (sequence.TargetCamTask.HasFlag(SequenceCameraTask.BackgroundColor))
								OnCameraBackgroundColor(sequence, sequence.BackgroundColorStart, sequence.BackgroundColorEnd, isReversed);
							if (sequence.TargetCamTask.HasFlag(SequenceCameraTask.OrthographicSize))
								OnCameraOrthographicSize(sequence, sequence.OrthographicSizeStart, sequence.OrthographicSizeEnd, isReversed);
							break;
						}
						case SequenceObjectType.TextMeshPro: {
							if (sequence.TargetTextMeshProTask.HasFlag(SequenceTextMeshProTask.Color))
								OnColor(sequence, sequence.TextMeshProColorStart, sequence.TextMeshProColorEnd, isReversed);
							if (sequence.TargetTextMeshProTask.HasFlag(SequenceTextMeshProTask.MaxVisibleCharacters))
								OnTextMaxVisibleCharacter(sequence, sequence.MaxVisibleCharactersStart, sequence.MaxVisibleCharactersEnd, isReversed);
							break;
						}
					}
				} else if (sequence.SequenceType == SequenceType.Wait) {
					yield return new WaitForSecondsRealtime(sequence.Duration);
				} else if (sequence.SequenceType == SequenceType.SetActiveAllInput) {
					SetActiveAllInput(sequence.IsActivating != isReversed);
				} else if (sequence.SequenceType == SequenceType.SetActive) {
					if (sequence.Target == null)
						continue;

					sequence.Target.SetActive(sequence.IsActivating != isReversed);
				} else if (sequence.SequenceType == SequenceType.SFX) {
					if (sequence.PlaySFXBy == SequenceSFXMethod.File) {
						if (sequence.SFXFile == null)
							continue;

						PlaySound(sequence.SFXFile);
					} else
						PlaySound(sequence.SFXIndex);
				} else if (sequence.SequenceType == SequenceType.LoadScene) {
					UnityEngine.SceneManagement.SceneManager.LoadScene(sequence.SceneToLoad);
				} else if (sequence.SequenceType == SequenceType.UnityEvent) {
					sequence.Event?.Invoke();
				}
			}

			OnAnimationEnded?.Invoke(); //Function to call at end

			if (isReversed) {
				Array.Reverse(AnimationSequence);
			}

			OnAnimationEnded = null;
			atTimeEvents.Clear();
			atTimes.Clear();
		}

		IEnumerator ReverseArrayAtTime(float t) {
			yield return new WaitForSecondsRealtime(t);
			Array.Reverse(AnimationSequence);
		}

		private void OnTextMaxVisibleCharacter(Sequence sequence, float start, float end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<TMP_Text>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskMaxVisibleCharacters(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnCameraOrthographicSize(Sequence sequence, float start, float end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<Camera>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskOrthographicSize(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnCameraBackgroundColor(Sequence sequence, Color start, Color end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<Camera>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskBackgroundColor(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnAlpha(Sequence sequence, float start, float end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<CanvasGroup>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskAlpha(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnFillAmount(Sequence sequence, float start, float end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<Image>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskFillAmount(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnColor(Sequence sequence, Color start, Color end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<Graphic>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskColor(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnLocalPosition(Sequence sequence, Vector3 start, Vector3 end, bool isReversed) {
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskLocalPosition(sequence.TargetComp.transform, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnPivot(Sequence sequence, Vector3 start, Vector3 end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<RectTransform>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskPivot(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnSizeDelta(Sequence sequence, Vector3 start, Vector3 end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<RectTransform>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskSizeDelta(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnAnchorMin(Sequence sequence, Vector3 start, Vector3 end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<RectTransform>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskAnchorMin(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnAnchorMax(Sequence sequence, Vector3 start, Vector3 end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<RectTransform>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskAnchorMax(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnLocalScale(Sequence sequence, Vector3 start, Vector3 end, bool isReversed) {
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskLocalScale(sequence.TargetComp.transform, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnLocalEulerAngles(Sequence sequence, Vector3 start, Vector3 end, bool isReversed) {
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskLocalEulerAngles(sequence.TargetComp.transform, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		private void OnAnchoredPosition(Sequence sequence, Vector3 start, Vector3 end, bool isReversed) {
			var target = sequence.TargetComp.GetComponent<RectTransform>();
			var startValue = isReversed ? end : start;
			var endValue = isReversed ? start : end;
			_coroutines.Add(StartCoroutine(TaskAnchoredPosition(target, startValue, endValue, sequence.Duration, sequence.EaseFunction)));
		}

		#region Tasks

		#region RectTransform

		IEnumerator TaskAnchoredPosition(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				rt.anchoredPosition = Vector3.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			rt.anchoredPosition = end;
		}

		IEnumerator TaskAnchorMax(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				rt.anchorMax = Vector3.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			rt.anchorMax = end;
		}

		IEnumerator TaskAnchorMin(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				rt.anchorMin = Vector3.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			rt.anchorMin = end;
		}

		IEnumerator TaskSizeDelta(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				rt.sizeDelta = Vector3.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			rt.sizeDelta = end;
		}

		IEnumerator TaskPivot(RectTransform rt, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				rt.pivot = Vector3.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			rt.pivot = end;
		}

		#endregion RectTransform

		#region TransformTask

		IEnumerator TaskLocalPosition(Transform trans, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				trans.localPosition = Vector3.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			trans.localPosition = end;
		}

		IEnumerator TaskLocalScale(Transform trans, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				trans.localScale = Vector3.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			trans.localScale = end;
		}

		IEnumerator TaskLocalEulerAngles(Transform trans, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				trans.localEulerAngles = Vector3.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			trans.localEulerAngles = end;
		}

		#endregion TransformTask

		#region ImageTask

		IEnumerator TaskColor(Graphic img, Color start, Color end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				img.color = Color.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			img.color = end;
		}

		IEnumerator TaskFillAmount(Image img, float start, float end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				img.fillAmount = Mathf.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			img.fillAmount = end;
		}

		#endregion ImageTask

		#region CanvasGroupTask

		IEnumerator TaskAlpha(CanvasGroup cg, float start, float end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				cg.alpha = Mathf.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			cg.alpha = end;
		}

		#endregion CanvasGroupTask

		#region CameraTask

		IEnumerator TaskBackgroundColor(Camera cam, Color start, Color end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				cam.backgroundColor = Color.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			cam.backgroundColor = end;
		}

		IEnumerator TaskOrthographicSize(Camera cam, float start, float end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				cam.orthographicSize = Mathf.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			cam.orthographicSize = end;
		}

		#endregion ImageTask

		#region TextMeshProTask

		IEnumerator TaskMaxVisibleCharacters(TMP_Text text, float start, float end, float duration, Ease.Function easeFunction) {
			float startTime = Time.time;
			float t = (Time.time - startTime) / duration;
			while (t <= 1) {
				t = Mathf.Clamp((Time.time - startTime) / duration, 0, 2);
				text.maxVisibleCharacters = (int)Mathf.LerpUnclamped(start, end, easeFunction(t));
				yield return null;
			}

			text.maxVisibleCharacters = (int)end;
		}

		#endregion ImageTask

		#endregion Tasks

		#region Event

		IEnumerator AtTimeEvent(Action atTimeEvent, float time) {
			yield return new WaitForSecondsRealtime(time);
			atTimeEvent();
		}

		public AnimationUI AddFunctionAt(float time, Action func) {
			atTimes.Add(time);
			atTimeEvents.Add(func);
			return this;
		}

		#endregion Event

		#region Overidable

		void SetActiveAllInput(bool isActivating) {
			OnSetActiveAllInput?.Invoke(isActivating);
			AnimationUICustomizable.SetActiveAllInput(isActivating);
		}

		void PlaySound(AudioClip _SFXFile) {
			OnPlaySoundByFile?.Invoke(_SFXFile);
			AnimationUICustomizable.PlaySound(_SFXFile);
		}

		void PlaySound(int _index) {
			OnPlaySoundByIndex?.Invoke(_index);
			AnimationUICustomizable.PlaySound(_index);
		}

		#endregion

		public void PreviewAnimation() {
			InitFunction();
			if (UpdateSequence == null) {
				Debug.Log("No animation exist");
				return;
			}

			_startTime = Time.time;
			CurrentTime = 0;
			IsPlayingInEditMode = true;
			UpdateSequence(0); // Make sure the first frame is called
		}

		public void PreviewStart() {
			InitFunction();
			if (UpdateSequence == null) {
				Debug.Log("No animation exist");
				return;
			}

			CurrentTime = 0;
			IsPlayingInEditMode = false;
			UpdateSequence(0);

			Array.Reverse(AnimationSequence);
			foreach (Sequence sequence in AnimationSequence) {
				if (sequence.SequenceType == SequenceType.Animation) {
					if (sequence.TargetComp == null) {
						Debug.Log("Please assign Target for Sequence at " + sequence.StartTime.ToString() + "s");
						continue;
					}

					if (sequence.TargetType == SequenceObjectType.RectTransform) {
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchoredPosition))
							sequence.TargetComp.GetComponent<RectTransform>().anchoredPosition = sequence.AnchoredPositionStart;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.LocalEulerAngles))
							sequence.TargetComp.GetComponent<RectTransform>().localEulerAngles = sequence.LocalEulerAnglesStart;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.LocalScale))
							sequence.TargetComp.GetComponent<RectTransform>().localScale = sequence.LocalScaleStart;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchorMax))
							sequence.TargetComp.GetComponent<RectTransform>().anchorMax = sequence.AnchorMaxStart;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchorMin))
							sequence.TargetComp.GetComponent<RectTransform>().anchorMin = sequence.AnchorMinStart;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.SizeDelta))
							sequence.TargetComp.GetComponent<RectTransform>().sizeDelta = sequence.SizeDeltaStart;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.Pivot))
							sequence.TargetComp.GetComponent<RectTransform>().pivot = sequence.PivotStart;
					} else if (sequence.TargetType == SequenceObjectType.Transform) {
						if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalPosition))
							sequence.TargetComp.transform.localPosition = sequence.LocalPositionStart;
						if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalEulerAngles))
							sequence.TargetComp.transform.localEulerAngles = sequence.LocalEulerAnglesStart;
						if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalScale))
							sequence.TargetComp.transform.localScale = sequence.LocalScaleStart;
					} else if (sequence.TargetType == SequenceObjectType.Image) {
						if (sequence.TargetImgTask.HasFlag(SequenceImageTask.Color))
							sequence.TargetComp.GetComponent<Image>().color = sequence.ColorStart;
						if (sequence.TargetImgTask.HasFlag(SequenceImageTask.FillAmount))
							sequence.TargetComp.GetComponent<Image>().fillAmount = sequence.FillAmountStart;
					} else if (sequence.TargetType == SequenceObjectType.CanvasGroup) {
						if (sequence.TargetCgTask.HasFlag(Sequence.CgTask.Alpha))
							sequence.TargetComp.GetComponent<CanvasGroup>().alpha = sequence.AlphaStart;
					} else if (sequence.TargetType == SequenceObjectType.Camera) {
						if (sequence.TargetCamTask.HasFlag(SequenceCameraTask.BackgroundColor))
							sequence.TargetComp.GetComponent<Camera>().backgroundColor = sequence.BackgroundColorStart;
						if (sequence.TargetCamTask.HasFlag(SequenceCameraTask.OrthographicSize))
							sequence.TargetComp.GetComponent<Camera>().orthographicSize = sequence.OrthographicSizeStart;
					} else if (sequence.TargetType == SequenceObjectType.TextMeshPro) {
						if (sequence.TargetTextMeshProTask.HasFlag(SequenceTextMeshProTask.Color))
							sequence.TargetComp.GetComponent<TMP_Text>().color = sequence.TextMeshProColorStart;
						if (sequence.TargetTextMeshProTask.HasFlag(SequenceTextMeshProTask.MaxVisibleCharacters))
							sequence.TargetComp.GetComponent<TMP_Text>().maxVisibleCharacters = sequence.MaxVisibleCharactersStart;
					}
				} else if (sequence.SequenceType == SequenceType.Wait) { } else if (sequence.SequenceType == SequenceType.SetActiveAllInput) { } else if
					(sequence.SequenceType == SequenceType.SetActive) {
					if (sequence.Target == null) {
						// Debug.LogError("Please assign Target for Sequence at "+sequence.StartTime.ToString()+"s");
						continue;
					}

					sequence.Target.SetActive(sequence.IsActivating);
				} else if (sequence.SequenceType == SequenceType.SFX) { } else if (sequence.SequenceType == SequenceType.LoadScene) { } else if
					(sequence.SequenceType == SequenceType.UnityEvent) { }
			}

			Array.Reverse(AnimationSequence);
		}

		public void PreviewEnd() {
			CurrentTime = TotalDuration;
			InitFunction();
			if (UpdateSequence == null) {
				Debug.Log("No animation exist");
				return;
			}

			IsPlayingInEditMode = false;
			CurrentTime = TotalDuration;
			UpdateSequence(Mathf.Clamp01(TotalDuration));

			foreach (Sequence sequence in AnimationSequence) {
				if (sequence.SequenceType == SequenceType.Animation) {
					if (sequence.TargetComp == null) {
						Debug.Log("Please assign Target for Sequence at " + sequence.StartTime.ToString() + "s");
						continue;
					}

					if (sequence.TargetType == SequenceObjectType.RectTransform) {
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchoredPosition))
							sequence.TargetComp.GetComponent<RectTransform>().anchoredPosition = sequence.AnchoredPositionEnd;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.LocalEulerAngles))
							sequence.TargetComp.GetComponent<RectTransform>().localEulerAngles = sequence.LocalEulerAnglesEnd;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.LocalScale))
							sequence.TargetComp.GetComponent<RectTransform>().localScale = sequence.LocalScaleEnd;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchorMax))
							sequence.TargetComp.GetComponent<RectTransform>().anchorMax = sequence.AnchorMaxEnd;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchorMin))
							sequence.TargetComp.GetComponent<RectTransform>().anchorMin = sequence.AnchorMinEnd;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.SizeDelta))
							sequence.TargetComp.GetComponent<RectTransform>().sizeDelta = sequence.SizeDeltaEnd;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.Pivot))
							sequence.TargetComp.GetComponent<RectTransform>().pivot = sequence.PivotEnd;
					} else if (sequence.TargetType == SequenceObjectType.Transform) {
						if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalPosition))
							sequence.TargetComp.transform.localPosition = sequence.LocalPositionEnd;
						if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalEulerAngles))
							sequence.TargetComp.transform.localEulerAngles = sequence.LocalEulerAnglesEnd;
						if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalScale))
							sequence.TargetComp.transform.localScale = sequence.LocalScaleEnd;
					} else if (sequence.TargetType == SequenceObjectType.Image) {
						if (sequence.TargetImgTask.HasFlag(SequenceImageTask.Color))
							sequence.TargetComp.GetComponent<Image>().color = sequence.ColorEnd;
						if (sequence.TargetImgTask.HasFlag(SequenceImageTask.FillAmount))
							sequence.TargetComp.GetComponent<Image>().fillAmount = sequence.FillAmountEnd;
					} else if (sequence.TargetType == SequenceObjectType.CanvasGroup) {
						if (sequence.TargetCgTask.HasFlag(Sequence.CgTask.Alpha))
							sequence.TargetComp.GetComponent<CanvasGroup>().alpha = sequence.AlphaEnd;
					} else if (sequence.TargetType == SequenceObjectType.Camera) {
						if (sequence.TargetCamTask.HasFlag(SequenceCameraTask.BackgroundColor))
							sequence.TargetComp.GetComponent<Camera>().backgroundColor = sequence.BackgroundColorEnd;
						if (sequence.TargetCamTask.HasFlag(SequenceCameraTask.OrthographicSize))
							sequence.TargetComp.GetComponent<Camera>().orthographicSize = sequence.OrthographicSizeEnd;
					} else if (sequence.TargetType == SequenceObjectType.TextMeshPro) {
						if (sequence.TargetTextMeshProTask.HasFlag(SequenceTextMeshProTask.Color))
							sequence.TargetComp.GetComponent<TMP_Text>().color = sequence.TextMeshProColorEnd;
						if (sequence.TargetTextMeshProTask.HasFlag(SequenceTextMeshProTask.MaxVisibleCharacters))
							sequence.TargetComp.GetComponent<TMP_Text>().maxVisibleCharacters = sequence.MaxVisibleCharactersEnd;
					}
				} else if (sequence.SequenceType == SequenceType.Wait) { } else if (sequence.SequenceType == SequenceType.SetActiveAllInput) { } else if
					(sequence.SequenceType == SequenceType.SetActive) {
					if (sequence.Target == null) {
						// Debug.LogError("Please assign Target for Sequence at "+sequence.StartTime.ToString()+"s");
						continue;
					}

					sequence.Target.SetActive(!sequence.IsActivating);
				} else if (sequence.SequenceType == SequenceType.SFX) { } else if (sequence.SequenceType == SequenceType.LoadScene) { } else if
					(sequence.SequenceType == SequenceType.UnityEvent) { }
			}
		}

		//For the default value. A hacky way because the inspector reset the value for Serialized class
		void Reset() => AnimationSequence = new Sequence[1] { new() };

		#region timing

		public void InitTime() {
			TotalDuration = 0;
			foreach (Sequence sequence in AnimationSequence) {
				TotalDuration += (sequence.SequenceType == SequenceType.Wait) ? sequence.Duration : 0;
			}

			// for case when the duration of a non wait is bigger
			float currentTimeCheck = 0;
			float tempTotalDuration = TotalDuration;
			foreach (Sequence sequence in AnimationSequence) {
				currentTimeCheck += (sequence.SequenceType == SequenceType.Wait) ? sequence.Duration : 0;
				if (sequence.SequenceType == SequenceType.Animation) {
					if (TotalDuration < currentTimeCheck + sequence.Duration) {
						TotalDuration = currentTimeCheck + sequence.Duration;
					}
				}
			}

			CurrentTime = Mathf.Clamp(CurrentTime, 0, TotalDuration);
		}

		#endregion timing

		void InitFunction() //For preview
		{
			UpdateSequence = null;
			foreach (Sequence sequence in AnimationSequence) {
				// sequence.IsDone = false;
				if (sequence.SequenceType == SequenceType.Animation) {
					if (sequence.TargetComp == null) {
						Debug.Log("Please assign Target");
						return;
					}

					sequence.Init();
					if (sequence.TargetType == SequenceObjectType.RectTransform) {
						RectTransform rt = sequence.TargetComp.GetComponent<RectTransform>();

						void RtAnchoredPosition(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.AnchoredPositionState = SequenceState.During;
								rt.anchoredPosition
									= Vector3.LerpUnclamped(
										sequence.AnchoredPositionStart,
										sequence.AnchoredPositionEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.AnchoredPositionState == SequenceState.During ||
							     sequence.AnchoredPositionState == SequenceState.Before)) {
								rt.anchoredPosition = sequence.AnchoredPositionEnd;
								sequence.AnchoredPositionState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.AnchoredPositionState == SequenceState.During ||
							            sequence.AnchoredPositionState == SequenceState.After)) {
								rt.anchoredPosition = sequence.AnchoredPositionStart;
								sequence.AnchoredPositionState = SequenceState.Before;
							}
						}

						void RtLocalEulerAngles(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.LocalEulerAnglesState = SequenceState.During;
								rt.localEulerAngles
									= Vector3.LerpUnclamped(
										sequence.LocalEulerAnglesStart,
										sequence.LocalEulerAnglesEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.LocalEulerAnglesState == SequenceState.During ||
							     sequence.LocalEulerAnglesState == SequenceState.Before)) {
								rt.localEulerAngles = sequence.LocalEulerAnglesEnd;
								sequence.LocalEulerAnglesState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.LocalEulerAnglesState == SequenceState.During ||
							            sequence.LocalEulerAnglesState == SequenceState.After)) {
								rt.localEulerAngles = sequence.LocalEulerAnglesStart;
								sequence.LocalEulerAnglesState = SequenceState.Before;
							}
						}

						void RtLocalScale(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.LocalScaleState = SequenceState.During;
								rt.localScale
									= Vector3.LerpUnclamped(
										sequence.LocalScaleStart,
										sequence.LocalScaleEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.LocalScaleState == SequenceState.During ||
							     sequence.LocalScaleState == SequenceState.Before)) {
								rt.localScale = sequence.LocalScaleEnd;
								sequence.LocalScaleState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.LocalScaleState == SequenceState.During ||
							            sequence.LocalScaleState == SequenceState.After)) {
								rt.localScale = sequence.LocalScaleStart;
								sequence.LocalScaleState = SequenceState.Before;
							}
						}

						void RtSizeDelta(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.SizeDeltaState = SequenceState.During;
								rt.sizeDelta
									= Vector3.LerpUnclamped(
										sequence.SizeDeltaStart,
										sequence.SizeDeltaEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.SizeDeltaState == SequenceState.During ||
							     sequence.SizeDeltaState == SequenceState.Before)) {
								rt.sizeDelta = sequence.SizeDeltaEnd;
								sequence.SizeDeltaState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.SizeDeltaState == SequenceState.During ||
							            sequence.SizeDeltaState == SequenceState.After)) {
								rt.sizeDelta = sequence.SizeDeltaStart;
								sequence.SizeDeltaState = SequenceState.Before;
							}
						}

						void RtAnchorMin(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.AnchorMinState = SequenceState.During;
								rt.anchorMin
									= Vector3.LerpUnclamped(
										sequence.AnchorMinStart,
										sequence.AnchorMinEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.AnchorMinState == SequenceState.During ||
							     sequence.AnchorMinState == SequenceState.Before)) {
								rt.anchorMin = sequence.AnchorMinEnd;
								sequence.AnchorMinState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.AnchorMinState == SequenceState.During ||
							            sequence.AnchorMinState == SequenceState.After)) {
								rt.anchorMin = sequence.AnchorMinStart;
								sequence.AnchorMinState = SequenceState.Before;
							}
						}

						void RtAnchorMax(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.AnchorMaxState = SequenceState.During;
								rt.anchorMax
									= Vector3.LerpUnclamped(
										sequence.AnchorMaxStart,
										sequence.AnchorMaxEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.AnchorMaxState == SequenceState.During ||
							     sequence.AnchorMaxState == SequenceState.Before)) {
								rt.anchorMax = sequence.AnchorMaxEnd;
								sequence.AnchorMaxState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.AnchorMaxState == SequenceState.During ||
							            sequence.AnchorMaxState == SequenceState.After)) {
								rt.anchorMax = sequence.AnchorMaxStart;
								sequence.AnchorMaxState = SequenceState.Before;
							}
						}

						void RtPivot(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.PivotState = SequenceState.During;
								rt.pivot
									= Vector3.LerpUnclamped(
										sequence.PivotStart,
										sequence.PivotEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.PivotState == SequenceState.During ||
							     sequence.PivotState == SequenceState.Before)) {
								rt.pivot = sequence.PivotEnd;
								sequence.PivotState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.PivotState == SequenceState.During ||
							            sequence.PivotState == SequenceState.After)) {
								rt.pivot = sequence.PivotStart;
								sequence.PivotState = SequenceState.Before;
							}
						}

						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchoredPosition))
							UpdateSequence += RtAnchoredPosition;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.LocalEulerAngles))
							UpdateSequence += RtLocalEulerAngles;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.LocalScale))
							UpdateSequence += RtLocalScale;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.SizeDelta))
							UpdateSequence += RtSizeDelta;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchorMax))
							UpdateSequence += RtAnchorMax;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.AnchorMin))
							UpdateSequence += RtAnchorMin;
						if (sequence.TargetRtTask.HasFlag(SequenceRectTransformTask.Pivot))
							UpdateSequence += RtPivot;
					} else if (sequence.TargetType == SequenceObjectType.Transform) {
						Transform trans = sequence.TargetComp.transform;

						void TransLocalPosition(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.LocalPositionState = SequenceState.During;
								trans.localPosition
									= Vector3.LerpUnclamped(
										sequence.LocalPositionStart,
										sequence.LocalPositionEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.LocalPositionState == SequenceState.During ||
							     sequence.LocalPositionState == SequenceState.Before)) {
								trans.localPosition = sequence.LocalPositionEnd;
								sequence.LocalPositionState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.LocalPositionState == SequenceState.During ||
							            sequence.LocalPositionState == SequenceState.After)) {
								trans.localPosition = sequence.LocalPositionStart;
								sequence.LocalPositionState = SequenceState.Before;
							}
						}

						void TransLocalEulerAngles(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.LocalEulerAnglesState = SequenceState.During;
								trans.localEulerAngles
									= Vector3.LerpUnclamped(
										sequence.LocalEulerAnglesStart,
										sequence.LocalEulerAnglesEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.LocalEulerAnglesState == SequenceState.During ||
							     sequence.LocalEulerAnglesState == SequenceState.Before)) {
								trans.localEulerAngles = sequence.LocalEulerAnglesEnd;
								sequence.LocalEulerAnglesState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.LocalEulerAnglesState == SequenceState.During ||
							            sequence.LocalEulerAnglesState == SequenceState.After)) {
								trans.localEulerAngles = sequence.LocalEulerAnglesStart;
								sequence.LocalEulerAnglesState = SequenceState.Before;
							}
						}

						void TransLocalScale(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.LocalScaleState = SequenceState.During;
								trans.localScale
									= Vector3.LerpUnclamped(
										sequence.LocalScaleStart,
										sequence.LocalScaleEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.LocalScaleState == SequenceState.During ||
							     sequence.LocalScaleState == SequenceState.Before)) {
								trans.localScale = sequence.LocalScaleEnd;
								sequence.LocalScaleState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.LocalScaleState == SequenceState.During ||
							            sequence.LocalScaleState == SequenceState.After)) {
								trans.localScale = sequence.LocalScaleStart;
								sequence.LocalScaleState = SequenceState.Before;
							}
						}

						if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalPosition))
							UpdateSequence += TransLocalPosition;
						if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalEulerAngles))
							UpdateSequence += TransLocalEulerAngles;
						if (sequence.TargetTransTask.HasFlag(SequenceTransformTask.LocalScale))
							UpdateSequence += TransLocalScale;
					} else if (sequence.TargetType == SequenceObjectType.Image) {
						Image img = sequence.TargetComp.GetComponent<Image>();

						void ImgColor(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.ColorState = SequenceState.During;
								img.color
									= Color.LerpUnclamped(
										sequence.ColorStart,
										sequence.ColorEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.ColorState == SequenceState.During ||
							     sequence.ColorState == SequenceState.Before)) {
								img.color = sequence.ColorEnd;
								sequence.ColorState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.ColorState == SequenceState.During ||
							            sequence.ColorState == SequenceState.After)) {
								img.color = sequence.ColorStart;
								sequence.ColorState = SequenceState.Before;
							}
						}

						void ImgFillAmount(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.ColorState = SequenceState.During;
								img.fillAmount
									= Mathf.LerpUnclamped(
										sequence.FillAmountStart,
										sequence.FillAmountEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.ColorState == SequenceState.During ||
							     sequence.ColorState == SequenceState.Before)) {
								img.fillAmount = sequence.FillAmountEnd;
								sequence.ColorState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.ColorState == SequenceState.During ||
							            sequence.ColorState == SequenceState.After)) {
								img.fillAmount = sequence.FillAmountStart;
								sequence.ColorState = SequenceState.Before;
							}
						}

						if (sequence.TargetImgTask.HasFlag(SequenceImageTask.Color))
							UpdateSequence += ImgColor;
						if (sequence.TargetImgTask.HasFlag(SequenceImageTask.FillAmount))
							UpdateSequence += ImgFillAmount;
					} else if (sequence.TargetType == SequenceObjectType.CanvasGroup) {
						CanvasGroup cg = sequence.TargetComp.GetComponent<CanvasGroup>();

						void CgAlpha(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.AlphaState = SequenceState.During;
								cg.alpha
									= Mathf.LerpUnclamped(
										sequence.AlphaStart,
										sequence.AlphaEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.AlphaState == SequenceState.During ||
							     sequence.AlphaState == SequenceState.Before)) {
								cg.alpha = sequence.AlphaEnd;
								sequence.AlphaState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.AlphaState == SequenceState.During ||
							            sequence.AlphaState == SequenceState.After)) {
								cg.alpha = sequence.AlphaStart;
								sequence.AlphaState = SequenceState.Before;
							}
						}

						if (sequence.TargetCgTask.HasFlag(Sequence.CgTask.Alpha))
							UpdateSequence += CgAlpha;
					} else if (sequence.TargetType == SequenceObjectType.Camera) {
						Camera cam = sequence.TargetComp.GetComponent<Camera>();

						void CamBackgroundColor(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.BackgroundColorState = SequenceState.During;
								cam.backgroundColor
									= Color.LerpUnclamped(
										sequence.BackgroundColorStart,
										sequence.BackgroundColorEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.BackgroundColorState == SequenceState.During ||
							     sequence.BackgroundColorState == SequenceState.Before)) {
								cam.backgroundColor = sequence.BackgroundColorEnd;
								sequence.BackgroundColorState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.BackgroundColorState == SequenceState.During ||
							            sequence.BackgroundColorState == SequenceState.After)) {
								cam.backgroundColor = sequence.BackgroundColorStart;
								sequence.BackgroundColorState = SequenceState.Before;
							}
						}

						void CamOrthographicSize(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.OrthographicSizeState = SequenceState.During;
								cam.orthographicSize
									= Mathf.LerpUnclamped(
										sequence.OrthographicSizeStart,
										sequence.OrthographicSizeEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.OrthographicSizeState == SequenceState.During ||
							     sequence.OrthographicSizeState == SequenceState.Before)) {
								cam.orthographicSize = sequence.OrthographicSizeEnd;
								sequence.OrthographicSizeState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.OrthographicSizeState == SequenceState.During ||
							            sequence.OrthographicSizeState == SequenceState.After)) {
								cam.orthographicSize = sequence.OrthographicSizeStart;
								sequence.OrthographicSizeState = SequenceState.Before;
							}
						}

						if (sequence.TargetCamTask.HasFlag(SequenceCameraTask.BackgroundColor))
							UpdateSequence += CamBackgroundColor;
						if (sequence.TargetCamTask.HasFlag(SequenceCameraTask.OrthographicSize))
							UpdateSequence += CamOrthographicSize;
					} else if (sequence.TargetType == SequenceObjectType.TextMeshPro) {
						TMP_Text text = sequence.TargetComp.GetComponent<TMP_Text>();

						void TextMeshProColor(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.TextMeshProColorState = SequenceState.During;
								text.color
									= Color.LerpUnclamped(
										sequence.TextMeshProColorStart,
										sequence.TextMeshProColorEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.TextMeshProColorState == SequenceState.During ||
							     sequence.TextMeshProColorState == SequenceState.Before)) {
								text.color = sequence.TextMeshProColorEnd;
								sequence.TextMeshProColorState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.TextMeshProColorState == SequenceState.During ||
							            sequence.TextMeshProColorState == SequenceState.After)) {
								text.color = sequence.TextMeshProColorStart;
								sequence.TextMeshProColorState = SequenceState.Before;
							}
						}

						void MaxVisibleCharacters(float t) {
							if ((0 <= t - sequence.StartTime) && (t - sequence.StartTime < sequence.Duration)) {
								sequence.MaxVisibleCharactersState = SequenceState.During;
								text.maxVisibleCharacters
									= (int)Mathf.LerpUnclamped(
										sequence.MaxVisibleCharactersStart,
										sequence.MaxVisibleCharactersEnd,
										sequence.EaseFunction(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration))
									);
							}

							if ((t - sequence.StartTime >= sequence.Duration) &&
							    (sequence.MaxVisibleCharactersState == SequenceState.During ||
							     sequence.MaxVisibleCharactersState == SequenceState.Before)) {
								text.maxVisibleCharacters = sequence.MaxVisibleCharactersEnd;
								sequence.MaxVisibleCharactersState = SequenceState.After;
							} else if ((t - sequence.StartTime < 0) &&
							           (sequence.MaxVisibleCharactersState == SequenceState.During ||
							            sequence.MaxVisibleCharactersState == SequenceState.After)) {
								text.maxVisibleCharacters = sequence.MaxVisibleCharactersStart;
								sequence.MaxVisibleCharactersState = SequenceState.Before;
							}
						}

						if (sequence.TargetTextMeshProTask.HasFlag(SequenceTextMeshProTask.Color))
							UpdateSequence += TextMeshProColor;
						if (sequence.TargetTextMeshProTask.HasFlag(SequenceTextMeshProTask.MaxVisibleCharacters))
							UpdateSequence += MaxVisibleCharacters;
					} else if (sequence.TargetType == SequenceObjectType.UnityEventDynamic) {
						Image img = sequence.TargetComp.GetComponent<Image>();

						void EventDynamic(float t) {
							if (t - sequence.StartTime < 0)
								return;
							sequence.EventDynamic?.Invoke(Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration));
						}

						UpdateSequence += EventDynamic;
					}
				} else if (sequence.SequenceType == SequenceType.Wait) { } else if (sequence.SequenceType == SequenceType.SetActiveAllInput) {
					// void SetActiveALlInput(float t)
					// {
					//     float time = Mathf.Clamp01((t-sequence.StartTime)/sequence.Duration);
					//     if(!sequence.IsDone) // so that SetActiveAllInput in the first frame can also be called
					//     {
					//         if(t - sequence.StartTime > -0.01f)
					//         {
					//             sequence.IsDone = true;
					//             SetActiveAllInput(sequence.IsActivating);
					//         }
					//     }
					//     else if(t - sequence.StartTime < 0)
					//     {
					//         sequence.IsDone = false;
					//         SetActiveAllInput(!sequence.IsActivating);
					//     }
					// }
					// sequence.IsDone = false;
					// UpdateSequence += SetActiveALlInput;
				} else if (sequence.SequenceType == SequenceType.SetActive) {
					void SetActive(float t) {
						var time = Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration);
						if (sequence.Target == null) {
							Debug.Log("Please assign Target for Sequence at " + sequence.StartTime.ToString() + "s");
							return;
						}

						if (!sequence.IsDone) {
							if (t - sequence.StartTime >= sequence.Duration) {
								sequence.IsDone = true;
								sequence.Target.SetActive(sequence.IsActivating);
							}
						} else if (t - sequence.StartTime < 0) // IsDone == true && t-sequence.StartTime < 0 
						{
							sequence.IsDone = false;
							sequence.Target.SetActive(!sequence.IsActivating);
						}
					}

					UpdateSequence += SetActive;
				} else if (sequence.SequenceType == SequenceType.SFX) {
					// void SFX(float t)
					// {
					//     float time = Mathf.Clamp01((t-sequence.StartTime)/sequence.Duration);
					//     if(!sequence.IsDone) // so that SetActiveAllInput in the first frame can also be called
					//     {
					//         if(t - sequence.StartTime > -0.01f)
					//         {
					//             sequence.IsDone = true;
					//             if(sequence.PlaySFXBy == Sequence.SFXMethod.File)
					//             {
					//                 if(sequence.SFXFile == null)
					//                 {
					//                     // Debug.LogWarning("Please assign SFX for Sequence at "+sequence.StartTime.ToString()+"s");
					//                     return;
					//                 }
					//                 PlaySound(sequence.SFXFile);
					//             }
					//             else
					//                 PlaySound(sequence.SFXIndex);
					//         }
					//     }
					//     else if(t - sequence.StartTime < 0)
					//     {
					//         if(sequence.PlaySFXBy == Sequence.SFXMethod.File)
					//         {
					//             if(sequence.SFXFile == null)
					//             {
					//                 // Debug.LogWarning("Please assign SFX for Sequence at "+sequence.StartTime.ToString()+"s");
					//                 return;
					//             }
					//             PlaySound(sequence.SFXFile);
					//         }
					//         else
					//             PlaySound(sequence.SFXIndex);

					//         sequence.IsDone = false;
					//     }
					// }
					// sequence.IsDone = false;
					// UpdateSequence += SFX;
				} else if (sequence.SequenceType == SequenceType.UnityEvent) {
					void UnityEvent(float t) {
						float time = Mathf.Clamp01((t - sequence.StartTime) / sequence.Duration);
						if (!sequence.IsDone) {
							// -0.01f so that SetActiveAllInput in the first frame can also be called
							if (t - sequence.StartTime > -0.01f) //Nested conditional may actually more performant in this case
							{
								sequence.IsDone = true;
								sequence.Event?.Invoke();
							}
						} else if (t - sequence.StartTime < 0) {
							sequence.IsDone = false;
							sequence.Event?.Invoke();
						}
					}

					sequence.IsDone = false;
					UpdateSequence += UnityEvent;
				}
			}
		}

#if UNITY_EDITOR
		void OnEnable() => UnityEditor.EditorApplication.update += EditorUpdate;
		void OnDisable() => UnityEditor.EditorApplication.update -= EditorUpdate;

		void ForceRepaint() {
			if (!Application.isPlaying) {
				UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
				UnityEditor.SceneView.RepaintAll();
				UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
			}
		}

		void OnDrawGizmos() => ForceRepaint();

		void EditorUpdate() {
			if (Application.isPlaying)
				return;
			ForceRepaint();

			if (IsPlayingInEditMode && CurrentTime < TotalDuration) {
				CurrentTime = Mathf.Clamp(Time.time - _startTime, 0, TotalDuration);
				UpdateSequence(CurrentTime);
			} else {
				if (UpdateSequence != null && IsPlayingInEditMode)
					UpdateSequence(TotalDuration); //Make sure the latest frame is called
				IsPlayingInEditMode = false;
			}
		}

		public void UpdateBySlider() {
			if (Application.isPlaying)
				return;
			if (IsPlayingInEditMode)
				return;
			InitFunction();
			if (UpdateSequence != null)
				UpdateSequence(CurrentTime);
		}
#endif
	}
}