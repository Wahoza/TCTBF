using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class Gradiant : GameObject
{
    private Sprite? gradiant;
    float intensity = 10;
    Vector2f area;
    public override void Initialize()
    {
        Texture texture =  AssetManager.Instance.Textures["Gradiant"];
        gradiant = new Sprite(texture);

        Vector2f textureSize = new Vector2f(texture.Size.X, texture.Size.Y);
        Vector2f center = textureSize * 0.5f;
        gradiant.Origin = center;
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        if(gradiant.Position.Y < -30){
            gradiant.Scale = new Vector2f(area.X, area.X);
        }else{
            gradiant.Scale = new Vector2f(area.X,area.X) / (intensity * gradiant.Position.Y/1000);
        }
    }

    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        window.Draw(gradiant);
        DrawUtils.DrawRect(Color.Black, new FloatRect(gradiant.Position.X-(gradiant.Texture.Size.X-4)*gradiant.Scale.X/2, gradiant.Position.Y - area.Y, -area.X, area.Y*2), window);
        DrawUtils.DrawRect(Color.Black, new FloatRect(gradiant.Position.X+(gradiant.Texture.Size.X-4)*gradiant.Scale.X/2, gradiant.Position.Y - area.Y, area.X, area.Y*2), window);
        DrawUtils.DrawRect(Color.Black, new FloatRect(gradiant.Position.X - area.X, gradiant.Position.Y+(gradiant.Texture.Size.Y-4)*gradiant.Scale.Y/2, area.X*2, area.Y), window);
        DrawUtils.DrawRect(Color.Black, new FloatRect(gradiant.Position.X - area.X, gradiant.Position.Y-(gradiant.Texture.Size.Y-4)*gradiant.Scale.Y/2, area.X*2, -area.Y), window);
    }   
    public void SetPositon(Vector2f hook){
        gradiant.Position = hook;
    }
    public void SetStats(float _intensity, Vector2f _area){
        intensity = _intensity;
        area = _area;
    }

}