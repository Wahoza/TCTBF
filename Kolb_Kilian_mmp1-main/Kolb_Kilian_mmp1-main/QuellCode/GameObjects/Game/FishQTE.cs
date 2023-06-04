using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class FishQTE : GameObject
{
    CircleShape OutsideCircle = new CircleShape();
    CircleShape InsideCircle = new CircleShape();
    float radius = 100;
    float speed = 60;
    bool QTEcompleted = false;
    bool GoodCatch = false;
    private enum qteStatus{
        Failed = 0,
        Completed = 1,
        Ongoing = 2
    }
    qteStatus status;
    public override void Initialize()
    {
        QTEcompleted = false;
        GoodCatch = false;
        status = qteStatus.Ongoing;
        float radius = 150;
        OutsideCircle = new CircleShape(radius);
        OutsideCircle.OutlineColor = Color.White;
        OutsideCircle.OutlineThickness = 3;
        OutsideCircle.FillColor = Color.Transparent;

        InsideCircle = new CircleShape(40);
        InsideCircle.OutlineColor = Color.Green;
        InsideCircle.OutlineThickness = 10;
        InsideCircle.FillColor = Color.Transparent;
    }

    public override void Update(float deltaTime)
    {
       OutsideCircle.Radius -= deltaTime * speed;
       if(InputManager.Instance.GetKeyDown(Keyboard.Key.Space)){
            if(OutsideCircle.Radius <= InsideCircle.Radius + 7  && InsideCircle.Radius -7 <= OutsideCircle.Radius){
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
    public float GetStatus(){
        return (int)status;
    }
}