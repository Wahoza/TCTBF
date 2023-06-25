using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class WaterwDock : GameObject
{
    private const int WATER_TILING_X = 3;

    private Sprite water = new();
    private Sprite dock = new();
    private float animationTime = 0;
    private float animationSpeed = 5;
    private int frameCount = 3;
    public override void Initialize()
    {

        Texture waterTexture = AssetManager.Instance.Textures["Water"];
        water = new Sprite(waterTexture);
        water.TextureRect = new IntRect(
            0, 0,
            (int)(waterTexture.Size.X / WATER_TILING_X),
            (int)(waterTexture.Size.Y)
        );
        water.Scale *= (float) 5;
        water.Position = new Vector2f(0, 0);

        Texture dockTexture = AssetManager.Instance.Textures["Dock"];
        dock = new Sprite(dockTexture);
        dock.Scale *=3;
    }

    public override void Update(float deltaTime){
        animationTime += deltaTime * animationSpeed; 
        int animationFrame = (int)animationTime % frameCount;
        int waterOffsetX = animationFrame * water.TextureRect.Width;
        int waterOffsetY = 0;
        water.TextureRect = new IntRect(
            waterOffsetX,
            waterOffsetY,
            water.TextureRect.Width,
            water.TextureRect.Height
        );
    }

    public override void Draw(RenderWindow window){

        float windowlengthX = window.GetView().Size.X;
        Vector2f _position = new Vector2f((water.Scale.X*water.Texture.Size.X/(WATER_TILING_X+1)), 0);
        for(float b = 0; b <=windowlengthX + (window.GetView().Center).X/(water.Scale.X*water.Texture.Size.X/(WATER_TILING_X+1)); b++){
            water.Position = new Vector2f((windowlengthX), -30) - b* _position;
            window.Draw(water);
        }

        for(float b = -windowlengthX; b<=0; b += dock.Scale.X * dock.Texture.Size.X / 1.6f){
            dock.Position = new Vector2f(b-30, -25);
            window.Draw(dock);
        }
    }
}