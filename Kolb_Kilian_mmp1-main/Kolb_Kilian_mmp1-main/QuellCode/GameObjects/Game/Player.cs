using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class Player : GameObject
{
    private const int PLAYER_TILING_X = 3;
    private const int PLAYER_TILING_Y = 3;

    private Sprite sprite;

    private float animationTime = 0;
    private float animationSpeed = 10;

    private Vector2f move = new Vector2f(0, 0);
    public FloatRect CollisionBox;

    private int[] frameCountPerAnimation = new int[3];
    private AnimationType currentAnimation = AnimationType.Down;
    public enum AnimationType
{
    Down = 0,
    Left = 1,
    Right = 2
}
    
    private Vector2f MoveVec;
    private float MoveScale = 50;
    private float MoveCooldown = 0;
    public bool startmovement = false;
    private float DeltaTime;
    public bool collided = false;
    public bool restart = false;

    Fish CoughtFish;
    Vector2f textureSize;
    float scale = 3;
    float hitboxscale = 0.5f;


    View view;
    float CameraX = 0;
    bool CameramoveX = true;

    FishQTE qte;
    bool qteActive;
    bool[] completed = new bool[2];
    private Vector2f returnPoint;

    float MoveTimer;
    float weight;

    Sprite splash;
    float splashAnimSpeed = 10;
    float SPLASH_TILING_X = 5;
    bool splashAnim = false;
    float splashAnimTime = 0;
    public override void Initialize()
    {   
        weight = 100 * ((float)ValueParser.ParseGameState(6)) ;
        
        move = new Vector2f(0, 0);
        CameraX = 0;
        CameramoveX = true;
        startmovement = false;
        collided = false;
        restart = false;
        qteActive = false;
        completed = new bool[]{false, false};
        frameCountPerAnimation[0] = 1;
        frameCountPerAnimation[1] = 3;
        frameCountPerAnimation[2] = 3;

        Texture playerTexture = AssetManager.Instance.Textures["hookwbait"];
        sprite = new Sprite(playerTexture);
        sprite.TextureRect = new IntRect(
            0, 0,
            (int)(playerTexture.Size.X / PLAYER_TILING_X),
            (int)(playerTexture.Size.Y / PLAYER_TILING_Y)
        );

        textureSize = new Vector2f(sprite.Texture.Size.X/PLAYER_TILING_X, sprite.Texture.Size.Y/PLAYER_TILING_Y);
        sprite.Scale *= scale;
        Vector2f center = textureSize * 0.5f;
        sprite.Origin = center;
        CollisionBox = new FloatRect(sprite.Position.X, sprite.Position.Y, textureSize.X * scale * hitboxscale, textureSize.Y*scale * hitboxscale);

        qte = new FishQTE();
        qte.Initialize();
        qteActive = false;

        MoveTimer = ValueParser.ParseGameState(5) * 15;

        CoughtFish = null;

        Texture SplashTexture = AssetManager.Instance.Textures["Splash"];
        splash = new Sprite(SplashTexture);
        splash.TextureRect = new IntRect(
            1000, 0,
            (int)(SplashTexture.Size.X/SPLASH_TILING_X),
            (int)SplashTexture.Size.Y
        );
        splash.Origin = new Vector2f(splash.TextureRect.Width, splash.TextureRect.Height) /2;
        splash.Scale *= 4;

        sprite.Position = new Vector2f(0, 0);
    }

    public override void Update(float deltaTime)
    {
        SplashUpdate(deltaTime);
        animationTime += deltaTime * animationSpeed;
        
        if(MoveStarted() && !collided){
            Move(deltaTime);

        }else if (!completed[0] && collided){
            if(collided && !qteActive){
                PlaySound("Bite");
                CoughtFish.SetPositon(sprite.Position + new Vector2f(0, sprite.TextureRect.Height).Normalize() * CoughtFish.GetScale()*2);
                CoughtFish.SetRotation(GetHookPosition()-sprite.Position);
            }
            
            qteActive = true;
            qte.SetPositon(sprite.Position);
            qte.Update(deltaTime);
            currentAnimation = AnimationType.Down;

            if(qte.GetStatus() == 0){
                completed = new bool[] {true, false};  
                CoughtFish = null;
            }else if(qte.GetStatus() == 1){
                completed = new bool[] {true, true};
            }else if(qte.GetStatus() == 2){
                completed = new bool[] {false, false};
            }

        }else{
            qteActive = false;
            if(completed[0]){
                if(Utils.Distance(sprite.Position, returnPoint) > 20){
                    sprite.Position += (returnPoint -sprite.Position).Normalize() * 1000 * deltaTime;
                    currentAnimation = AnimationType.Down;

                }else{
                    restart = true;

                    if(CoughtFish != null){
                        int value = (int)(CoughtFish.GetValue()*20);
                        ValueParser.WriteGamestate(2, value);
                    }else{
                        ValueParser.WriteGamestate(2, 0);
                    }
                }
            }
        }

        int animationFrame = (int)animationTime % frameCountPerAnimation[(int)currentAnimation];

        int spriteOffsetX = animationFrame * sprite.TextureRect.Width;
        int spriteOffsetY = (int)currentAnimation * sprite.TextureRect.Height;

        sprite.TextureRect = new IntRect(
            spriteOffsetX,
            spriteOffsetY,
            sprite.TextureRect.Width,
            sprite.TextureRect.Height
        );
        if(CoughtFish!=null && completed[1]){

            if(CoughtFish is BiggestFish b){
                b.Update(deltaTime);
            }

            CoughtFish.SetPositon(sprite.Position + new Vector2f(0, sprite.TextureRect.Height).Normalize() * CoughtFish.GetScale()*2);
            CoughtFish.SetRotation(GetHookPosition()-sprite.Position);

        }
        

        CollisionBox.Left = sprite.Position.X - textureSize.X * 0.5f * scale * hitboxscale;
        CollisionBox.Top = sprite.Position.Y - textureSize.Y * 0.5f * scale * hitboxscale;

    }
    private bool MoveStarted(){

        if(!(startmovement) && InputManager.Instance.GetKeyPressed(Keyboard.Key.Space)){
            startmovement = true;
            move = new Vector2f(10, -8) * MoveScale;
        }
        return startmovement;
    }

    private void Move(float dt){
        MoveTimer -= dt;
        if(MoveTimer >= 0){

            if(MoveCooldown <= 0 && sprite.Position.Y >= 100){
                if(InputManager.Instance.GetKeyDown(Keyboard.Key.A)){
                    move = new Vector2f(-4, -2) * MoveScale;
                    MoveCooldown = (float)0.5;
                    currentAnimation = AnimationType.Left;            
                    }
                if(InputManager.Instance.GetKeyDown(Keyboard.Key.D)){
                    move = new Vector2f(4, -2) * MoveScale;
                    MoveCooldown = (float)0.5;
                    currentAnimation = AnimationType.Right; 
                }
                if(InputManager.Instance.GetKeyDown(Keyboard.Key.S)){
                    move = new Vector2f(0, -5) * MoveScale;
                    MoveCooldown = (float)0.5;
                    currentAnimation = AnimationType.Down; 
                }

            }else{
                MoveCooldown -= dt;
            }
        }
        CheckCameraCollision();

        if(move.X == 0){
            //do nothing, but skip other checks
            currentAnimation = AnimationType.Down;

        }else if((int)move.X < 0){
            move += new Vector2f(1, 0) * dt * MoveScale * 2;
        }else if ((int)move.X>0){
            move += new Vector2f(-1, 0) * dt * MoveScale * 4;
        }else{
            move.X = 0;
        }

        if((int)move.Y <= 0){
            move += new Vector2f(0, 1) * dt * MoveScale * 5;
        }else{
            move += new Vector2f(0, 1) * dt * MoveScale * 0.6f * weight/90;
        }

        if(move.Y > weight  && sprite.Position.Y > 70){
            move.Y = weight;
        }
        if(sprite.Position.Y <= 70){
            move += new Vector2f(0, 1) * dt * MoveScale * 10;
        }
        sprite.Position += move * dt;
    }

    private void CheckCameraCollision(){
        if(sprite.Position.X <= (view.Center).X - view.Size.X/2){
            if(move.X < 0){
                move.X = 0;
            }
        }else if(sprite.Position.X + sprite.Scale.X*sprite.Texture.Size.X/PLAYER_TILING_X>= (view.Center).X + view.Size.X/2){
            if(move.X > 0){
                move.X = 0;
            }
        } 
    }

    public override void Draw(RenderWindow window)
    {

        view = window.GetView();
        
        if(sprite.Position.Y <100 && CameramoveX){
            CameraX = sprite.Position.X;
        }
        if(sprite.Position.Y >0 && CameramoveX){
            CameramoveX = false;
        }

        if(!completed[0] && !qteActive){
            view.Center = new Vector2f(CameraX, sprite.Position.Y);
        }else{
            view.Center = sprite.Position;
        }
        window.SetView(view);

        if(!((completed[1] && completed[0])||qteActive)){

            window.Draw(sprite);
        }
    
        if(collided && !completed[0]){
            qte.Draw(window);
        }

        window.Draw(splash);
    }

    public Vector2f GetPosition()
    {
        return sprite.Position;
    }
    public Vector2f GetHookPosition()
    {

        float EmptySpaceComp = 1.7f;
        Vector2f pos = sprite.Position - (textureSize * scale / 2)/EmptySpaceComp + new Vector2f(0, weight / 100);
        if(currentAnimation == AnimationType.Left){
            return pos;
        }
        else if (currentAnimation == AnimationType.Right){
            return new Vector2f (pos.X + textureSize.X * scale/EmptySpaceComp, pos.Y);
        }
        else{
            return new Vector2f (pos.X + textureSize.X/2 * scale/EmptySpaceComp, pos.Y);
        }
    }
    public bool[] GetCompleted(){
        return completed;
    }
    public bool GetQTEstatus(){
        return qteActive;
    }
    public void SetReturnPoint(Vector2f _returnPoint){
        returnPoint = _returnPoint;
    }
    public float GetMoveTimer(){
        return MoveTimer;
    }
    public void SetCoughtFish(Fish fish){
        CoughtFish = fish;
    }

    private void SplashUpdate(float deltaTime){
        if(sprite.Position.Y >= 25 && sprite.Position.Y <= 30 && !splashAnim){
            splash.Position = sprite.Position;
            splashAnim = true;
            if(!completed[0]){
                PlaySound("Splash");
            }
            
        }
        if(splashAnim){
            splashAnimTime += deltaTime * splashAnimSpeed;
            int SplashOffsetX = (int)(splashAnimTime % SPLASH_TILING_X);
            splash.TextureRect = new IntRect(
                SplashOffsetX * splash.TextureRect.Width, 0,
                splash.TextureRect.Width,
                splash.TextureRect.Height
            );

            if(SplashOffsetX >= SPLASH_TILING_X -1 || splash.Position.Y <20){
                splashAnim = false;
                splashAnimTime = 0;
                splash.TextureRect = new IntRect(
                (int)SPLASH_TILING_X * splash.TextureRect.Width, 0,
                splash.TextureRect.Width,
                splash.TextureRect.Height
            );
            }
        }
    }

    private void PlaySound(string SoundName){
        if(ValueParser.ParseSettings(1) != 0){
            SoundBuffer buffer  = AssetManager.Instance.SoundBuffers[SoundName];
            Sound sound = new(buffer);
            sound.Play();
        }
    }

}