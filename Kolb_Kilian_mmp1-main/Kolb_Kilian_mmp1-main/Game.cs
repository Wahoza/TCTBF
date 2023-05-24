using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Game
{
    private RenderWindow window;
    private List<GameObject> gameObjects = new();
    private List<Shaders> Shaders = new();
    private float gameTime = 0;
    Player player;
    waveShader wave;
    int coll = 0; 
    public Game()
    {
        VideoMode mode = new VideoMode(1000, 1000);
        string title = "Fishing Game";
        window = new RenderWindow(mode, title);

        window.Closed += OnWindowClose;
        window.Resized += OnWindowResized;
    }

    private void OnWindowResized(object? sender, SizeEventArgs e)
    {
        window.SetView(new View(window.GetView().Center, (Vector2f)window.Size));
    }

    private void OnWindowClose(object? sender, EventArgs e)
    {
        window.Close();
    }

    public void Run()
    {
        Initialize();

        Clock clock = new Clock();

        while (window.IsOpen)
        {
            float deltaTime = clock.Restart().AsSeconds();

            HandleEvents();

            Update(deltaTime);

            Draw();
        }
    }

    private void Update(float deltaTime)
    {
        gameTime += deltaTime;

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.Update(deltaTime, InputManager.Instance);

            if(gameObject is FishManager FishMan){
                //if(coll == 0){
                //    coll = FishMan.ColisionCheck(player.CollisionBox);
                //    if(coll > 0){
                //        //player.collided = true;
                //    }
                //}
                FishMan.SpawnFish(player.startmovement, window.GetView().Size, window.GetView().Center);
                FishMan.MoveToGoal(player.GetPosition());
            }
            if(gameObject is Turtle turtle){
                turtle.SetEndPoint(player.GetHookPosition());
            }
        }
        foreach(Shaders shader in Shaders){
            shader.Update(deltaTime, player.GetPosition());
        }

        InputManager.Instance.Update(deltaTime);
    }

    private void Draw()
    {
        window.Clear(Color.Blue);
        RenderStates states = wave.GetRenderStates();
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.Draw(window, states, false);
        }
        //waveEffect.Draw(window, RenderStates.Default);

        window.Display();
    }

    private void HandleEvents()
    {
        window.DispatchEvents();
    }

    private void Initialize()
    {
        InputManager.Instance.Initialize(window);
        AssetManager.Instance.Initialize();

        View view = new View(new Vector2f(), (Vector2f)window.Size);
        window.SetView(view);
        window.SetFramerateLimit(120);

        player = new Player();
        player.Initialize();

        gameObjects.Add(player);

        Turtle turtle = new Turtle();
        turtle.Initialize();
        turtle.SetPositon(new Vector2f(0, -40));

        gameObjects.Add(turtle);

        Water water = new Water();
        water.Initialize();

        gameObjects.Add(water);

        FishManager FishMan = new FishManager();
        FishMan.Initialize();

        gameObjects.Add(FishMan);
        
        wave = new waveShader();
        wave.Initialize(window.GetView().Size);
        Shaders.Add(wave);
    }
    
}