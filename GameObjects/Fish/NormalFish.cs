//This Fish will move in bursts twoards the hook
using SFML.Graphics;
using SFML.System;

public class NormalFish : Fish
{
    private Sprite sprite;
    private bool locked = false;
    private Vector2f movevec;
    FloatRect CollisionBox;
    private float scale = 3;
    private float hitboxscale = 0.9f;
    float resetmovevectime;
    Vector2f textureSize;
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
            movevec = Utils.Normalize(player-sprite.Position);
            locked = true;
            resetmovevectime = 2;
        }
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        if(resetmovevectime > 0){
            sprite.Position += movevec * deltaTime * resetmovevectime * 100;
            resetmovevectime -= deltaTime;
        }else{
            locked = false;
        }
        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;
    }

    public override void Draw(RenderWindow window)
    {
        window.Draw(sprite);
    }
}