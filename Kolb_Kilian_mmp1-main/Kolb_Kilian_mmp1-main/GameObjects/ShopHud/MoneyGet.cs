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
    public override void Initialize()
    {
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

    public override void Update(float deltaTime, InputManager inputman)
    {
        if(value >=0){
            value = (float)(int)((value-deltaTime*speed)*100)/100;
        }
    
        MoneyText.DisplayedString = "+" + value;
        
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
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
}