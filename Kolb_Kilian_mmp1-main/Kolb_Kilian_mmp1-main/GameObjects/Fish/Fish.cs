using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public abstract class Fish : GameObject{

    public abstract int ColisionCheck(FloatRect player);
    public abstract void MoveToGoal(Vector2f player);
    public abstract void SetPositon(Vector2f position);
}