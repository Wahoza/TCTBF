using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class ShopItem
{
    Sprite background = new();
    Sprite icon = new();
    ShopButton button = new();
    ShopBar bar = new();
    float scale = 10;
    int InitialCost;
    ValueParser.linePositionGame ParseLocation;
    public void Initialize(string FileName, int _InitialCost, ValueParser.linePositionGame _ParseLocation)
    {
        Texture texture = AssetManager.Instance.Textures["ShopBackground"];
        background = new Sprite(texture);
        background.Origin = (Vector2f)background.Texture.Size/2;
        background.Scale *= scale;

        button = new ShopButton();
        button.Initialize();
        button.SetParseLocation(_ParseLocation);

        bar = new ShopBar();
        bar.Initialize();
        bar.SetParseLocation(_ParseLocation);

        Texture texture2 = AssetManager.Instance.Textures[FileName];
        icon = new Sprite(texture2);
        icon.Origin = (Vector2f)icon.Texture.Size/2;
        icon.Scale *= scale/2;

       InitialCost = _InitialCost;

       ParseLocation = _ParseLocation;
    }

    public void Update(float deltaTime, FloatRect MousePoint)
    {
        button.SetValue(InitialCost * ValueParser.ParseGameState(ParseLocation));
        button.SetMousePos(MousePoint);
        button.Update(deltaTime);
        bar.Update(deltaTime);
    }
    public void Draw(RenderWindow window)
    {
        window.Draw(background);
        button.Draw(window);
        window.Draw(icon);
        bar.Draw(window);
    }
    public void SetPositon(Vector2f pos){
        background.Position = pos;
        bar.SetPosition(pos);
        button.SetPosition(pos + new Vector2f(0, 1) * scale *6);
        icon.Position = pos + new Vector2f(0, -1) * scale *8;
    }
    public float GetScaleX(){
        return scale * background.Texture.Size.X;
    }
}
