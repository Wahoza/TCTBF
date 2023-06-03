using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Ending : GameObject
{
    Sprite sprite;
    float animationTime = 0;
    float animationSpeed = 2;
    Vector2f area;
    Vector2f rectpos;
    public override void Initialize()
    {
        Texture texture = AssetManager.Instance.Textures["EndScreen"];
        sprite = new Sprite(texture);

        Vector2f textureSize = new Vector2f(sprite.Texture.Size.X, sprite.Texture.Size.Y);
    
        Vector2f center = textureSize * 0.5f;
        sprite.Origin = center;
    }

    public override void Update(float deltaTime, InputManager inputman)
    {

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
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        FloatRect rect = new FloatRect(rectpos.X, rectpos.Y, area.X, area.Y);
        DrawUtils.DrawRect(Color.Black, rect, window);
        window.Draw(sprite);
    }
}