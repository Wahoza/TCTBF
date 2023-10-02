using SFML.Graphics;
//Author: Kilian Kolb
using SFML.Audio;
using SFML.System;
using SFML.Window;
public abstract class Fish : GameObject{
    public Sprite sprite = new();
    private float value = 0;
    float scale = 1;
    public abstract bool CollisionCheck(FloatRect player);
    public abstract void MoveToGoal(Vector2f player);
    public void SetPosition(Vector2f position){
        if(value == 0){
            value = scale*4 + position.Y/40+5;
        }
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
    public void SetValue(int initialValue, float depth){
        value = scale * 3 + initialValue + depth *3;
    }
    public void SetTransparity(byte value){
        sprite.Color = new Color(sprite.Color.R, sprite.Color.G, sprite.Color.B, value);
    }
}