using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Game
{
    private RenderWindow window;
    private List<GameObject> gameObjects = new();
    private List<Fish> FishListParse = new();
    private List<Shaders> Shaders = new();
    private float gameTime = 0;
    Player player;
    waveShader wave;
    int coll = 0;
    bool qteActive; 
    enum GameState{
        FirstLoad = 0,
        FishingPhase = 1,
        ShoppigPhase = 2,
        FinishPhase = 3
    }
    GameState gameState = new GameState();
    public Game()
    {
        VideoMode mode = new VideoMode(1900, 1080);
        string title = "Fishing Game";
        window = new RenderWindow(mode, title);

        window.Closed += OnWindowClose;
        window.Resized += OnWindowResized;

        window.SetMouseCursorVisible(false);
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

        if(!player.restart && gameState == GameState.FishingPhase){
            if(!qteActive){
            foreach (GameObject gameObject in gameObjects)
            {
                    gameObject.Update(deltaTime, InputManager.Instance);

                    if(gameObject is FishManager FishMan){
                        if(coll == 0){
                            coll = FishMan.ColisionCheck(player.CollisionBox);
                            if(coll > 0){
                                player.collided = true;
                            }
                        }
                        FishMan.SpawnFish(player.startmovement, window.GetView().Size, window.GetView().Center);
                        FishMan.MoveToGoal(player.GetPosition());
                        FishListParse = FishMan.GetFish();
                    }
                    if(gameObject is Turtle turtle){
                        turtle.SetEndPoint(player.GetHookPosition());
                        player.SetReturnPoint(turtle.GetLineStart());
                    }
                    if(gameObject is Gradiant gradiant){
                        gradiant.SetPositon(player.GetHookPosition());
                    }
                    if(gameObject is BubbleManager BubbleMan){
                        BubbleMan.SetFish(FishListParse);
                    }
                }
                foreach(Shaders shader in Shaders){
                    shader.Update(deltaTime, player.GetPosition());
                }
            }else{
                player.Update(deltaTime, InputManager.Instance);
            }
            qteActive = player.GetQTEstatus();
        }else{
            GameInitialize();
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

    private void OnWindowResized(object? sender, EventArgs e)
    {
        window.Size = new Vector2u(window.Size.X, window.Size.X);
    }
    private void Initialize(){
        InputManager.Instance.Initialize(window);
        AssetManager.Instance.Initialize();

        player = new Player();

        gameState = GameState.FirstLoad;
        int test = ValueParser.ParseGameState(0);
        GameInitialize();
    }
    private void GameInitialize()
    {
        coll = 0;
        gameObjects.Clear();

        View view = new View(new Vector2f(), (Vector2f)window.Size);
        window.SetView(view);
        window.SetFramerateLimit(120);

        WaterBackground waterBackground = new WaterBackground();
        waterBackground.Initialize();
        waterBackground.getArea(view.Size);
        gameObjects.Add(waterBackground);

        WaterwDock water = new WaterwDock();
        water.Initialize();

        gameObjects.Add(water);

        FishManager FishMan = new FishManager();
        FishMan.Initialize();

        gameObjects.Add(FishMan);
        
        Gradiant gradiant = new Gradiant();
        gradiant.Initialize();
        gradiant.SetStats(10, view.Size);

        gameObjects.Add(gradiant);

        BubbleManager bubbleManager = new BubbleManager();
        bubbleManager.Initialize();
        
        gameObjects.Add(bubbleManager);

        player.Initialize();

        gameObjects.Add(player);

        Turtle turtle = new Turtle();
        turtle.Initialize();
        turtle.SetPositon(new Vector2f(0, -40));
        gameObjects.Add(turtle);

        Hud hud = new Hud();
        hud.Initialize();
        gameObjects.Add(hud);

        FishMouse Fm = new FishMouse();
        Fm.Initialize();
        Fm.SetFishMouseArea(view.Size);
        gameObjects.Add(Fm);

        wave = new waveShader();
        wave.Initialize(window.GetView().Size);
        Shaders.Add(wave);

        qteActive = false;
    }
    
    private void StoryInitialize(){

    }
}