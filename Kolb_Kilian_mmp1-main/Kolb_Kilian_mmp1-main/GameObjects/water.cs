using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Water : GameObject
{
    private const int WATER_TILING_X = 3;
    private const int WATER_TILING_Y = 1;

    private Sprite sprite;

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

        Texture waterTexture = AssetManager.Instance.Textures["water"];
        sprite = new Sprite(waterTexture);
        sprite.TextureRect = new IntRect(
            0, 0,
            (int)(waterTexture.Size.X / WATER_TILING_X),
            (int)(waterTexture.Size.Y / WATER_TILING_Y)
        );
        sprite.Scale *= (float) 5;
        sprite.Position = new Vector2f(0, 0);
    }

    public override void Update(float deltaTime, InputManager inputman){
        animationTime += deltaTime * animationSpeed; 
        int animationFrame = (int)animationTime % frameCountPerAnimation[(int)currentAnimation];
        int spriteOffsetX = animationFrame * sprite.TextureRect.Width;
        int spriteOffsetY = (int)currentAnimation * sprite.TextureRect.Height;   
        sprite.TextureRect = new IntRect(
            spriteOffsetX,
            spriteOffsetY,
            sprite.TextureRect.Width,
            sprite.TextureRect.Height
        );
    }

    public override void Draw(RenderWindow window, RenderStates states, bool shaderon){
        //(window.GetView().Center).X
        float windowlengthX = window.GetView().Size.X;
        Vector2f _position = new Vector2f((sprite.Scale.X*sprite.Texture.Size.X/(WATER_TILING_X+1)), 0);
        for(float b = 0; b <=windowlengthX + (window.GetView().Center).X/(sprite.Scale.X*sprite.Texture.Size.X/(WATER_TILING_X+1)); b++){
            sprite.Position = new Vector2f((windowlengthX), -30) - b* _position;
            window.Draw(sprite);
        } 
        
    }
}