using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

public class AssetManager
{
    private static AssetManager instance;

    public static AssetManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AssetManager();
            }

            return instance;
        }
    }

    public void Initialize(){
        LoadTexture("hookwbait", "./Assets/FishSprites/hookwbait.png");
        LoadTexture("lineEnd", "./Assets/Other/line-end.png");
        LoadTexture("turtle", "./Assets/Other/startFish.png");

        LoadTexture("Dock", "./Assets/Other/Dock.png");
        LoadTexture("Water", "./Assets/Other/wave.png");

        LoadTexture("fish", "./Assets/FishSprites/Fish.png");
        LoadTexture("DriftFish", "./Assets/FishSprites/Soap.png");
        LoadTexture("NormalFish", "./Assets/FishSprites/Orgle.png");
        LoadTexture("SquidFish", "./Assets/FishSprites/Squid.png");
        LoadTexture("BulletFish", "./Assets/FishSprites/Bullet.png");
        LoadTexture("BiggestFish", "./Assets/FishSprites/BigFish.png");

        LoadTexture("Gradiant","./Assets/Other/gradient.png");
        LoadTexture("Bubbles","./Assets/FishSprites/Bubble.png");

        LoadTexture("Mouse", "./Assets/HudSprites/Mouse.png");

        LoadFont("PixelFont", "./Assets/Fonts/Pixeled.ttf");
    }

    public Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();
    public Dictionary<string, SoundBuffer> SoundBuffers = new Dictionary<string, SoundBuffer>();
    public Dictionary<string, Music> Musics = new Dictionary<string, Music>();
    public Dictionary<string, Font> Fonts = new Dictionary<string, Font>();

    public void LoadTexture(string name, string fileName){
        Textures.Add(name, new Texture(fileName));
    }
    public void LoadSoundBuffer(string name, string fileName){
        SoundBuffers.Add(name, new SoundBuffer(fileName));
    }
    public void LoadMusic(string name, string fileName){
        Musics.Add(name, new Music(fileName));
    }
    public void LoadFont(string name, string fileName){
        Fonts.Add(name, new Font(fileName));
    }
}