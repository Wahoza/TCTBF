using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class WaterwDock : GameObject
{
    private const int WATER_TILING_X = 3;
    private const int WATER_TILING_Y = 1;

    private Sprite water;
    private Sprite dock;
    private float animationTime = 0;
    private float animationSpeed = 5;
    private int[] frameCountPerAnimation = new int[1];
    private AnimationType currentAnimation = AnimationType.Flow;
    public enum AnimationType
{
    Flow = 0
}
    bool startmovement = false;
    private float DeltaTime;
    public override void Initialize()
    {
        frameCountPerAnimation[0] = 3;

        Texture waterTexture = AssetManager.Instance.Textures["Water"];
        water = new Sprite(waterTexture);
        water.TextureRect = new IntRect(
            0, 0,
            (int)(waterTexture.Size.X / WATER_TILING_X),
            (int)(waterTexture.Size.Y / WATER_TILING_Y)
        );
        water.Scale *= (float) 5;
        water.Position = new Vector2f(0, 0);

        Texture dockTexture = AssetManager.Instance.Textures["Dock"];
        dock = new Sprite(dockTexture);
        dock.Scale *=3;
    }

    public override void Update(float deltaTime, InputManager inputman){
        animationTime += deltaTime * animationSpeed; 
        int animationFrame = (int)animationTime % frameCountPerAnimation[(int)currentAnimation];
        int waterOffsetX = animationFrame * water.TextureRect.Width;
        int waterOffsetY = (int)currentAnimation * water.TextureRect.Height;   
        water.TextureRect = new IntRect(
            waterOffsetX,
            waterOffsetY,
            water.TextureRect.Width,
            water.TextureRect.Height
        );
    }

    public override void Draw(RenderWindow window, RenderStates states, bool shaderon){

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