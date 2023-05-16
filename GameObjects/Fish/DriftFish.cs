//This Fish will move without any goal in mind
using SFML.Graphics;
using SFML.System;

public class DriftFish : Fish
{
    Sprite? sprite;
    public override int ColisionCheck(FloatRect player)
    {
        if(sprite.GetGlobalBounds().Intersects(player)){
            return 3;
        }
        return 0;
    }

    public override void Draw(RenderWindow window)
    {
        throw new NotImplementedException();
    }

    public override void Initialize(AssetManager assetman)
    {
        throw new NotImplementedException();
    }

    public override void MoveToGoal(Vector2f player)
    {
        throw new NotImplementedException();
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        throw new NotImplementedException();
    }
}