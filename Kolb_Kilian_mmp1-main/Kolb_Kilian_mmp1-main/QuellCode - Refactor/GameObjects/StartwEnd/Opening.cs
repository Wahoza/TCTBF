using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Opening : GameObject
{
    Sprite sprite;
    float OPENING_TILING_X = 38;
    float animationTime = 0;
    float animationSpeed = 2;
    bool finished = false;
    Vector2f area;
    Vector2f rectpos;
    public override void Initialize()
    {
        Texture texture = AssetManager.Instance.Textures["InitialAnim"];
        sprite = new Sprite(texture);
        sprite.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / OPENING_TILING_X),
            (int)(texture.Size.Y)
        );
        Vector2f textureSize = new Vector2f(sprite.Texture.Size.X/OPENING_TILING_X, sprite.Texture.Size.Y);
    
        Vector2f center = textureSize * 0.5f;
        sprite.Origin = center;
    }

    public override void Update(float deltaTime)
    {
        if(!finished){
            sprite.Position = new Vector2f(0, 0);
            animationTime += deltaTime * animationSpeed;

            int animationFrame = (int)animationTime % 38;

            int spriteOffsetX = animationFrame * sprite.TextureRect.Width;

            sprite.TextureRect = new IntRect(
                spriteOffsetX+1,
                0,
                sprite.TextureRect.Width,
                sprite.TextureRect.Height
            );
            if(animationFrame > 27){
                sprite.Scale *= 1.01f * deltaTime;
            }
            if(animationFrame == 37){
                finished = true;
            }
        }
        
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
    public bool GetFinished(){
        return finished;
    }
    public override void Draw(RenderWindow window)
    {
        FloatRect rect = new FloatRect(rectpos.X, rectpos.Y, area.X, area.Y);
        DrawUtils.DrawRect(Color.Black, rect, window);
        window.Draw(sprite);
    }
}