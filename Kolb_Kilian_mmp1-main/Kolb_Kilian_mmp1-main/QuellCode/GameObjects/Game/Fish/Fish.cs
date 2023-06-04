using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public abstract class Fish : GameObject{

    public abstract bool CollisionCheck(FloatRect player);
    public abstract void MoveToGoal(Vector2f player);
    public abstract void SetPositon(Vector2f position);
    public abstract Vector2f GetPosition();
    public abstract void SetRotation(Vector2f Rotationvec);
    public abstract float GetScale();
    public abstract float GetValue();
}