using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class Hud : GameObject
{
    Text text = new();
    Vector2f DistanceFromEdge = new Vector2f(20, 20);
    string Depth;
    public override void Initialize(){
        Font font = AssetManager.Instance.Fonts["PixelFont"];
        text.Font = font;
        text.CharacterSize = 24;
        text.OutlineColor = Color.Black;
        text.OutlineThickness =3;
        text.FillColor = Color.White;
        text.Position = new Vector2f(0, 0);
    }
    public override void Update(float deltaTime, InputManager inputman)
    {
        if((text.Position.Y * -1 -414) < 0){
            Depth = (((int)((text.Position.Y+414)/100))*-1).ToString();
        }else{
            Depth = "0";
        }
        text.DisplayedString = "Depth: " + Depth;
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        text.Position = window.GetView().Center - window.GetView().Size / 2 + DistanceFromEdge;
        window.Draw(text);
    }
}