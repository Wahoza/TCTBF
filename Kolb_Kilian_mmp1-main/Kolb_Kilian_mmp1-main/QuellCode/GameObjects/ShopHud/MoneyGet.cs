using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class MoneyGet : GameObject
{
    Text MoneyText = new Text();
    Sprite icon;
    float iconscale = 3;
    float TTL;
    float value;
    float speed;
    float waittime = 0.5f;

    bool playSound = true;
    float soundPlayTime = 0.5f;
    float spaceoutTime = 0.05f;
    float spaceoutElapsed = 0;
    public override void Initialize()
    {
        if (ValueParser.ParseSettings(1) == 0){
            playSound = false;
        }

        MoneyText = new Text();
        Font font = AssetManager.Instance.Fonts["PixelFont"];
        MoneyText.Font = font;
        MoneyText.CharacterSize = 24;
        MoneyText.OutlineColor = Color.Black;
        MoneyText.OutlineThickness =3;
        MoneyText.FillColor = Color.White;

        Texture texture = AssetManager.Instance.Textures["Money"];
        icon = new Sprite(texture);
        icon.Origin = (Vector2f)texture.Size/2;
        icon.Scale *= iconscale;

        value = ValueParser.ParseGameState(2);
        ValueParser.WriteGamestate(1, ValueParser.ParseGameState(1) + (int)value);
        ValueParser.WriteGamestate(2, 0);
        value /= 100;
        speed = value;
    }

    public override void Update(float deltaTime)
    {
        if(waittime < 0){
            if(value > 0){
                value = (float)(int)((value-deltaTime*speed)*100)/100;
                if(soundPlayTime > 0){
                    if(spaceoutElapsed >= spaceoutTime){
                        PlaySound();
                        spaceoutElapsed = 0;
                    }else{
                        spaceoutElapsed += deltaTime;
                    }
                    
                    soundPlayTime -= deltaTime;
                }
            }
        }else{
            waittime -= deltaTime;
        }
        
    
        MoneyText.DisplayedString = "+" + value;
        
    }
    public override void Draw(RenderWindow window)
    {
        if(value >0){
            window.Draw(icon);
            window.Draw(MoneyText);
        }
    }
    public void SetPosition(Vector2f pos){
        MoneyText.Position = pos;

        icon.Position = pos + new Vector2f(MoneyText.GetLocalBounds().Width, 0);
    }
    private void PlaySound(){
        if(playSound){
            SoundBuffer buffer = AssetManager.Instance.SoundBuffers["Coin"];
            Sound sound = new Sound(buffer);
            sound.Play();
        }
    }
}