//Author: Kilian Kolb
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class Hud : GameObject
{
    Text DepthText = new();
    Vector2f DistanceFromEdge = new Vector2f(20, 20);
    string Depth = "";
    float MoveAmount;
    float InitialMoveAmount;
    Sprite MoveDisable = new();
    Vector2f textureSize;
    float scale = 6;
    float SPRITE_TILING_X = 2;
    Text TimeText = new();
    float DepthOffset = -414;
    float TextDistanceFromLeft = 80;
    public override void Initialize(){
        Font font = AssetManager.Instance.Fonts["PixelFont"];
        DepthText.Font = font;
        DepthText.CharacterSize = 24;
        DepthText.OutlineColor = Color.Black;
        DepthText.OutlineThickness =3;
        DepthText.FillColor = Color.White;
        DepthText.Position = new Vector2f(0, 0);

        Texture texture = AssetManager.Instance.Textures["MoveEnabled"];
        MoveDisable = new Sprite(texture);
        MoveDisable.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / SPRITE_TILING_X),
            (int)(texture.Size.Y)
        );
        textureSize = new Vector2f(MoveDisable.Texture.Size.X/SPRITE_TILING_X, MoveDisable.Texture.Size.Y);
        MoveDisable.Scale *= scale;
        Vector2f center = textureSize * 0.5f;
        MoveDisable.Origin = center;
        TimeText.Font = font;
        TimeText.CharacterSize = 24;
        TimeText.OutlineColor = Color.Black;
        TimeText.OutlineThickness =3;
        TimeText.FillColor = Color.White;
        TimeText.Position = new Vector2f(0, 0);
        
    }
    public override void Update(float deltaTime)
    {
        if((DepthText.Position.Y * -1 +DepthOffset) < 0){
            Depth = (((int)((DepthText.Position.Y - DepthOffset)/100))*-1).ToString();
        }else{
            Depth = "0";
        }
        DepthText.DisplayedString = "Depth: " + Depth;

        if(InitialMoveAmount - MoveAmount >= 0){
            TimeText.DisplayedString = "" +(InitialMoveAmount - MoveAmount);
        }else{

            int spriteOffsetX = 1 * MoveDisable.TextureRect.Width;


            MoveDisable.TextureRect = new IntRect(
                spriteOffsetX,
                0,
                MoveDisable.TextureRect.Width,
                MoveDisable.TextureRect.Height
            );
        }
    }
    public override void Draw(RenderWindow window)
    {
        DepthText.Position = window.GetView().Center - window.GetView().Size / 2 + DistanceFromEdge;
        TimeText.Position = window.GetView().Center - window.GetView().Size / 2 + DistanceFromEdge+new Vector2f(MoveDisable.TextureRect.Width * scale, TextDistanceFromLeft);
        MoveDisable.Position = window.GetView().Center - window.GetView().Size / 2 + DistanceFromEdge+new Vector2f(MoveDisable.TextureRect.Width * scale/2 -6, TextDistanceFromLeft);
        window.Draw(DepthText);
        window.Draw(TimeText);
        window.Draw(MoveDisable);
    }
    public void SetMoveAmount(float _MoveAmount){
        MoveAmount += _MoveAmount;
    }
    public void SetInitialMoveAmount(float init){
        InitialMoveAmount = init;
    }
    public void ChangeMovesAllowed(float move){
        MoveAmount -= move;
    }
}