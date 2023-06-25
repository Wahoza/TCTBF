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
    private float gameTime = 0;
    Player player = new();
    FishMouse fishMouse = new();
    bool qteActive;

    Music AboveWaterMusic = null!;
    Music UnderWaterMusic = null!;
    Music VictoryMusic = null!;

    //Compares new music state[0] to old music state [1], to avoid repeating music.Play(). [2]: currently active music
    int[] MusicState = new int[3]{0, 0, 0};
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

        window = new RenderWindow(mode, title, Styles.Fullscreen);

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

    public void Run(){
        Initialize();
        Clock clock = new Clock();
        while(window.IsOpen){
            if(gameState == GameState.FirstLoad){
                HandleEvents();

                float deltaTime = clock.Restart().AsSeconds();
                OpeningUpdate(deltaTime);

                Draw();
            }else{
                RunGame();
            }
        }
    }
    public void RunGame()
    {
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

        if(gameState == GameState.FishingPhase){
            GameUpdate(deltaTime);
        }

        if(gameState == GameState.ShoppigPhase){
            ShopUpdate(deltaTime);
        }
        if (gameState == GameState.FinishPhase){
            EndingUpdate(deltaTime);
        }

        HandleMusic();

        InputManager.Instance.Update(deltaTime);
    }

    private void Draw()
    {
        window.Clear(Color.Blue);

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.Draw(window);
        }

        window.Display();
    }
    private void OpeningUpdate(float deltaTime){
        foreach(GameObject gameObject in gameObjects){

            gameObject.Update(deltaTime);

            if (gameObject is Opening op && op.GetFinished())
            {
                gameState = GameState.ShoppigPhase;
                ValueParser.WriteGamestate(0, (int)gameState);
                ShopInitialize();
                break;
            }
        }
    }
    
    private void GameUpdate(float deltaTime){
        if(!qteActive){
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(deltaTime);
                    switch(gameObject){

                        case FishManager fm:
                            if (!player.collided && fm.CollisionTest(player.CollisionBox) != null)
                            {
                                player.collided = true;
                                player.SetCoughtFish(fm.CollisionTest(player.CollisionBox));
                            }

                            fm.SpawnFish(player.started, window.GetView().Size, window.GetView().Center);
                            fm.MoveToGoal(player.GetPosition());
                            FishListParse = fm.GetFish();
                        break;

                        case Turtle turtle:
                            turtle.SetEndPoint(player.GetHookPosition());
                        break;

                        case Gradiant gradiant:
                            gradiant.SetPositon(player.GetHookPosition());
                        break;

                        case BubbleManager bubbleManager:
                            bubbleManager.SetFish(FishListParse);
                        break;

                        case Hud hud:
                            hud.SetTimer(player.GetMoveTimer());
                        break;
                    }
                }
            }else{
                player.Update(deltaTime);
                foreach(GameObject o in gameObjects){
                    if(o is Turtle t){
                        t.Update(deltaTime);
                    }
                }
            }

            qteActive = player.GetQTEstatus();

            if(player.restart){
                if(ValueParser.ParseGameState(ValueParser.linePositionGame.MoneyRecieved)>10000000){
                    gameState = GameState.FinishPhase;
                    EndInitialize();

                }else{
                    gameState = GameState.ShoppigPhase;
                    ShopInitialize();
                }
            }
    }
    private void ShopUpdate(float deltaTime){
        foreach(GameObject gameObject in gameObjects){
            if(gameObject is Vendor shop){
                shop.getMousePosition(fishMouse.GetMouseRect());
            }
            gameObject.Update(deltaTime);
        }

        if(InputManager.Instance.GetKeyDown(Keyboard.Key.Space)){
            gameState = GameState.FishingPhase;
            GameInitialize();
        }
    }
    private void EndingUpdate(float deltaTime){
        foreach(GameObject o in gameObjects){
            o.Update(deltaTime);
            if(o is Ending end){
                end.getMousePosition(fishMouse.GetMouseRect());
            }
        }
    }

    private void HandleMusic(){
        if(gameState != GameState.FinishPhase){
        MusicState[1] = MusicState[0];
        MusicState[0] = ValueParser.ParseSettings(0);

        float Height = window.GetView().Center.Y;

        if(Height > 30 && MusicState[2] == 0){
            MusicState[2] = 1;
            if(MusicState[0] == 1){
                AboveWaterMusic.Stop();

                UnderWaterMusic.PlayingOffset = AboveWaterMusic.PlayingOffset;
                UnderWaterMusic.Play();
                UnderWaterMusic.Loop = true;
            }

        }else if (Height <= 30 && MusicState[2] == 1){
            MusicState[2] = 0;

            if(MusicState[0] == 1){
                UnderWaterMusic.Pause();
                AboveWaterMusic.PlayingOffset = UnderWaterMusic.PlayingOffset;
                AboveWaterMusic.Play();
                AboveWaterMusic.Loop = true;
            }
        }

        if(MusicState[0] != MusicState[1]){
            if(MusicState[0] == 1){
                AboveWaterMusic.Play();
                AboveWaterMusic.Loop = true;
            }else{
                AboveWaterMusic.Pause();
                UnderWaterMusic.Pause();
            }
        }

        if(Math.Sqrt(Math.Abs(Height-29))/30 >1){
            UnderWaterMusic.Volume = (int)(100 / (Math.Sqrt(Math.Abs(Height-29)) /30));
        }

        }else{
            if(MusicState[2] != 2 && ValueParser.ParseSettings(0) == 1 ){
                MusicState[2] = 2;
                VictoryMusic.Play();
            }
        }
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

        AboveWaterMusic = AssetManager.Instance.Musics["AboveWater"];
        UnderWaterMusic = AssetManager.Instance.Musics["UnderWater"];
        VictoryMusic = AssetManager.Instance.Musics["Victory"];

        AboveWaterMusic.Loop = true;
        UnderWaterMusic.Loop = true;

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
        player.SetReturnPoint(turtle.GetLineStart());
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

        AboveWaterMusic.Stop();
        UnderWaterMusic.Stop();

        View view = new View(new Vector2f(), (Vector2f)window.Size);
        window.SetView(view);
        window.SetFramerateLimit(120);

        Ending end = new ();
        end.Initialize();
        end.SetArea(view.Size);
        gameObjects.Add(end);

        fishMouse.Initialize();
        fishMouse.SetFishMouseArea(view.Size);
        gameObjects.Add(fishMouse);
    }
}