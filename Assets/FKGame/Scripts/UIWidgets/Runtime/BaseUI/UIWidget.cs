using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
//------------------------------------------------------------------------
// UI组件基类，提供给自己继承实现
// 可以通过 WidgetUtility.Find<T>(name) 进行按名称和分类进行搜索
//------------------------------------------------------------------------
namespace FKGame.UIWidgets
{
	[RequireComponent(typeof(CanvasGroup))]
	public class UIWidget : ComponentCallbackHandler
	{
		[Tooltip("组件名称，可通过 WidgetUtility.Find<T>(name) 进行按名称和分类进行搜索")]
		[SerializeField]
		protected new string name;
		public string Name {
			get { return name; }
			set { name = value; }
		}

		// Inspector回调
		public override string[] Callbacks
		{
			get
			{
				return new string[]{
					"OnShow",
					"OnClose",
				};
			}
		}

        [Tooltip("WidgetUtility.Find<T>(name) 搜索时的优先级")]
		[Range(0, 100)]
		public int priority;

		[Header("Appearence")]
		[Tooltip("开启/关闭的快捷键")]
		[SerializeField]
		protected KeyCode m_KeyCode = KeyCode.None;

		[Tooltip("小部件的渐变缓动方程")]
		[SerializeField]
		private EasingEquations.EaseType m_EaseType = EasingEquations.EaseType.EaseInOutBack;

		[Tooltip("小部件的渐变补渐的持续时间")]
		[SerializeField]
		protected float m_Duration = 0.7f;

		[SerializeField]
		protected bool m_IgnoreTimeScale = true;
		public bool IgnoreTimeScale { get { return this.m_IgnoreTimeScale; } }

		[Tooltip("显示UI时的音效")]
		[SerializeField]
        protected AudioClip m_ShowSound;
		[Tooltip("关闭UI时的音效")]
		[SerializeField]
		protected AudioClip m_CloseSound;
		[Tooltip("是否被聚焦。如果被聚焦将被显示在最前面")]
		[SerializeField]
        protected bool m_Focus = true;
		[Tooltip("如果该值为True，关闭该对象后将停止它，以防止每帧调用update()")]
        [SerializeField]
		protected bool m_DeactivateOnClose = true;
		[Tooltip("显示该窗口时启用光标，当窗口关闭或角色移动时再次隐藏光标")]
		[SerializeField]
		protected bool m_ShowAndHideCursor = false;
		[SerializeField]
		protected string m_CameraPreset = "UI";
		[Tooltip("当角色移动时隐藏该窗口")]
		[SerializeField]
		protected bool m_CloseOnMove = true;
		[Tooltip("该选项将允许聚焦和旋转角色，一般只有第三角色摄像机和FocusTarget组件才需要")]
		[SerializeField]
		protected bool m_FocusPlayer = false;

		protected static CursorLockMode m_PreviousCursorLockMode;
		protected static bool m_PreviousCursorVisibility;
		protected Transform m_CameraTransform;
		protected MonoBehaviour m_CameraController;
		protected MonoBehaviour m_ThirdPersonController;
		protected static bool m_PreviousCameraControllerEnabled;
		protected static List<UIWidget> m_CurrentVisibleWidgets=new List<UIWidget>();
		protected CursorLockMode m_PreviousLockMode;

		// 本组件是否可见
		public bool IsVisible { 
			get {
				if (this.m_CanvasGroup == null)
					this.m_CanvasGroup = GetComponent<CanvasGroup>();
				return m_CanvasGroup.alpha == 1f; 
			} 
		}

		protected RectTransform m_RectTransform;
		protected CanvasGroup m_CanvasGroup;
		protected bool m_IsShowing;

		private TweenRunner<FloatTween> m_AlphaTweenRunner;
		private TweenRunner<Vector3Tween> m_ScaleTweenRunner;

		protected Scrollbar[] m_Scrollbars;

		protected bool m_IsLocked = false;
		public bool IsLocked
		{
			get { return this.m_IsLocked; }
		}

		private void Awake ()
		{
			ComponentWidgetInputHandler.RegisterInput(this.m_KeyCode, this);
			m_RectTransform = GetComponent<RectTransform> ();
			m_CanvasGroup = GetComponent<CanvasGroup> ();
			this.m_Scrollbars = GetComponentsInChildren<Scrollbar>();
			this.m_CameraTransform = Camera.main.transform;
			this.m_CameraController = this.m_CameraTransform.GetComponent("ThirdPersonCamera") as MonoBehaviour;
			PlayerInfo playerInfo = new PlayerInfo("Player");

			if (playerInfo.gameObject != null)
				this.m_ThirdPersonController = playerInfo.gameObject.GetComponent("ThirdPersonController") as MonoBehaviour;

			if (!IsVisible) {
				m_RectTransform.localScale = Vector3.zero;
			}
			if (this.m_AlphaTweenRunner == null)
				this.m_AlphaTweenRunner = new TweenRunner<FloatTween> ();
			this.m_AlphaTweenRunner.Init (this);

			if (this.m_ScaleTweenRunner == null)
				this.m_ScaleTweenRunner = new TweenRunner<Vector3Tween> ();
			this.m_ScaleTweenRunner.Init (this);
            m_IsShowing = IsVisible;

			OnAwake ();
		}

		protected virtual void OnAwake ()
		{
		}

		private void Start ()
		{
			OnStart ();
			StartCoroutine (OnDelayedStart ());
		}

