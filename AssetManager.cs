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
        LoadTexture("hookwbait", "./Assets/hookwbait.png");
        //LoadMusic("track1", "./Assets/musicTrack.ogg");
        //LoadSoundBuffer("sound1", "./Assets/completeSound.wav");
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