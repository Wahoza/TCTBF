using SFML.Graphics;
//Author: Kilian Kolb
using SFML.Audio;
using SFML.System;
using SFML.Window;
public abstract class Fish : GameObject{
    public Sprite sprite = new();
    private float value = 0;
    float scale = 1;
    bool exists = false;
    public abstract bool CollisionCheck(FloatRect player);
    public abstract void MoveToGoal(Vector2f player);
    public void SetPositon(Vector2f position){
        if(value == 0){
            value = scale + position.Y/40;
        }
        Console.WriteLine(value);
        sprite.Position = position;
    }
    public Vector2f GetPosition(){
        return sprite.Position;
    }
    public void SetRotation(Vector2f Rotationvec){
        sprite.Rotation = (float)(Utils.Angle(Rotationvec) * 360 / (2*Math.PI));
    }
    public float GetScale(){
        return scale;
    }
    public float GetValue(){
        return value / 2;
    }
}