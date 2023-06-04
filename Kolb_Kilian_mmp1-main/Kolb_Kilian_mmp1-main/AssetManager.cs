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
        LoadTexture("shop", "./Assets/HudSprites/Shop.png");

        LoadTexture("Dock", "./Assets/Other/Dock.png");
        LoadTexture("Water", "./Assets/Other/wave.png");
        LoadTexture("Background", "./Assets/Other/Background.png");

        LoadTexture("fish", "./Assets/FishSprites/Fish.png");
        LoadTexture("DriftFish", "./Assets/FishSprites/Soap.png");
        LoadTexture("NormalFish", "./Assets/FishSprites/Orgle.png");
        LoadTexture("SquidFish", "./Assets/FishSprites/Squid.png");
        LoadTexture("BulletFish", "./Assets/FishSprites/Bullet.png");
        LoadTexture("BiggestFish", "./Assets/FishSprites/BigFish.png");
        LoadTexture("SubFish", "./Assets/FishSprites/Fish.png");

        LoadTexture("Gradiant","./Assets/Other/gradient.png");
        LoadTexture("Bubbles","./Assets/FishSprites/Bubble.png");
        LoadTexture("Splash","./Assets/Other/Splash.png");
        LoadTexture("MoveEnabled", "./Assets/HudSprites/MoveDuration.png");

        LoadTexture("Mouse", "./Assets/HudSprites/Mouse.png");
        LoadTexture("Button", "./Assets/HudSprites/Button.png");
        LoadTexture("ShopBackground", "./Assets/HudSprites/ShopBox.png");
        LoadTexture("Money", "./Assets/HudSprites/Money.png");
        LoadTexture("UpgradeBar", "./Assets/HudSprites/UpgradeBar.png");
        LoadTexture("UpgradeBait", "./Assets/HudSprites/UpgradeBait.png");
        LoadTexture("UpgradeVision", "./Assets/HudSprites/UpgradeVision.png");
        LoadTexture("UpgradeWeight", "./Assets/HudSprites/UpgradeWeight.png");
        LoadTexture("UpgradeMove", "./Assets/HudSprites/UpgradeMove.png");
        LoadTexture("Information", "./Assets/HudSprites/information.png");

        LoadTexture("ShopButton", "./Assets/HudSprites/Shopbutton.png");
        LoadTexture("InfoButton", "./Assets/HudSprites/Informationbutton.png");
        LoadTexture("CloseButton", "./Assets/HudSprites/exit.png");
        LoadTexture("MusicButton", "./Assets/HudSprites/Music.png");
        LoadTexture("SoundButton", "./Assets/HudSprites/Sound.png");

        LoadTexture("InitialAnim", "./Assets/Other/InitAnim.png");
        LoadTexture("EndScreen", "./Assets/Other/Victory.png");

        LoadSoundBuffer("Bite", "./Assets/Sound/bite.wav");
        LoadSoundBuffer("Click", "./Assets/Sound/click.wav");
        LoadSoundBuffer("Splash", "./Assets/Sound/hookinwater.wav");
        LoadSoundBuffer("Coin", "./Assets/Sound/coin.wav");
        LoadSoundBuffer("CastLine", "./Assets/Sound/fish.wav");


        LoadMusic("AboveWater", "./Assets/Music/AboveWater.ogg");
        LoadMusic("UnderWater", "./Assets/Music/UnderWater.ogg");
        LoadMusic("Victory", "./Assets/Music/Victory.ogg");
        
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