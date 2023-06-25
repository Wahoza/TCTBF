using SFML.Graphics;
using SFML.Window;

public class InputManager
{
    private static InputManager instance = null!;

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputManager();
            }

            return instance;
        }
    }

    private Dictionary<Keyboard.Key, bool> pressedKeys = new Dictionary<Keyboard.Key, bool>();
    private Dictionary<Keyboard.Key, bool> keysUpThisFrame = new Dictionary<Keyboard.Key, bool>();
    private Dictionary<Keyboard.Key, bool> keysDownThisFrame = new Dictionary<Keyboard.Key, bool>();

    private Dictionary<Mouse.Button, bool> pressedMouse = new Dictionary<Mouse.Button, bool>();
    private Dictionary<Mouse.Button, bool> MouseButtonUpThisFrame = new Dictionary<Mouse.Button, bool>();
    private Dictionary<Mouse.Button, bool> MouseButtonDownThisFrame = new Dictionary<Mouse.Button, bool>();
    private InputManager() { }

    public void Initialize(RenderWindow window)
    {
        window.KeyPressed += OnKeyPressed;
        window.KeyReleased += OnKeyReleased;
        window.MouseButtonPressed += OnMousePressed;
        window.MouseButtonReleased += OnMouseReleased;

        pressedKeys[Keyboard.Key.Space] = false;
        pressedKeys[Keyboard.Key.A] = false;
        pressedKeys[Keyboard.Key.W] = false;
        pressedKeys[Keyboard.Key.D] = false;
        pressedKeys[Keyboard.Key.S] = false;

        pressedMouse[Mouse.Button.Left] = false;

        foreach (var key in pressedKeys.Keys)
        {
            keysUpThisFrame[key] = false;
            keysDownThisFrame[key] = false;
        }
        foreach (var button in pressedMouse.Keys){
            MouseButtonDownThisFrame[button] = false;
            MouseButtonUpThisFrame[button] = false;
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var keyState in keysUpThisFrame)
        {
            keysUpThisFrame[keyState.Key] = false;
        }
        foreach (var keyState in keysDownThisFrame)
        {
            keysDownThisFrame[keyState.Key] = false;
        }

        foreach (var buttonState in MouseButtonUpThisFrame)
        {
           MouseButtonUpThisFrame[buttonState.Key] = false;
        }
        foreach (var buttonState in MouseButtonDownThisFrame)
        {
           MouseButtonDownThisFrame[buttonState.Key] = false;
        }
    }

    private void OnKeyReleased(object? sender, KeyEventArgs e)
    {
        if (pressedKeys.ContainsKey(e.Code))
        {
            if (pressedKeys[e.Code])
            {
                keysUpThisFrame[e.Code] = true;
                pressedKeys[e.Code] = false;
            }
        }
    }

    private void OnKeyPressed(object? sender, KeyEventArgs e)
    {
        if (pressedKeys.ContainsKey(e.Code))
        {
            if (!pressedKeys[e.Code])
            {
                keysDownThisFrame[e.Code] = true;
                pressedKeys[e.Code] = true;
            }
        }
    }

    private void OnMouseReleased(object? sender, MouseButtonEventArgs e){
        if (pressedMouse.ContainsKey(e.Button))
        {
            if (pressedMouse[e.Button])
            {
                MouseButtonUpThisFrame[e.Button] = true;
                pressedMouse[e.Button] = false;
            }
        }
    }

    private void OnMousePressed(object? sender, MouseButtonEventArgs e){
        if (pressedMouse.ContainsKey(e.Button))
        {
            if (!pressedMouse[e.Button])
            {
                MouseButtonDownThisFrame[e.Button] = true;
                pressedMouse[e.Button] = true;
            }
        }
    }

    public bool GetKeyPressed(Keyboard.Key key)
    {
        return pressedKeys[key];
    }

    public bool GetKeyDown(Keyboard.Key key)
    {
        return keysDownThisFrame[key];
    }

    public bool GetMousePressed(Mouse.Button button){
        return pressedMouse[button];
    }
    public bool GetMouseDown(Mouse.Button button){
        return MouseButtonDownThisFrame[button];
    }
    public bool GetMouseUp(Mouse.Button button){
        return MouseButtonUpThisFrame[button];
    }
}