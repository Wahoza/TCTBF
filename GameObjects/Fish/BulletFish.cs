//This Fish will Lock Target once and move quickly in that direction
using SFML.Graphics;
using SFML.System;

public class BulletFish : Fish
{
    private Sprite sprite;
    private bool locked = false;
    private Vector2f movevec;
    FloatRect CollisionBox;
    private float scale = 3;
    private float hitboxscale = 0.9f;
    Vector2f textureSize;
    float wait = 1;
    float speed = 2;
    public override int ColisionCheck(FloatRect player)
    {
        if(CollisionBox.Intersects(player)){
            return 2;
        }
        return 0;
    }

    public override void Initialize()
    {
        Texture texture =  AssetManager.Instance.Textures["fish"];
        sprite = new Sprite(texture);
        textureSize = (Vector2f)texture.Size;
        sprite.Scale *= scale;
        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y *scale * hitboxscale);
    
    }

    public override void MoveToGoal(Vector2f player)
    {
        if(!locked){
            if(Utils.Distance(player, sprite.Position) <= 400){
                movevec = Utils.Normalize(player-sprite.Position);
                locked = true;
            }else{
                movevec = Utils.Normalize(player-sprite.Position) * 3;
            }
        }
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;
        sprite.Rotation = Utils.Angle(movevec);
        if(locked && wait>1){ wait -= deltaTime;} 
        else if (!locked){
            sprite.Position += movevec *deltaTime;
        }else{
            sprite.Position += movevec * speed * deltaTime;
            speed = speed * (1 + deltaTime);
        }
    }
    public override void Draw(RenderWindow window)
    {
        window.Draw(sprite);
    }
}
