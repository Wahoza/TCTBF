using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class WaterBackground : GameObject{
    FloatRect Water = new FloatRect();
    Vector2f area;
    
    private Sprite background;
    private float scale = 0;
    public override void Initialize()
    {
        Water = new FloatRect(); 
        Texture texture = AssetManager.Instance.Textures["Background"];
        background = new Sprite(texture);
    
    }
    public override void Update(float deltaTime){

    }
    public override void Draw(RenderWindow window)
    {
        Vector2f center = window.GetView().Center;
        Water.Top = (center.Y - area.Y/2 > 70)
            ? center.Y - area.Y/2
            : 70;
        
        Water.Left = center.X- area.X/2;
        
        byte blue = (center.Y < 30)
            ? (byte)255
            : (center.Y / 100 < 255)
                ? (byte)(255 - (int)(center.Y / 100))
                : (byte)0;


        byte green = (center.Y < 30)
            ? (byte)200
            : (center.Y / 50 < 200)
                ? (byte)(200 - (int)(center.Y / 50))
                : (byte)0;
        background.Position = new Vector2f(-area.X/2 - 2*scale, -area.Y/2 - background.Texture.Size.Y*scale/2 + 200);

        for (int i = -1; i <= 1; i++){
            background.Position = new Vector2f(0 + background.Scale.X * (background.Texture.Size.X-3)*i, -370);
            window.Draw(background);
        }
        

        DrawUtils.DrawRect(new Color(0, green, (blue)), Water, window);
    }
    public void SetArea(Vector2f _area){
        area = _area;
        Water.Width = area.X;
        Water.Height = area.Y;

        scale = area.X/ (background.Texture.Size.X-10) * 1.5f;
        background.Scale*=scale;
        background.Origin = (Vector2f)background.Texture.Size / 2;
        background.Position = new Vector2f(0, -200);
    }
}