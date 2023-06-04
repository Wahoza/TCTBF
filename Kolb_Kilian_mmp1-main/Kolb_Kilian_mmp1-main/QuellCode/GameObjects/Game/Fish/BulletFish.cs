//This Fish will Lock Target once and move quickly in that direction
using SFML.Graphics;
using SFML.System;

public class BulletFish : Fish
{
    private Sprite sprite;
    private bool locked = false;
    private Vector2f movevec;
    FloatRect CollisionBox;
    private float scale = 6;
    private float hitboxscale = 0.6f;
    Vector2f textureSize;
    float wait = 1;
    float speed = 2;
    float FISH_TILING_X = 4;
    float FISH_TILING_Y = 1;
    float animationTime = 0;
    float animationSpeed = 4;
    float value = 0;

    public override bool CollisionCheck(FloatRect player)
    {
        if(CollisionBox.Intersects(player)){
            return true;
        }
        return false;
    }

    public override void Initialize()
    {
        locked = false;
        
        Texture texture =  AssetManager.Instance.Textures["BulletFish"];
        sprite = new Sprite(texture);
        textureSize = (Vector2f)texture.Size;

        sprite.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / FISH_TILING_X),
            (int)(texture.Size.Y / FISH_TILING_Y)
        );
        textureSize = new Vector2f(sprite.Texture.Size.X/FISH_TILING_X, sprite.Texture.Size.Y/FISH_TILING_Y);

        Random random = new Random();
        scale =  random.Next(40, 60)/10;
        speed = 100/scale;
        sprite.Scale *= scale;

        Vector2f center = textureSize * 0.5f;
        sprite.Origin = center;
        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y*scale * hitboxscale);
    }

    public override void MoveToGoal(Vector2f player)
    {
        if(!locked){
            if(Utils.Distance(player, sprite.Position) <= 400){
                movevec = Utils.Normalize(new Vector2f(player.X, player.Y -20)-sprite.Position);
                locked = true;
            }else{
                movevec = Utils.Normalize(player-sprite.Position);
                locked = false;
            }
        }
    }

    public override void Update(float deltaTime)
    {

        if(locked && wait>0)
        { 
            wait -= deltaTime;
            animationTime += deltaTime * animationSpeed;
            int animationFrame = (int)animationTime % 4;
            int spriteOffsetX = animationFrame * sprite.TextureRect.Width;
            sprite.TextureRect = new IntRect(
                spriteOffsetX,
                0,
                sprite.TextureRect.Width,
                sprite.TextureRect.Height
            );
        } 
        else if (!locked){

            movevec.Y = 0;
            sprite.Position += new Vector2f(movevec.X *deltaTime * 200, 0);

        }else{
            sprite.Position += movevec * speed * deltaTime;
            speed = 1000;
        }
        if(sprite.Position.Y< 100){
            SetPositon(new Vector2f(sprite.Position.X, 100));
        }
        if(Utils.Angle(movevec) >= Math.PI){
            sprite.Scale = new Vector2f(scale, -scale);
        }

        sprite.Rotation = Utils.Angle(movevec) *  360 /2 /(float)Math.PI;
        
        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;
    }


    public override void Draw(RenderWindow window)
    {

        window.Draw(sprite);
    }
    public override void SetPositon(Vector2f position)
    {
        sprite.Position = position;
        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;
        if(value == 0){
            value = 3 + scale + position.Y/20;
        }
    }
    public override Vector2f GetPosition()
    {
        return sprite.Position;
    }
    public override void SetRotation(Vector2f Rotationvec)
    {
        sprite.Rotation = (float)(Utils.Angle(Rotationvec) * 360 / (2*Math.PI));
    }
    public override float GetScale()
    {
        return scale;
    }
    public override float GetValue()
    {
        return value;
    }
}
