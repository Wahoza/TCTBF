//Author: Kilian Kolb
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class BubbleManager : GameObject
{
    List<Bubble> BubbleList = new List<Bubble>();
    List<Fish> FishList = new List<Fish>();
    Random random = new();
    public override void Initialize()
    {
        BubbleList = new List<Bubble>();
        FishList = new();
    }
    public override void Update(float deltaTime)
    {
        foreach(Fish fish in FishList){
            random = new Random();
            
            if(random.Next(1, 1000) > 995){
                Bubble bubble = new Bubble();
                bubble.Initialize();
                bubble.SetPositon(fish.GetPosition());
                BubbleList.Add(bubble);
            }
        }
        for(int bubbleNum = 0; bubbleNum < BubbleList.Count(); bubbleNum++){
            BubbleList[bubbleNum].Update(deltaTime);
            if(BubbleList[bubbleNum].GetTimeToLive() < 0){
                BubbleList.RemoveAt(bubbleNum);
            }
        }
    }
    public override void Draw(RenderWindow window)
    {
        foreach(Bubble bubble in BubbleList){
            bubble.Draw(window);
        }
    }
    public void SetFish(List<Fish> _FishList){
        FishList = _FishList;
    }
}