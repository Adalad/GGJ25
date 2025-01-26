using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : BubbleComponent
{
    public Slider BlowSlider;
    public CinemachineCamera PlayerCamera;
    [SerializeField]
    private float m_MoveSpeed = 1f;
    [SerializeField]
    private float m_MaxMove = 1f;
    private InputActions m_InputActions;
    private Coroutine m_MoveRoutine;
    private float m_MoveAmmount;
    private float m_MoveAcum;
    private bool m_Ready;

    private void Awake()
    {
        BlowSlider.gameObject.SetActive(false);
        m_InputActions = new InputActions();
        m_Ready = false;
        GameManager.Instance.BeginPlay += OnBeginPlay;
        Initialize();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BeginPlay -= OnBeginPlay;
        }
    }

    private void OnEnable()
    {
        if (m_Ready)
        {
            EnableInput();
        }
    }

    private void OnDisable()
    {
        DisableInput();
    }

    public void OnInteractStart(InputAction.CallbackContext context)
    {
        if (m_MoveRoutine == null)
        {
            m_MoveRoutine = StartCoroutine(MoveRoutine());
        }
    }

    public void OnInteractCanceled(InputAction.CallbackContext context)
    {
        if (m_MoveRoutine != null)
        {
            StopCoroutine(m_MoveRoutine);
            m_MoveRoutine = null;
        }
    }

    protected override void Initialize()
    {
        m_CurrentAge = -1;
    }

    private IEnumerator MoveRoutine()
    {
        GameManager.Instance.TryAdvance();
        Vector3 newPosition = transform.position;
        m_MoveAcum = 0f;
        while (true)
        {
            m_MoveAmmount += m_MoveSpeed * Time.deltaTime;
            m_MoveAcum += m_MoveSpeed * Time.deltaTime;
            BlowSlider.value = m_MoveAcum / m_MaxMove;
            newPosition.y += m_MoveSpeed * Time.deltaTime;
            transform.position = newPosition;
            if (m_MoveAcum >= m_MaxMove)
            {
                OnDie();
            }
            else if (m_MoveAmmount >= 1f)
            {
                m_MoveAmmount -= 1f;
                GameManager.Instance.Advance();
            }

            yield return null;
        }
    }

    protected override void OnDie()
    {
        DisableInput();
        GameManager.Instance?.EndGame(m_CurrentAge);
        base.OnDie();
    }

    protected override void OnAge()
    {
        GameManager.Instance?.PlayerAged(m_CurrentAge);
        if (m_CurrentAge >= 100)
        {
            OnDie();
        }
    }

    private void EnableInput()
    {
        m_InputActions.Player.Interact.started += OnInteractStart;
        m_InputActions.Player.Interact.canceled += OnInteractCanceled;
        m_InputActions.Player.Interact.Enable();
        m_InputActions.Player.Enable();
        m_InputActions.Enable();
        BlowSlider.gameObject.SetActive(true);
    }

    private void DisableInput()
    {
        BlowSlider.gameObject.SetActive(false);
        m_InputActions.Disable();
        m_InputActions.Player.Disable();
        m_InputActions.Player.Interact.Disable();
        m_InputActions.Player.Interact.performed -= OnInteractStart;
        m_InputActions.Player.Interact.canceled -= OnInteractCanceled;
    }

    private void OnBeginPlay()
    {
        StartCoroutine(BeginRoutine());
    }

    private IEnumerator BeginRoutine()
    {
        while (Vector3.Distance(transform.position, Vector3.zero) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.zero, m_MoveSpeed * Time.deltaTime);

            yield return null;
        }

        PlayerCamera.Target.TrackingTarget = transform;
        EnableInput();
        m_Ready = true;
    }
}
