using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private Vector2 movementInput;
    private bool jumpInput;
    private bool pauseInput;

    public event Action<Vector2> OnMovementInput;
    public event Action OnJumpInput;
    public event Action OnEscapeButtonPressed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        ProcessMovementInput();
        ProcessJumpInput();
        ProcessPauseInput();
    }

    private void ProcessMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementInput = new Vector2(horizontal, vertical);

        OnMovementInput?.Invoke(movementInput);
    }

    private void ProcessJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
            OnJumpInput?.Invoke();
        }
        else
        {
            jumpInput = false;
        }
    }

    private void ProcessPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseInput = true;
            OnEscapeButtonPressed?.Invoke();
        }
    }

    public Vector2 GetMovementInput() => movementInput;
    public bool GetJumpInput() => jumpInput;
    public bool GetPauseInput() => pauseInput;
}