//This Fish will Lock Target once and move quickly in that direction
using SFML.Graphics;
using SFML.System;

public class BulletFish : Fish
{
    private Sprite? sprite;
    private bool locked = false;
    private Vector2f movevec;
    public override int ColisionCheck(FloatRect player)
    {
        if(sprite.GetGlobalBounds().Intersects(player)){
            return 2;
        }
        return 0;
    }

    public override void Initialize(AssetManager assetman)
    {
        Texture texture =  AssetManager.Instance.Textures["fish"];
        sprite.Texture = texture;
        sprite.Scale *= 3;
    }

    public override void MoveToGoal(Vector2f player)
    {
        if(!locked){
            if(Utils.Distance(player, sprite.Position) <= 200){
                movevec = Utils.Normalize(player-sprite.Position);
                locked = true;
            }else{
                movevec = Utils.Normalize(player-sprite.Position) * 3;
            }
        }
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        sprite.Rotation = Utils.Angle(movevec);
        sprite.Position += movevec *deltaTime;
        movevec = movevec * 2;
    }
    public override void Draw(RenderWindow window)
    {
        window.Draw(sprite);
    }
}