		protected virtual void OnStart ()
		{
		}

		private IEnumerator OnDelayedStart ()
		{
			yield return null;
			if (!IsVisible && m_DeactivateOnClose) {
				gameObject.SetActive (false);
			}
		}

		protected virtual void Update() {
			if (this.m_ShowAndHideCursor && this.IsVisible && this.m_CloseOnMove && (this.m_ThirdPersonController == null || this.m_ThirdPersonController.enabled) && (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f))
			{
				Close();
			}
		}

		// 显示UI组件
		public virtual void Show ()
		{
            if (this.m_IsShowing) {
                return;
            }
            this.m_IsShowing = true;
			gameObject.SetActive (true);
            if (this.m_Focus) {
				Focus ();
			}
			TweenCanvasGroupAlpha (m_CanvasGroup.alpha, 1f);
			TweenTransformScale (Vector3.ClampMagnitude (m_RectTransform.localScale, 1.9f), Vector3.one);
			
			WidgetUtility.PlaySound (this.m_ShowSound, 1.0f);
			m_CanvasGroup.interactable = true;
			m_CanvasGroup.blocksRaycasts = true;
			Canvas.ForceUpdateCanvases();
			for (int i = 0; i < this.m_Scrollbars.Length; i++)
			{
				this.m_Scrollbars[i].value = 1f;
			}
			if (this.m_ShowAndHideCursor) {
				m_CurrentVisibleWidgets.Add(this);

				if (this.m_FocusPlayer && !this.m_IsLocked)
					this.m_CameraController.SendMessage("Focus", true, SendMessageOptions.DontRequireReceiver);

				if ( m_CurrentVisibleWidgets.Count == 1)
				{
					if (m_CameraController != null)
					{
						this.m_CameraTransform.SendMessage("Activate", this.m_CameraPreset, SendMessageOptions.DontRequireReceiver);
					}else {
						this.m_PreviousLockMode = Cursor.lockState;
						Cursor.lockState = CursorLockMode.None;
						Cursor.visible = true;
					}
				}
			}

			Execute("OnShow", new CallbackEventData());
		}

		// 关闭UI组件
		public virtual void Close ()
		{
            if (!m_IsShowing) {
                return;
            }
            m_IsShowing = false;
			TweenCanvasGroupAlpha (m_CanvasGroup.alpha, 0f);
			TweenTransformScale (m_RectTransform.localScale, Vector3.zero);
			
			WidgetUtility.PlaySound (this.m_CloseSound, 1.0f);
			m_CanvasGroup.interactable = false;
			m_CanvasGroup.blocksRaycasts = false;
			if (this.m_ShowAndHideCursor) {
				m_CurrentVisibleWidgets.Remove(this);

				if(m_CurrentVisibleWidgets.Find(x=>x.m_FocusPlayer) == null)
					this.m_CameraController.SendMessage("Focus", false, SendMessageOptions.DontRequireReceiver);

				if (m_CurrentVisibleWidgets.Count == 0) {

					if (this.m_CameraController != null)
					{
						this.m_CameraTransform.SendMessage("Deactivate", this.m_CameraPreset, SendMessageOptions.DontRequireReceiver);
					}
					else {
						Cursor.lockState = this.m_PreviousLockMode;
						Cursor.visible = false;
					}
				}
			}

			Execute("OnClose", new CallbackEventData());

		}

		private void TweenCanvasGroupAlpha (float startValue, float targetValue)
		{
				FloatTween alphaTween = new FloatTween {
					easeType = m_EaseType,
					duration = m_Duration,
					startValue = startValue,
					targetValue = targetValue,
					ignoreTimeScale = m_IgnoreTimeScale
				};

				alphaTween.AddOnChangedCallback ((float value) => {
					m_CanvasGroup.alpha = value;
				});
				alphaTween.AddOnFinishCallback (() => {
					if (alphaTween.startValue > alphaTween.targetValue) {
						if (m_DeactivateOnClose && !this.m_IsShowing) {
							gameObject.SetActive (false);
						}
					} 
				});
			
			m_AlphaTweenRunner.StartTween (alphaTween);
		}

		private void TweenTransformScale (Vector3 startValue, Vector3 targetValue)
		{
            Vector3Tween scaleTween = new Vector3Tween
            {
                easeType = m_EaseType,
                duration = m_Duration,
                startValue = startValue,
                targetValue = targetValue,
				ignoreTimeScale = m_IgnoreTimeScale
			};
            scaleTween.AddOnChangedCallback((Vector3 value) => {
                m_RectTransform.localScale = value;
            });

            m_ScaleTweenRunner.StartTween(scaleTween);
        }

		// 更变可见性
		public virtual void Toggle ()
		{
			if (!IsVisible) {
				Show ();
			} else {
				Close ();
			}
		}

		// 选中后将其设置为最上层
		public virtual void Focus ()
		{
			m_RectTransform.SetAsLastSibling ();
		}

		protected virtual void OnDestroy() {
			ComponentWidgetInputHandler.UnregisterInput(this.m_KeyCode, this);
		}

		public void Lock(bool state)
		{
			this.m_IsLocked = state;
		}

		public static void LockAll(bool state) {
			UIWidget[] widgets = WidgetUtility.FindAll<UIWidget>();
			for (int i = 0; i < widgets.Length; i++)
			{
				widgets[i].Lock(state);
			}
		}
	}
}