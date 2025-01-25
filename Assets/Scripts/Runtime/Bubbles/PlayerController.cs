using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : BubbleComponent
{
    [SerializeField]
    private float m_MoveSpeed = 1f;
    [SerializeField]
    private float m_MaxMove = 1f;
    private InputActions m_InputActions;
    private Coroutine m_MoveRoutine;
    private float m_MoveAmmount;
    private float m_MoveAcum;

    private void Awake()
    {
        m_InputActions = new InputActions();
        Initialize();
    }

    //private void OnEnable()
    //{
    //    m_InputActions.Player.Interact.performed += OnInteract;
    //    m_InputActions.Player.Interact.Enable();
    //    m_InputActions.Player.Enable();
    //    m_InputActions.Enable();
    //}

    //private void OnDisable()
    //{
    //    m_InputActions.Disable();
    //    m_InputActions.Player.Disable();
    //    m_InputActions.Player.Interact.Disable();
    //    m_InputActions.Player.Interact.performed -= OnInteract;
    //}

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (m_MoveRoutine == null)
            {
                m_MoveRoutine = StartCoroutine(MoveRoutine());
            }
        }
        else if (context.canceled)
        {
            if (m_MoveRoutine != null)
            {
                StopCoroutine(m_MoveRoutine);
                m_MoveRoutine = null;
            }
        }
    }

    protected override void Initialize()
    {
        m_CurrentAge = -1;
    }

    private IEnumerator MoveRoutine()
    {
        Vector3 newPosition = transform.position;
        m_MoveAcum = 0f;
        while (true)
        {
            m_MoveAmmount += m_MoveSpeed * Time.deltaTime;
            m_MoveAcum += m_MoveSpeed * Time.deltaTime;
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
        base.OnDie();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
