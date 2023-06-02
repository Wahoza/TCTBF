using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class ShopButton : GameObject
{
    Sprite sprite;
    int BUTTON_TILING_X = 3;
    FloatRect CollisionBox;
    float scale = 5;
    float hitboxscale = 1;
    int ParseLocation = 0;
    enum ButtonStatus
    {
        normal = 0,
        down = 1,
        unavadible = 2,
        undefined = 3
    }
    ButtonStatus status = ButtonStatus.undefined;
    public Vector2f textureSize;
    Text Cost = new Text();
    int value = 0;
    FloatRect MousePos;
    public override void Initialize()
    {
        Font font = AssetManager.Instance.Fonts["PixelFont"];
        Cost.Font = font;
        Cost.CharacterSize = 24;
        Cost.OutlineColor = Color.Black;
        Cost.OutlineThickness =3;
        Cost.FillColor = Color.White;
        Cost.Position = new Vector2f(0, 0);
        

        Texture texture = AssetManager.Instance.Textures["Button"];
        sprite = new Sprite(texture);
        sprite.TextureRect = new IntRect(
            0, 0,
            (int)(texture.Size.X / BUTTON_TILING_X),
            (int)(texture.Size.Y)
        );
        textureSize = new Vector2f(sprite.Texture.Size.X/BUTTON_TILING_X, sprite.Texture.Size.Y);
    
        Vector2f center = textureSize * 0.5f;
        sprite.Origin = center;
        sprite.Scale *= scale;
        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y*scale * hitboxscale);
    }
    public void ButtonClick(FloatRect mouse)
    {   
        if(ValueParser.ParseGameState(1)/100 < value || ValueParser.ParseGameState(ParseLocation) == 6){
            status = ButtonStatus.unavadible;
        }else{
            status = ButtonStatus.normal;
        }

        if(status != ButtonStatus.unavadible){
            if(CollisionBox.Intersects(mouse)){
                if(InputManager.Instance.GetMousePressed(Mouse.Button.Left)){
                    status = ButtonStatus.down;
                    
                }else{
                    status = ButtonStatus.normal;
                }

                if(InputManager.Instance.GetMouseUp(Mouse.Button.Left)){
                   ValueParser.WriteGamestate(ParseLocation, ValueParser.ParseGameState(ParseLocation) + 1);
                   ValueParser.WriteGamestate(1, ValueParser.ParseGameState(1) - value*100);
                   
                }
            }
            else{
                
                status = ButtonStatus.normal;
            }
            
        }
    }


    public override void Update(float deltaTime, InputManager inputman)
    {     
        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;

        ButtonClick(MousePos);
    
        int spriteOffsetX = (int)status * sprite.TextureRect.Width;
        
            sprite.TextureRect = new IntRect(
                spriteOffsetX,
                0,
                sprite.TextureRect.Width,
                sprite.TextureRect.Height
            );
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        if(ValueParser.ParseGameState(ParseLocation) != 6){
            window.Draw(sprite);
            window.Draw(Cost);
        }    
    }

    public void SetPosition(Vector2f pos){
        sprite.Position = pos;
        Cost.Position = pos;
    }
    public void SetValue(int _value){
        Cost.DisplayedString = "" + _value;
        value = _value;
    }
    public void SetParseLocation(int _ParseLocation){
        ParseLocation = _ParseLocation;
    }
    public void SetMousePos(FloatRect _MousePos){
        MousePos = _MousePos;
    }
}