using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
public class Vendor : GameObject
{
    enum Vendorstate{
        none = 0,
        shop = 1,
        information = 2,
        Fishingphase = 3
    }
    bool StartPhase;
    Vendorstate state;
    ShopHud shopHud;
    Sprite Information;


    HudButton ShopButton = new();
    HudButton InformationButton = new HudButton();
    HudButton ExitButton = new();

    SettingsButton SoundButton = new();
    SettingsButton MusicButton = new();

    FloatRect mouse;
    Sprite information;
    Sprite VendorSprite;
    float VENDOR_TILING_X = 5;
    float scale = 4;
    float animationTime = 0;
    float animationSpeed = 5;
    public override void Initialize()
    {
        animationTime = 0;

        shopHud = new ShopHud();
        shopHud.Initialize();

        InformationButton.Initialize("InfoButton", 3, new Vector2f(-70, 0));
        ShopButton.Initialize("ShopButton", 3 , new Vector2f(-140, 0));


        ExitButton.Initialize("CloseButton", 3 , new Vector2f(-900, -500));
        SoundButton.Initialize("SoundButton", 3, new Vector2f(-850, -500), 1);
        MusicButton.Initialize("MusicButton", 3, new Vector2f(-800, -500), 0);

        Texture VendorTexture = AssetManager.Instance.Textures["shop"];
        VendorSprite = new Sprite(VendorTexture);
        VendorSprite.TextureRect = new IntRect(
            0,
            0,
            (int)(VendorTexture.Size.X / VENDOR_TILING_X),
            (int)(VendorTexture.Size.Y)
        );
        Vector2f textureSize = new Vector2f(VendorSprite.Texture.Size.X/VENDOR_TILING_X, VendorSprite.Texture.Size.Y);
        VendorSprite.Scale *= scale;
        Vector2f origin = new Vector2f(textureSize.X, textureSize.Y)/2;
        VendorSprite.Origin = origin;

        VendorSprite.Position = new Vector2f(-105, -90);

        Texture InformationTexture = AssetManager.Instance.Textures["Information"];
        information = new Sprite(InformationTexture);
        information.Origin = (Vector2f)InformationTexture.Size/2;
        information.Scale*= 1;
        information.Position = new Vector2f(-600, -200);
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        animationTime += deltaTime * animationSpeed;
        int animationFrame = (int)((int)(animationTime) % (VENDOR_TILING_X));
        int spriteOffsetX = animationFrame * VendorSprite.TextureRect.Width;
        if(StartPhase){

            if(VendorSprite.TextureRect.Left != 0 || (animationTime < 2)){
                VendorSprite.TextureRect = new IntRect(
                  (int)(VendorSprite.TextureRect.Width * (VENDOR_TILING_X-1)- spriteOffsetX),
                  0,
                  VendorSprite.TextureRect.Width,
                  VendorSprite.TextureRect.Height
                );
            }
        }else{
            if(VendorSprite.TextureRect.Left != 4 * VendorSprite.TextureRect.Width || (animationTime < 2)){
                VendorSprite.TextureRect = new IntRect(
                  (int)spriteOffsetX,
                  0,
                  VendorSprite.TextureRect.Width,
                  VendorSprite.TextureRect.Height
                );
            }
        }

        InformationButton.Update(mouse);
        ShopButton.Update(mouse);


        ExitButton.Update(mouse);
        SoundButton.Update(mouse);
        MusicButton.Update(mouse);

        if(state != Vendorstate.Fishingphase){
            if(InformationButton.Clicked()){
                state = Vendorstate.information;
            }
            if(ShopButton.Clicked()){
                state = Vendorstate.shop;
            }
        }
        if(state == Vendorstate.shop){
            shopHud.Update(deltaTime, InputManager.Instance);
        }
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        window.Draw(VendorSprite);
        if(state == Vendorstate.shop){
            shopHud.Draw(window, states, shaderon);
        }
        if(state == Vendorstate.information){
            window.Draw(information);
        }
        if(state!= Vendorstate.Fishingphase){
            ShopButton.Draw(window);
            InformationButton.Draw(window);
            ExitButton.Draw(window);
            MusicButton.Draw(window);
            SoundButton.Draw(window);

            if(ExitButton.Clicked()){
                window.Close();
            }
        }
    }
    public void setShopingPhase(bool phase){
        StartPhase = phase;
    }
    public void getMousePosition(FloatRect pos){
        mouse = pos;
        shopHud.UpdatePointer(pos);
    }
}