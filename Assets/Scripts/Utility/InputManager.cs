using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private Vector2 movementInput;
    private bool jumpInput;
    private bool pauseInput;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        // Process inputs
        ProcessMovementInput();
        ProcessJumpInput();
        ProcessPauseInput();
    }

    private void ProcessMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementInput = new Vector2(horizontal, vertical);
    }

    private void ProcessJumpInput()
    {
        jumpInput = Input.GetButtonDown("Jump");
    }

    private void ProcessPauseInput()
    {
        pauseInput = Input.GetKeyDown(KeyCode.Escape);
    }
    
    private void ProcessTrickAction()
    {
        // TODO: implement triggering
        EventManager.Broadcast(new PlayerEvents.TrickActionEventR());
    }

    private void ProcessSpecialAction()
    {
        // implement triggering
        EventManager.Broadcast(new PlayerEvents.SpecialActionEventR());
    }

    public Vector2 GetMovementInput()
    {
        return movementInput;
    }

    public bool GetJumpInput()
    {
        return jumpInput;
    }

    public bool GetPauseInput()
    {
        return pauseInput;
    }
}