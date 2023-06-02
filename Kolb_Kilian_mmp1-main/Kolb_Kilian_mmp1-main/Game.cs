using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Game
{
    private RenderWindow window;
    private List<GameObject> gameObjects = new();
    private List<GameObject> shopObjects = new();
    private List<Fish> FishListParse = new();
    private List<Shaders> Shaders = new();
    private float gameTime = 0;
    Player player;
    FishMouse fishMouse;
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
        VideoMode mode = new VideoMode(1920, 1080);
        string title = "Fishing Game";
        window = new RenderWindow(mode, title, Styles.None);

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
        if(gameState == GameState.FirstLoad){
            foreach(GameObject gameObject in gameObjects){

                gameObject.Update(deltaTime, InputManager.Instance);
                
                if(gameObject is Opening op){

                    if(op.GetFinished()){

                        gameState = GameState.FishingPhase;
                        ValueParser.WriteGamestate(0, (int)gameState);
                        GameInitialize();
                        break;
                    }
                }
            }
        }


        else if(!player.restart && gameState == GameState.FishingPhase){
            if(!qteActive){
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(deltaTime, InputManager.Instance);

                    if(gameObject is FishManager FishMan){
                        if(!player.collided){
                            if(FishMan.CollisionTest(player.CollisionBox) != null){
                                player.collided = true;
                                player.SetCoughtFish(FishMan.CollisionTest(player.CollisionBox));
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
                    if(gameObject is Hud hud){
                        hud.SetTimer(player.GetMoveTimer());
                    }
                }
                foreach(Shaders shader in Shaders){
                    shader.Update(deltaTime, player.GetPosition());
                }
            }else{
                player.Update(deltaTime, InputManager.Instance);
                foreach(GameObject o in gameObjects){
                    if(o is Turtle t){
                        t.Update(deltaTime, InputManager.Instance);
                    }
                }
            }

            qteActive = player.GetQTEstatus();
            
        }else{
    
            if(ValueParser.ParseGameState(1)>10000000){
                gameState = GameState.FinishPhase;
                EndInitialize();
            }else if(gameState != GameState.ShoppigPhase){
                gameState = GameState.ShoppigPhase;
                ShopInitialize();
            }
        }
        if(gameState == GameState.ShoppigPhase && InputManager.Instance.GetKeyDown(Keyboard.Key.Space)){
            gameState = GameState.FishingPhase;
            GameInitialize();

        }else if(gameState == GameState.ShoppigPhase){
                foreach(GameObject gameObject in shopObjects){
                    if(gameObject is Vendor shop){
                        shop.getMousePosition(fishMouse.GetMouseRect());
                    }
                    gameObject.Update(deltaTime, InputManager.Instance);
                }
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
        fishMouse = new FishMouse();
        wave = new waveShader();
        wave.Initialize(window.GetView().Size);
        Shaders.Add(wave);

        gameState = (GameState)ValueParser.ParseGameState(0);
        if(gameState == GameState.FirstLoad){
            FirstInitialize();
        }else if(gameState == GameState.FishingPhase){
            GameInitialize();
        }else if(gameState == GameState.ShoppigPhase){
            ShopInitialize();
        }
    }
    private void GameInitialize()
    {
        coll = 0;
        shopObjects.Clear();
        gameObjects.Clear();

        View view = new View(new Vector2f(0, 0), (Vector2f)window.Size);
        window.SetFramerateLimit(120);
        window.SetView(view);

        WaterBackground waterBackground = new WaterBackground();
        waterBackground.Initialize();
        waterBackground.SetArea(view.Size);
        gameObjects.Add(waterBackground);

        Vendor vendor = new Vendor();
        vendor.Initialize();
        vendor.setShopingPhase(false);
        gameObjects.Add(vendor);

        WaterwDock water = new WaterwDock();
        water.Initialize();

        gameObjects.Add(water);

        FishManager FishMan = new FishManager();
        FishMan.Initialize();

        gameObjects.Add(FishMan);
        
        Gradiant gradiant = new Gradiant();
        gradiant.Initialize();
        gradiant.SetStats(view.Size);

        gameObjects.Add(gradiant);

        BubbleManager bubbleManager = new BubbleManager();
        bubbleManager.Initialize();
        
        gameObjects.Add(bubbleManager);

        player.Initialize();

        gameObjects.Add(player);

        Turtle turtle = new Turtle();
        turtle.Initialize();
        turtle.SetPositon(new Vector2f(0, -40));
        turtle.SetEndPoint(player.GetHookPosition());
        gameObjects.Add(turtle);

        Hud hud = new Hud();
        hud.Initialize();
        gameObjects.Add(hud);

        qteActive = false;

    }

    private void ShopInitialize(){
        
        gameObjects.Clear();
        
        View view = new View(new Vector2f(), (Vector2f)window.Size);
        window.SetFramerateLimit(120);
        window.SetView(view);

        WaterBackground waterBackground = new WaterBackground();
        waterBackground.Initialize();
        waterBackground.SetArea(view.Size);
        gameObjects.Add(waterBackground);

        Vendor vendor = new Vendor();
        vendor.Initialize();
        vendor.setShopingPhase(true);
        gameObjects.Add(vendor);

        WaterwDock water = new WaterwDock();
        water.Initialize();
        gameObjects.Add(water);

        player.Initialize();
        gameObjects.Add(player);
        
        Turtle turtle = new Turtle();
        turtle.Initialize();
        turtle.SetPositon(new Vector2f(0, -40));
        turtle.SetEndPoint(player.GetHookPosition());
        gameObjects.Add(turtle);
        
        MoneyGet moneyGet = new MoneyGet();
        moneyGet.Initialize();
        moneyGet.SetPosition((player.GetHookPosition() + new Vector2f(100 , 0)));
        gameObjects.Add(moneyGet);

        fishMouse.Initialize();
        fishMouse.SetFishMouseArea(view.Size);
        gameObjects.Add(fishMouse);
    }
    private void FirstInitialize(){
        View view = new View(new Vector2f(), (Vector2f)window.Size);
        window.SetView(view);
        window.SetFramerateLimit(120);

        Opening op = new Opening();
        op.Initialize();
        op.SetArea(view.Size);

        gameObjects.Add(op);
    }

    private void EndInitialize(){
        gameObjects.Clear();

        View view = new View(new Vector2f(), (Vector2f)window.Size);
        window.SetView(view);
        window.SetFramerateLimit(120);

        Ending end = new ();
        end.Initialize();
        end.SetArea(view.Size);

        gameObjects.Add(end);
    }
    

}