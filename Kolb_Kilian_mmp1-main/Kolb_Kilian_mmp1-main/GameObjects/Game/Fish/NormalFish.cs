//This Fish will move twoard the hook but be scared away
using SFML.Graphics;
using SFML.System;

public class NormalFish : Fish
{
    private Sprite sprite;
    private Vector2f movevec;
    FloatRect CollisionBox;
    private float scale = 3;
    private float hitboxscale = 0.8f;
    Vector2f textureSize;
    float playerclose = 0;
    float speed;
    float FISH_TILING_X = 2;
    float FISH_TILING_Y = 1;
    float animationTime = 0;
    float animationSpeed = 5;
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
        Texture texture =  AssetManager.Instance.Textures["NormalFish"];
        sprite = new Sprite(texture);
        textureSize = (Vector2f)texture.Size;
            sprite.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / FISH_TILING_X),
            (int)(texture.Size.Y / FISH_TILING_Y)
        );
        textureSize = new Vector2f(sprite.Texture.Size.X/FISH_TILING_X, sprite.Texture.Size.Y/FISH_TILING_Y);

        Random random = new Random();
        scale =  random.Next(20, 50)/10;
        speed = 300/scale;
        sprite.Scale *= scale;
        Vector2f center = textureSize * 0.5f;
        sprite.Origin = center;
        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y *scale * hitboxscale);
    
    }

    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        if(shaderon){
            window.Draw(sprite, states);
        }else{
            window.Draw(sprite);
        }
    }

    public override void MoveToGoal(Vector2f player)
    {
        if(Utils.Distance(player, sprite.Position) > 70 && playerclose <= 0){
            movevec = Utils.Normalize(player - sprite.Position);
        }else if(playerclose <= 0){
            movevec = Utils.Normalize(sprite.Position - player) * 2;
            playerclose = 0.5f;
        }else{
            movevec = Utils.Normalize(sprite.Position - player) * 2;
        }
        movevec.Normalize(); 
    }

    public override void Update(float deltaTime, InputManager inputman)
    {  
        if(playerclose >0){
            playerclose -= deltaTime;
        }
        sprite.Position += movevec * 100 * deltaTime;

        animationTime += deltaTime * animationSpeed;
        int animationFrame = (int)animationTime % 2;
        int spriteOffsetX = animationFrame * sprite.TextureRect.Width;
        sprite.TextureRect = new IntRect(
            spriteOffsetX,
            0,
            sprite.TextureRect.Width,
            sprite.TextureRect.Height
        );

        sprite.Rotation = Utils.Angle(movevec) *  360 /2 /(float)Math.PI;


        if(sprite.Position.Y< 100){
            SetPositon(new Vector2f(sprite.Position.X, 100));
        }
        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;
    }
    public override void SetPositon(Vector2f position)
    {
        sprite.Position = position;
        if(value == 0){
            value = 10 + scale + position.Y/10;
        }
    }

    public override Vector2f GetPosition(){
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