using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Turtle : GameObject
{
    private const int TURTLE_TILING_X = 9;
    private const int TURTLE_TILING_Y = 1;

    private Sprite sprite;

    private float animationTime = 0;
    private float animationSpeed = 10;
    private int[] frameCountPerAnimation = new int[1];
    private AnimationType currentAnimation = AnimationType.Throw;
    bool IsFishing = false;
    bool initAnim = false;
    public enum AnimationType
{
    Throw = 0
}
    bool startmovement = false;
    private float DeltaTime;
    public override void Initialize(AssetManager assetManager)
    {
        frameCountPerAnimation[0] = 9;

        Texture turtleTexture = assetManager.Textures["turtle"];
        sprite = new Sprite(turtleTexture);
        sprite.TextureRect = new IntRect(
            0, 0,
            (int)(turtleTexture.Size.X / TURTLE_TILING_X),
            (int)(turtleTexture.Size.Y / TURTLE_TILING_Y)
        );
        sprite.Scale *= (float) 3;
        sprite.Position = new Vector2f(0, -40);
    }

    public override void Update(float deltaTime, InputManager inputman){
        if(Throw(inputman) || initAnim){
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
            if(animationFrame == frameCountPerAnimation[(int)currentAnimation]-1){
                initAnim = false;
            }
        }
    }
    public bool Throw(InputManager inputman){
        if(!IsFishing && inputman.GetKeyDown(Keyboard.Key.Space)){
            IsFishing = false;
            initAnim = true;
        }
        return IsFishing;
    }

    //public void UpdateLine(Vector2f Linegoal){
    //    if(initAnim){
//
    //    }else{
//
    //    }
    //}

    public override void Draw(RenderWindow window){
        window.Draw(sprite);
    }
}