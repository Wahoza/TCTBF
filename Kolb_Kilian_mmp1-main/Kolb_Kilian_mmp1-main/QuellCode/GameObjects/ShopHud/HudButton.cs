//Author: Kilian Kolb
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class HudButton
{
    bool pressed;
    Sprite buttonSprite = new();
    float BUTTON_TILING_X = 2;
    FloatRect CollisionBox;
    Vector2f textureSize;
    float scale;
    enum ButtonStatus
    {
        up = 0,
        down = 1
    }
    ButtonStatus status;
    public void Initialize(string FileName, float _scale, Vector2f pos){
        Texture texture = AssetManager.Instance.Textures[FileName];
        buttonSprite = new Sprite(texture);
        buttonSprite.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / BUTTON_TILING_X),
            (int)(texture.Size.Y)
        );
        textureSize = new Vector2f(buttonSprite.Texture.Size.X/BUTTON_TILING_X, buttonSprite.Texture.Size.Y);
        buttonSprite.Origin =  textureSize/2;
        buttonSprite.Scale *= _scale;
        scale = _scale;
        buttonSprite.Position = pos;

        CollisionBox = new FloatRect(0, 0, textureSize.X * scale, textureSize.Y*scale);
    }
    public void Update(FloatRect mouse){
        CollisionBox.Left = buttonSprite.Position.X - textureSize.X * 0.5f * scale;
        CollisionBox.Top = buttonSprite.Position.Y - textureSize.Y * 0.5f * scale;

        int spriteOffsetX = (int)status * buttonSprite.TextureRect.Width;
        
            buttonSprite.TextureRect = new IntRect(
                spriteOffsetX,
                0,
                buttonSprite.TextureRect.Width,
                buttonSprite.TextureRect.Height
            );

            pressed = false;

            if(CollisionBox.Intersects(mouse)){
                if(InputManager.Instance.GetMousePressed(Mouse.Button.Left)){
                    status = ButtonStatus.down;
                    
                }else{
                    status = ButtonStatus.up;
                }

                if(InputManager.Instance.GetMouseUp(Mouse.Button.Left)){
                   pressed = true;
                   PlaySound();
                }
            }      
    }
    public bool Clicked(){
        return pressed;
    }
    public void Draw(RenderWindow window){
        window.Draw(buttonSprite);
    }
    private void PlaySound()
    {
        if(ValueParser.ParseSettings(ValueParser.linePositionSettings.Sound) == 1){
            SoundBuffer buffer = AssetManager.Instance.SoundBuffers["Click"];
            Sound sound  = new(buffer);
            sound.Play();
        }
    }

}