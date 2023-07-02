//Author: Kilian Kolb
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Ending : GameObject
{
    Sprite sprite = new();
    Vector2f area;
    Vector2f rectpos;
    HudButton ExitButton = new();
    FloatRect mouse;
    public override void Initialize()
    {
        Texture texture = AssetManager.Instance.Textures["EndScreen"];
        sprite = new Sprite(texture);

        Vector2f textureSize = new Vector2f(sprite.Texture.Size.X, sprite.Texture.Size.Y);
    
        Vector2f center = textureSize * 0.5f;
        sprite.Origin = center;

        ExitButton.Initialize("CloseButton", 3 , new Vector2f(-900, -500));
    }

    public override void Update(float deltaTime)
    {
        ExitButton.Update(mouse);
    }
    public void SetArea(Vector2f _area){
        area = _area;
        if(area.X < area.Y){
            sprite.Scale =new Vector2f(area.X, area.X)/sprite.Texture.Size.Y;
        }else{
            sprite.Scale =new Vector2f(area.Y, area.Y)/sprite.Texture.Size.Y;
        }
        rectpos = -area/2;
    }
    public void getMousePosition(FloatRect pos){
        mouse = pos;
    }
    public override void Draw(RenderWindow window)
    {
        FloatRect rect = new FloatRect(rectpos.X, rectpos.Y, area.X, area.Y);
        DrawUtils.DrawRect(Color.Black, rect, window);
        window.Draw(sprite);
        ExitButton.Draw(window);
        if(ExitButton.Clicked()){
                window.Close();
        }
    }
}