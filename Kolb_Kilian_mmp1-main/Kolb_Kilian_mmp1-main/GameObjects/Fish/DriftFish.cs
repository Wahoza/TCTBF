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
    float playerclose = 0;
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

        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;

        if(sprite.Position.Y< 30){
                SetPositon(new Vector2f(sprite.Position.X, 30));
        }
        
    }
    public override void SetPositon(Vector2f position)
    {
        sprite.Position = position;
    }
}