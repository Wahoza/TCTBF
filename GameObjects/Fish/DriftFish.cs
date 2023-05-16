//This Fish will move without any goal in mind
using SFML.Graphics;
using SFML.System;

public class DriftFish : Fish
{
    private Sprite sprite;
    private Vector2f movevec;
    FloatRect CollisionBox;
    private float scale = 3;
    private float hitboxscale = 0.9f;
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
        Vector2f textureSize = (Vector2f)texture.Size;
        sprite.Scale *= scale;
        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y *scale * hitboxscale);
    
    }

    public override void Draw(RenderWindow window)
    {
        window.Draw(sprite);
    }

    public override void MoveToGoal(Vector2f player)
    {
        if(Utils.Distance(player, sprite.Position) > 70){
            movevec = player - sprite.Position;
        }else{
            movevec = sprite.Position - player;
        }
        movevec.Normalize(); 
    }

    public override void Update(float deltaTime, InputManager inputman)
    {  
        sprite.Position += movevec * 2 * deltaTime;

        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;
        
    }
}