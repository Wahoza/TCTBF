//Author: Kilian Kolb
//This Fish will move in bursts twoards the hook
using SFML.Graphics;
using SFML.System;

public class SquidFish : Fish
{
    private bool locked = false;
    private Vector2f movevec;
    FloatRect CollisionBox;
    private float scale = 3;
    private float hitboxscale = 0.5f;
    float resetmovevectime;
    float speed;
    Vector2f textureSize;
    float animationTime = 0;
    float animationSpeed = 2;
    float FISH_TILING_X = 2;
    public override bool CollisionCheck(FloatRect player)
    {
        if(CollisionBox.Intersects(player)){
            return true;
        }
        return false;
    }

    public override void Initialize()
    {
        Texture texture =  AssetManager.Instance.Textures["SquidFish"];
        sprite = new Sprite(texture);
        textureSize = (Vector2f)texture.Size;
            sprite.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / FISH_TILING_X),
            (int)(texture.Size.Y)
        );
        textureSize = new Vector2f(sprite.Texture.Size.X/FISH_TILING_X, sprite.Texture.Size.Y);

        Random random = new Random();
        scale =  random.Next(40, 60)/10;
        speed = 400/scale;
        sprite.Scale *= scale;

        Vector2f center = textureSize * 0.5f;
        sprite.Origin = center;
        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y*scale * hitboxscale);
        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y *scale * hitboxscale);
    
    }

    public override void MoveToGoal(Vector2f player)
    {
        if(!locked){
            movevec = Utils.Normalize(player-sprite.Position);
            locked = true;
            resetmovevectime = 2;
        }
    }

    public override void Update(float deltaTime)
    {
        if(resetmovevectime > 0){
            sprite.Position += movevec * deltaTime * resetmovevectime * speed;
            resetmovevectime -= deltaTime;
            animationTime += deltaTime * animationSpeed;
            int animationFrame = (int)animationTime % 2;
            int spriteOffsetX = animationFrame * sprite.TextureRect.Width;
            sprite.TextureRect = new IntRect(
            spriteOffsetX,
            0,
            sprite.TextureRect.Width,
            sprite.TextureRect.Height
        );
        }else{
            locked = false;
        }

        sprite.Rotation = Utils.Angle(movevec) *  360 /2 /(float)Math.PI;
        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;

        if(sprite.Position.Y<  100){
                SetPositon(new Vector2f(sprite.Position.X, 100));
        }
    }

    public override void Draw(RenderWindow window)
    {
        window.Draw(sprite);
        
    }
}