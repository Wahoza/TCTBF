using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Bubble : GameObject
{
    private Sprite sprite;
    Vector2f textureSize;
    float BUBBLE_TILING_X = 4;
    float BUBBLE_TILING_Y = 1;

    Vector2f movevec;
    float speed = 20;

    float TimeToLive = 1;
    float animationTime = 0;
    float animationSpeed = 9;
    public override void Initialize()
    {
        Texture texture =  AssetManager.Instance.Textures["Bubbles"];
        sprite = new Sprite(texture);
        textureSize = (Vector2f)texture.Size;
            sprite.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / BUBBLE_TILING_X),
            (int)(texture.Size.Y / BUBBLE_TILING_Y)
        );
        textureSize = new Vector2f(sprite.Texture.Size.X/BUBBLE_TILING_X, sprite.Texture.Size.Y/BUBBLE_TILING_Y);

        Random random = new Random();
        sprite.Scale *= random.Next(2, 5);
    }
    public override void Update(float deltaTime, InputManager inputman)
    {
        movevec = new Vector2f((float)Math.Cos(sprite.Position.Y/20)-(float)Math.PI, 1).Normalize() * speed * deltaTime;
        sprite.Position += movevec;
        TimeToLive -= deltaTime;
        if(TimeToLive <= 0.3){
            animationTime += deltaTime * animationSpeed;
            int animationFrame = (int)animationTime % 3;
            int spriteOffsetX = animationFrame * sprite.TextureRect.Width;
            sprite.TextureRect = new IntRect(
            spriteOffsetX,
            0,
            sprite.TextureRect.Width,
            sprite.TextureRect.Height);
        }
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        if(shaderon){
            window.Draw(sprite, states);
        }else{
            window.Draw(sprite);
        }
    }
    public void SetPositon(Vector2f pos){
        sprite.Position = pos;
    }
    public float GetTimeToLive(){
        return TimeToLive;
    }
}