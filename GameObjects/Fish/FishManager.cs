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
        if(timeSinceSpawn>= 10){
            FishBool = true;
            timeSinceSpawn = 0;
        }else{
            timeSinceSpawn += deltaTime;
            if(random.Next(1, 1000) >990){
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
            int FishNum = random.Next(1, 4);
            float spawnX = center.X - area.X/2 + random.Next(1,2) % 2 * area.X;
            float spawnY = center.Y + random.Next(-(int)area.Y/2, (int)area.Y/2); 
            Vector2f pos = new Vector2f (spawnX, spawnY);
        switch (FishNum){
            case 1:
                BulletFish BulletFish = new BulletFish();
                BulletFish.Initialize();
                BulletFish.Position = pos;
                FishList.Add(BulletFish);
                Console.WriteLine("Bullet");
                break;
            case 2:
                NormalFish NormalFish = new NormalFish();
                NormalFish.Initialize();
                NormalFish.Position = pos;
                FishList.Add(NormalFish);
                Console.WriteLine("Normal");
                break;
            case 3:
                DriftFish DriftFish = new DriftFish();
                DriftFish.Initialize();
                DriftFish.Position = pos;
                FishList.Add(DriftFish);
                Console.WriteLine("Drift");
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
                Console.Write("bam");
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