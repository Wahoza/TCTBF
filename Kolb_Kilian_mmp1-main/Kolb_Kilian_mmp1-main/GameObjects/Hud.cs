using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class Hud : GameObject
{
    Text DepthText = new();
    Vector2f DistanceFromEdge = new Vector2f(20, 20);
    string Depth;
    float ClockTime;
    public override void Initialize(){
        Font font = AssetManager.Instance.Fonts["PixelFont"];
        DepthText.Font = font;
        DepthText.CharacterSize = 24;
        DepthText.OutlineColor = Color.Black;
        DepthText.OutlineThickness =3;
        DepthText.FillColor = Color.White;
        DepthText.Position = new Vector2f(0, 0);
    }
    public override void Update(float deltaTime, InputManager inputman)
    {
        if((DepthText.Position.Y * -1 -414) < 0){
            Depth = (((int)((DepthText.Position.Y+414)/100))*-1).ToString();
        }else{
            Depth = "0";
        }
        DepthText.DisplayedString = "Depth: " + Depth;
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        DepthText.Position = window.GetView().Center - window.GetView().Size / 2 + DistanceFromEdge;
        window.Draw(DepthText);
    }
    public void SetTimer(float _ClockTime){
        ClockTime = _ClockTime;
    }
}