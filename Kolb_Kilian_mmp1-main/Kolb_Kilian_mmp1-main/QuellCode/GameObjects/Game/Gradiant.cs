//Author: Kilian Kolb
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class Gradiant : GameObject
{
    private Sprite gradiant = new();
    float intensity = 100;
    Vector2f area;
    float InitIntensity = 75;
    public override void Initialize()
    {
        Texture texture =  AssetManager.Instance.Textures["Gradiant"];
        gradiant = new Sprite(texture);

        Vector2f textureSize = new Vector2f(texture.Size.X, texture.Size.Y);
        Vector2f center = textureSize * 0.5f;
        gradiant.Origin = center;
        gradiant.Position = new Vector2f (0, 0);
    }

    public override void Update(float deltaTime)
    {
        gradiant.Scale = (gradiant.Position.Y < -30)
            ? new Vector2f(area.X, area.X)
            : new Vector2f(area.X, area.X) / (intensity * gradiant.Position.Y / 2000);
    }

    public override void Draw(RenderWindow window)
    {
        window.Draw(gradiant);
        if(gradiant.Position !=new Vector2f (0, 0)){
            DrawUtils.DrawRect(Color.Black, new FloatRect(gradiant.Position.X-(gradiant.Texture.Size.X-4)*gradiant.Scale.X/2, gradiant.Position.Y - area.Y, -area.X, area.Y*2), window);
            DrawUtils.DrawRect(Color.Black, new FloatRect(gradiant.Position.X+(gradiant.Texture.Size.X-4)*gradiant.Scale.X/2, gradiant.Position.Y - area.Y, area.X, area.Y*2), window);
            DrawUtils.DrawRect(Color.Black, new FloatRect(gradiant.Position.X - area.X, gradiant.Position.Y+(gradiant.Texture.Size.Y-4)*gradiant.Scale.Y/2, area.X*2, area.Y), window);
            DrawUtils.DrawRect(Color.Black, new FloatRect(gradiant.Position.X - area.X, gradiant.Position.Y-(gradiant.Texture.Size.Y-4)*gradiant.Scale.Y/2, area.X*2, -area.Y), window);
        }
    }   
    public void SetPositon(Vector2f hook){
        gradiant.Position = hook;
    }
    public void SetStats(Vector2f _area){
        if(ValueParser.ParseGameState(ValueParser.linePositionGame.Vision) != 0){
            intensity = InitIntensity/ValueParser.ParseGameState(ValueParser.linePositionGame.Vision);
            area = _area;
        }else{
            intensity = InitIntensity;
        }

    }

}