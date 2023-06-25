//This Fish will Lock Target once and move quickly in that direction
using SFML.Graphics;
using SFML.System;

public class DriftFish : Fish
{
    private Sprite sprite = new();
    private Vector2f movevec = new Vector2f(0, 0);
    FloatRect CollisionBox;
    private float scale = 7;
    private float hitboxscale = 0.9f;
    Vector2f textureSize;
    float wait = 1;
    float passedtime = 0;
    float speed = 20;
    Vector2f TargetPosition;
    bool initialFind = true;
    float FISH_TILING_X = 3;
    float animationTime = 0;
    float animationSpeed = 10;
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
        Texture texture =  AssetManager.Instance.Textures["DriftFish"];
        sprite = new Sprite(texture);
        textureSize = (Vector2f)texture.Size;
            sprite.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / FISH_TILING_X),
            (int)(texture.Size.Y)
        );
        textureSize = new Vector2f(sprite.Texture.Size.X/FISH_TILING_X, sprite.Texture.Size.Y);

        Random random = new Random();
        scale =  random.Next(30, 60)/10;
        sprite.Scale *= scale;
        speed = 70/scale;

        Vector2f center = textureSize * 0.5f;
        sprite.Origin = center;
        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y*scale * hitboxscale);
    }

    public override void MoveToGoal(Vector2f player)
    {
        TargetPosition = player;
    }

    public override void Update(float deltaTime)
    {
        CalculateMovevec(deltaTime);
        
        sprite.Position += movevec * deltaTime *10;
        animationTime += deltaTime * animationSpeed;
        int animationFrame = (int)animationTime % 3;
        int spriteOffsetX = animationFrame * sprite.TextureRect.Width;
        sprite.TextureRect = new IntRect(
            spriteOffsetX,
            0,
            sprite.TextureRect.Width,
            sprite.TextureRect.Height
        );

        sprite.Rotation = Utils.Angle(movevec) *  360 /2 /(float)Math.PI;
        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;
        if(sprite.Position.Y< 100){
            SetPositon(new Vector2f(sprite.Position.X, 100));
        }
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
            value = scale + position.Y/17;
        }
    }
    private void CalculateMovevec(float deltaTime){
        if(((Utils.AngleBetween(movevec, TargetPosition-sprite.Position) > 0.1) && initialFind)|| passedtime >wait * 100){
            initialFind = true;
            passedtime -= deltaTime;

            if(movevec.SqrMagnitude() != 0){
                float rotationAngle = (float)Math.PI / 2 * deltaTime;
                movevec =   new Vector2f((float)(Math.Cos(rotationAngle) * movevec.X + Math.Sin(rotationAngle) * movevec.Y),
                            (float)(-Math.Sin(rotationAngle) * movevec.X + Math.Cos(rotationAngle) * movevec.Y));
                movevec = movevec.Normalize() *4;
            }else{
                movevec = new Vector2f(1, 1);
            }
        }else{
            if(initialFind){
                passedtime = 0;
                initialFind = false;
                movevec = TargetPosition-sprite.Position;
                movevec = movevec.Normalize() * speed;
            }
            passedtime += deltaTime * 100;
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
        return value / 3;
    }
}