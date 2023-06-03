using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class SettingsButton
{
    bool pressed;
    Sprite buttonSprite;
    float BUTTON_TILING_X = 2;
    FloatRect CollisionBox;
    Vector2f textureSize;
    float scale;
    int parseLocation;
    public void Initialize(string FileName, float _scale, Vector2f pos, int ParseLocation){

        parseLocation = ParseLocation;

        if(ValueParser.ParseSettings(ParseLocation) == 1){
            pressed = true;
        }else{
            pressed = false;
        }

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

        int pressedtonum;
        if(pressed){
            pressedtonum = 0;
        }else{
            pressedtonum = 1;
        }

        int spriteOffsetX = (int) pressedtonum* buttonSprite.TextureRect.Width;
        
            buttonSprite.TextureRect = new IntRect(
                spriteOffsetX,
                0,
                buttonSprite.TextureRect.Width,
                buttonSprite.TextureRect.Height
            );

            if(CollisionBox.Intersects(mouse)){

                if(InputManager.Instance.GetMouseUp(Mouse.Button.Left)){
                   pressed = !pressed;
                   
                   if(pressed){
                        ValueParser.WriteSettings(parseLocation, 1);
                   }else{
                        ValueParser.WriteSettings(parseLocation, 0);
                   }
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
        if(ValueParser.ParseSettings(1) == 1){
            SoundBuffer buffer = AssetManager.Instance.SoundBuffers["Click"];
            Sound sound  = new(buffer);
            sound.Play();
        }
    }
}