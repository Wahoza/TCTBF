//Author: Kilian Kolb
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class MoneyHud : GameObject
{
    Text MoneyText = new Text();
    Sprite icon = new();
    float iconscale = 3;
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
    }

    public override void Update(float deltaTime)
    {
        MoneyText.DisplayedString = "" + ValueParser.ParseGameState(ValueParser.linePositionGame.Money)/100;
    }
    public override void Draw(RenderWindow window)
    {
        window.Draw(icon);
        window.Draw(MoneyText);
    }
    public void SetPosition(Vector2f pos){
        icon.Position = pos;
        MoneyText.Position = pos+new Vector2f(1, 0) * (icon.Texture.Size.X * iconscale/2);
    }
}