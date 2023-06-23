using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class FishQTE : GameObject
{
    CircleShape OutsideCircle = new CircleShape();
    CircleShape InsideCircle = new CircleShape();
    float speed = 60;
    float Innerradius = 40;
    float Outerradius = 150;
    public enum qteStatus{
        Failed = 0,
        Completed = 1,
        Ongoing = 2
    }
    public qteStatus status;
    public override void Initialize()
    {
        status = qteStatus.Ongoing;
        OutsideCircle = new CircleShape(Outerradius);
        OutsideCircle.OutlineColor = Color.White;
        OutsideCircle.OutlineThickness = 3;
        OutsideCircle.FillColor = Color.Transparent;

        InsideCircle = new CircleShape(Innerradius);
        InsideCircle.OutlineColor = Color.Green;
        InsideCircle.OutlineThickness = 10;
        InsideCircle.FillColor = Color.Transparent;
    }

    public override void Update(float deltaTime)
    {
       OutsideCircle.Radius -= deltaTime * speed;
       if(InputManager.Instance.GetKeyDown(Keyboard.Key.Space)){
            if(OutsideCircle.Radius <= InsideCircle.Radius + (InsideCircle.OutlineThickness/2) && InsideCircle.Radius - (InsideCircle.OutlineThickness/2) <= OutsideCircle.Radius){
                status = qteStatus.Completed;
            }else{
                status = qteStatus.Failed;
            }

        }else{
            status = qteStatus.Ongoing;
        }
        if(OutsideCircle.Radius <= 1){
            status = qteStatus.Failed;
        }
    }
    public override void Draw(RenderWindow window)
    {
        window.Draw(InsideCircle);
        window.Draw(OutsideCircle);
    }

    public void SetPositon(Vector2f pos){
        InsideCircle.Position = new Vector2f(pos.X - InsideCircle.Radius, pos.Y - InsideCircle.Radius);
        OutsideCircle.Position = new Vector2f(pos.X - OutsideCircle.Radius, pos.Y - OutsideCircle.Radius);
    }
}