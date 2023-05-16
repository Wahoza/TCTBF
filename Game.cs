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
                FishMan.SpawnFish(player.startmovement, window.GetView().Size, window.GetView().Center);
                //int coll = FishMan.ColisionCheck(player.CollisionBox);
                //if(coll > 0){
                //    player.collided = true;
                //}
                FishMan.MoveToGoal(player.GetPosition());
            }
        }
        //waveEffect.Update(deltaTime);

        InputManager.Instance.Update(deltaTime);
    }

    private void Draw()
    {
        window.Clear(Color.Blue);
        DrawFloor(new Vector2f(-200, -200), new Vector2i(10, 10), new Vector2i(50, 50));

        //foreach (Shaders shader in Shaders)
        //{
        //    shader.Draw(window, player.Position);
        //}

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.Draw(window);
        }
        //waveEffect.Draw(window, RenderStates.Default);

        window.Display();
    }
        private void DrawRectangle(Vector2f position, int width, int height, Color color){
        RectangleShape rect = new RectangleShape(new Vector2f(width, height));
        rect.FillColor = color;
        rect.Position = position;
        window.Draw(rect);
    }
    private void DrawFloor(Vector2f position, Vector2i tiles, Vector2i tileSize){
        Color fill = new Color();
        for(int y = 0; y < tiles.Y; y++){
            for(int x = 0; x < tiles.X; x++){
                if((y + x) % 2 == 0){
                    fill = Color.Black;
                }else{
                    fill = Color.White;
                }

                DrawRectangle(
                    (position + new Vector2f(x * tileSize.X, y* tileSize.Y)),
                    tileSize.X, tileSize.Y, fill);
            }
        }
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

        gameObjects.Add(turtle);

        Water water = new Water();
        water.Initialize();

        gameObjects.Add(water);

        FishManager FishMan = new FishManager();
        FishMan.Initialize();

        gameObjects.Add(FishMan);
        
        hueShader hue = new hueShader();
        Shaders.Add(hue);
    }
}