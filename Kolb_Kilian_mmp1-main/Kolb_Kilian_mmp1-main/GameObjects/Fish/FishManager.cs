using SFML.Graphics;
using SFML.System;

public class FishManager : Fish
{
    List<Fish> FishList;
    float timeSinceSpawn = 0;
    Random random = new Random();
    bool FishBool;
    public override void Initialize()
    {
        FishList = new List<Fish>();
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        int n = random.Next(1, 1000);
        if(n >985){
            FishBool = true;
        }else{
            FishBool = false;
        }
        foreach(Fish fish in FishList){
            fish.Update(deltaTime, inputman);
        }
    }

    public void SpawnFish(bool started, Vector2f area, Vector2f center){
        if(started && FishBool){
            int FishNum = random.Next(1, 4);
            float spawnX = 0;
            float spawnY = 0;
            if(random.Next(1,3)%2 == 0){
                spawnX = center.X - area.X/2 + random.Next(1,3) % 2 * area.X;
                spawnY = center.Y + random.Next(-(int)area.Y/2, (int)area.Y/2); 
            }else{
                if (center.Y + area.Y/2 > 0){
                    spawnY = center.Y - area.Y/2 + area.Y;
                }else{
                    spawnY = center.Y - area.Y/2 + random.Next(1,3) % 2 * area.Y;
                }
                spawnX = center.X + random.Next(-(int)area.X/2, (int)area.X/2);
            }
            if(spawnY < 30){
                spawnY = 30;
            }
            Vector2f pos = new Vector2f (spawnX, spawnY);
            
        switch (FishNum){
            case 1:
                BulletFish BulletFish = new BulletFish();
                BulletFish.Initialize();
                BulletFish.SetPositon(pos);
                FishList.Add(BulletFish);
                break;
            case 2:
                NormalFish NormalFish = new NormalFish();
                NormalFish.Initialize();
                NormalFish.SetPositon(pos);
                FishList.Add(NormalFish);
                break;
            case 3:
                DriftFish DriftFish = new DriftFish();
                DriftFish.Initialize();
                DriftFish.SetPositon(pos);
                FishList.Add(DriftFish);
                break;
            default:
                break;
        }
        }
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        foreach(Fish fish in FishList){
            fish.Draw(window, states, shaderon);
        }
    }

    public override int ColisionCheck(FloatRect player)
    {
        foreach(Fish fish in FishList){
            if(fish.ColisionCheck(player) > 0){
                return fish.ColisionCheck(player);
            }
        }
        return 0;
    }

    public override void MoveToGoal(Vector2f player)
    {
        foreach(Fish fish in FishList){
            fish.MoveToGoal(player);
        }
    }
    public override void SetPositon(Vector2f position)
    {

    }
}