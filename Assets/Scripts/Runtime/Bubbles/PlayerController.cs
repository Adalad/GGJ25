using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BubbleComponent
{
    [SerializeField]
    private float m_MoveSpeed = 1f;
    private InputActions m_InputActions;
    private Coroutine m_MoveRoutine;

    private void Awake()
    {
        m_InputActions = new InputActions();
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
    }

    protected override void Initialize()
    {
        m_CurrentAge = 0;
    }

    private IEnumerator MoveRoutine()
    {
        Vector3 target = transform.position + Vector3.up;
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, m_MoveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = target;
        GameManager.Instance.Advance();
        m_MoveRoutine = null;
    }
}
