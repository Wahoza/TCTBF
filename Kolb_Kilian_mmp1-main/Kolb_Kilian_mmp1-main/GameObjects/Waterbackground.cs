using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class WaterBackground : GameObject{
    FloatRect Water = new FloatRect();
    Vector2f area;
    public override void Initialize()
    {
       FloatRect Water = new FloatRect(); 
    }
    public override void Update(float deltaTime, InputManager inputman){

    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        Vector2f center = window.GetView().Center;
        Water.Top = center.Y - area.Y/2;
        Water.Left = center.X- area.X/2;
        byte blue;
        if(center.Y < 30){
            blue = 255;
        }else if(center.Y/100 < 255){
           blue = (byte)(255-(int)(center.Y/100));
        }else{
            blue = 0;
        }

        byte green;
        if(center.Y < 30){
            green = 200;
        }else if(center.Y/50 < 200){
           green = (byte)(200-(int)(center.Y/50));
        }else{
            green = 0;
        }
        
        DrawUtils.DrawRect(new Color(0, green, (blue)), Water, window);
    }
    public void getArea(Vector2f _area){
        area = _area;
        Water.Width = area.X;
        Water.Height = area.Y;
    }
}