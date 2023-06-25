using SFML.Graphics;
using SFML.System;
public class SubFish : Fish
{
    private Sprite sprite = new();
    private Vector2f movevec;
    FloatRect CollisionBox;
    private float scale = 3;
    private float hitboxscale = 0.6f;
    Vector2f textureSize;
    float speed;
    int FISH_TILING_X = 3;
    int FISH_TILING_Y = 8;
    float animationTime = 0;
    float animationSpeed = 10;
    float value = 0;
    float radius;
    bool exists;
    Vector2f CenterOrigin = new Vector2f(0,0);

    public override void Initialize()
    {

        Texture texture =  AssetManager.Instance.Textures["SubFish"];
        sprite = new Sprite(texture);

        Random random = new();
        
        textureSize = (Vector2f)texture.Size;

        sprite.TextureRect = new IntRect(
            1, 
            1 + random.Next(0, FISH_TILING_Y-1) * (int)(texture.Size.Y / FISH_TILING_Y),

            (int)(texture.Size.X / FISH_TILING_X),
            (int)(texture.Size.Y / FISH_TILING_Y)
        );

        textureSize = new Vector2f(sprite.Texture.Size.X/FISH_TILING_X, sprite.Texture.Size.Y/FISH_TILING_Y);

        scale =  random.Next(20, 50)/10;
        speed = random.Next(100, 200);

        sprite.Scale *= scale;

        sprite.Origin = textureSize * 0.5f;

        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y *scale * hitboxscale);
        movevec = new Vector2f(1 - (random.Next(2,3) %2) * 2,0);
        exists = false;
    }
    public override bool CollisionCheck(FloatRect player)
    {
        return CollisionBox.Intersects(player) && exists;
    }
    public void SetOrigin(Vector2f FishOrigin){
        CenterOrigin = FishOrigin;
    }

    public override Vector2f GetPosition()
    {
        return sprite.Position;
    }

    public override float GetScale()
    {
        return scale;
    }

    public override float GetValue()
    {
        return value / 3;
    }

    public override void MoveToGoal(Vector2f player)
    {
        //doesnt react to player, therefore ignores player
        if(Utils.Distance(CenterOrigin, sprite.Position) > radius){
            if(CenterOrigin.X > sprite.Position.X){
                movevec = new Vector2f(1, 0);
            }else{
                movevec = new Vector2f (-1, 0);
            }

            //turns the fish around
            if(movevec.X>0){
                sprite.Scale = new Vector2f (scale, scale);
            }else{
                sprite.Scale = new Vector2f (-scale, scale);
            }
        }
    }

    public override void SetPositon(Vector2f position)
    {
        if(!exists){
            if(position.Y<100){
                exists = false;
            }else{
                if(value == 0){
                    value = scale *0.7f + position.Y/40;
                }
                exists = true;
            }
        }
        sprite.Position = position;
    }

    public override void SetRotation(Vector2f Rotationvec)
    {
        sprite.Rotation = (float)(Utils.Angle(Rotationvec) * 360 / (2*Math.PI));
        sprite.Scale = new Vector2f(scale, scale);
    }
    public void SetRadius(float _radius){
        radius = _radius;
    }

    public override void Update(float deltaTime)
    {
        if(exists){
            sprite.Position += movevec * deltaTime * speed;

            animationTime += deltaTime * animationSpeed;
            int animationFrame = (int)animationTime % 3;
            int spriteOffsetX;
            spriteOffsetX = animationFrame * sprite.TextureRect.Width;

            sprite.TextureRect = new IntRect(
                1 + spriteOffsetX,
                sprite.TextureRect.Top,
                sprite.TextureRect.Width,
                sprite.TextureRect.Height
            );

            CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
            CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;
        }
    }
    public override void Draw(RenderWindow window)
    {
        if(exists){
            window.Draw(sprite);
        }
    }
}