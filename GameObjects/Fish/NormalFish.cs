//This Fish will move in bursts twoards the hook
using SFML.Graphics;
using SFML.System;

public class NormalFish : Fish
{
    private Sprite? sprite;
    private bool locked = false;
    private Vector2f movevec;
    float resetmovevectime;
    public override int ColisionCheck(FloatRect player)
    {
        if(sprite.GetGlobalBounds().Intersects(player)){
            return 1;
        }
        return 0;
    }

    public override void MoveToGoal(Vector2f player)
    {
        if(!locked){
            movevec = Utils.Normalize(player-sprite.Position);
            locked = true;
            resetmovevectime = 2;
        }
    }
    
    public override void Initialize(AssetManager assetman)
    {
        Texture texture =  AssetManager.Instance.Textures["fish"];
        sprite.Texture = texture;
        sprite.Scale *= 3;
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        if(resetmovevectime > 0){
            sprite.Position += movevec * deltaTime * resetmovevectime;
            resetmovevectime -= deltaTime;
        }else{
            locked = false;
        }
    }

    public override void Draw(RenderWindow window)
    {
        window.Draw(sprite);
    }
}