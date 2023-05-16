using SFML.Graphics;
using SFML.System;

public class FishManager : Fish
{
    List<Fish> FishList;
    float timeSinceSpawn = 0;
    Random random = new Random();
    bool FishBool;
    public override void Initialize(AssetManager assetman)
    {
        FishList = new List<Fish>();
    }

    public override void Update(float deltaTime, InputManager inputman)
    {
        if(timeSinceSpawn>= 10){
            FishBool = true;
        }else{
            timeSinceSpawn += deltaTime;
            if(random.Next(1, 100) >98){
                FishBool = true;
            }else{
                FishBool = false;
            }
        }

        foreach(Fish fish in FishList){
            fish.Update(deltaTime, inputman);
        }
    }

    public void SpawnFish(bool started, Vector2f area, Vector2f center){
        if(started && FishBool){
            int FishNum = random.Next(1, 3);
            float spawnX = center.X - area.X/2 + random.Next(1,2) % 2 * area.X;
            float spawnY = center.Y + random.Next(-(int)area.Y/2, (int)area.Y/2); 
            Vector2f pos = new Vector2f (spawnX, spawnY);
        switch (FishNum){
            case 1:
                BulletFish BulletFish = new BulletFish();
                BulletFish.Position = pos;
                FishList.Add(BulletFish);
                break;
            case 2:
                NormalFish NormalFish = new NormalFish();
                NormalFish.Position = pos;
                FishList.Add(NormalFish);
                break;
            case 3:
                DriftFish DriftFish = new DriftFish();
                DriftFish.Position = pos;
                FishList.Add(DriftFish);
                break;
            default:
                break;
        }
        }
    }
    public override void Draw(RenderWindow window)
    {
        foreach(Fish fish in FishList){
            fish.Draw(window);
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
}