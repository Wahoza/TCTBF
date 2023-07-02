//Author: Kilian Kolb
using SFML.Graphics;
using SFML.System;
using SFML.Window;
public static class Utils{
    public static float SqrMagnitude(this Vector2f input){
        return (input.X *input.X + input.Y*input.Y);
    }
    public static float Magnitude(this Vector2f input){
        return Convert.ToSingle(Math.Sqrt(input.X *input.X + input.Y*input.Y));
    }
    public static float ToDegrees(this float angle){
        return (angle / Convert.ToSingle(Math.PI)) * 180;
    }
    public static float ToRadians(this float angle){
        return (angle * Convert.ToSingle(Math.PI)) / 180;
    }
    public static Vector2f Normalize(this Vector2f source){
        Vector2f normalized;
        float sqr = source.Magnitude();
        if(sqr != 0){
        normalized.X = source.X / sqr;
        normalized.Y = source.Y / sqr;
        }else{
        normalized = new Vector2f (0, 0);    
        }

        return normalized;
    }

    public static Vector2f RotateVector(Vector2f v, float angle){
        Vector2f newVec;
        newVec.X = v.X * Convert.ToSingle(Math.Cos(angle)) -v.Y * Convert.ToSingle(Math.Sin(v.Y));
        newVec.Y = v.X * Convert.ToSingle(Math.Sin(angle)) +v.Y * Convert.ToSingle(Math.Cos(v.Y));
        return newVec;
    }
    public static float AngleBetween(Vector2f v1, Vector2f v2){
        //acos((ax*bx+ay*by)/(|a|*|b|)
        return(
            Convert.ToSingle(Math.Acos(Dot(v1, v2)
            /(v1.Magnitude()*v2.Magnitude())))
        );
    }
    public static float Angle(Vector2f v){
        int i = 1;
        if(v.Y < 0){
            i = -1;
        }
        return (AngleBetween(v,new Vector2f (1,0)) * i);
    }
    public static float Distance(Vector2f a, Vector2f b){
        Vector2f newVec = b - a;
        return newVec.Magnitude();
    }
    public static Vector2f Lerp(Vector2f firstVector, Vector2f secondVector, float t){
        Vector2f newVec = (secondVector-firstVector) * t + firstVector;
        return newVec;
    }
    public static float Dot(Vector2f lhs, Vector2f rhs){
        return (lhs.X * rhs.X) + (lhs.Y * rhs.Y);
    }
}
