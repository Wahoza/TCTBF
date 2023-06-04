using SFML.Graphics;
using SFML.System;


//Note: due to prior code the Group needs to be a fish, however not all of the Methods are actually usefull
public class FishGroup : Fish
{
    public List<SubFish> Group = new();
    SubFish collided;
    Vector2f position;
    float radius;
    int MaxfishAmount = 7;
    int MinfishAmount = 1;
    Random random = new();

    public override void Initialize()
    {
        radius = random.Next(100, 500);
        Group = new();
        for(int i = 0; i <= random.Next(MinfishAmount, MaxfishAmount); i++){
            SubFish fish = new();
            fish.Initialize();
            fish.SetRadius(radius);
            Group.Add(fish);
        }
    }
    public override bool CollisionCheck(FloatRect player)
    {
        foreach(SubFish fish in Group){
            if(fish.CollisionCheck(player)){
                return fish.CollisionCheck(player);
            }

        }
        return false;
    }

    public Fish CollisionTest(FloatRect player){
        foreach(SubFish fish in Group){
            if(fish.CollisionCheck(player)){
                collided = fish;
                return fish;
            }

        }
        return null;
    }

    public override Vector2f GetPosition()
    {
        return position;
    }

    public override float GetScale()
    {
        return radius;
    }

    public override float GetValue()
    {
        return collided.GetValue();
    }

    public override void MoveToGoal(Vector2f player)
    {
        //handles Movement around Center
        foreach(SubFish fish in Group){
            fish.MoveToGoal(position);
        }
        
    }

    public override void SetPositon(Vector2f _position)
    {
        position = _position;
        foreach(SubFish fish in Group){
            float y = random.Next(0, 2*(int)radius) - radius;
            float x = random.Next(0, 2*(int)radius) - radius;
            fish.SetPositon(_position - new Vector2f(x, y));
        }
    }

    public override void SetRotation(Vector2f Rotationvec)
    {
        foreach(SubFish fish in Group){
            fish.SetRotation(Rotationvec);
        }
    }

    public override void Update(float deltaTime)
    {
        foreach(SubFish fish in Group){
            fish.Update(deltaTime);
        }
    }
    public override void Draw(RenderWindow window)
    {
        foreach(SubFish fish in Group){
            fish.Draw(window);
        }
    }
}

public class SubFish : Fish
{
    private Sprite sprite;
    private Vector2f movevec;
    FloatRect CollisionBox;
    private float scale = 3;
    private float hitboxscale = 0.6f;
    Vector2f textureSize;
    float playerclose = 0;
    float speed;
    int FISH_TILING_X = 3;
    int FISH_TILING_Y = 8;
    float animationTime = 0;
    float animationSpeed = 10;
    float value = 0;
    float radius;
    bool exists;

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

        Vector2f center = textureSize * 0.5f;
        sprite.Origin = center;

        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y *scale * hitboxscale);
        movevec = new Vector2f(1 - (random.Next(2,3) %2) * 2,0);
        exists = false;
    }
    public override bool CollisionCheck(FloatRect player)
    {
        if(CollisionBox.Intersects(player) && exists){
            return true;
        }
        return false;
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
        return value;
    }

    public override void MoveToGoal(Vector2f center)
    {
        if(Utils.Distance(center, sprite.Position) > radius){
            if(center.X > sprite.Position.X){
                movevec = new Vector2f(1, 0);
            }else{
                movevec = new Vector2f (-1, 0);
            }
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
                value = scale + position.Y/10;
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