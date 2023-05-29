using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

public class BubbleManager : GameObject
{
    List<Bubble> BubbleList;
    List<Fish> FishList;
    Random random;
    public override void Initialize()
    {
        BubbleList = new List<Bubble>();
        FishList = new();
    }
    public override void Update(float deltaTime, InputManager inputman)
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
            BubbleList[bubbleNum].Update(deltaTime, inputman);
            if(BubbleList[bubbleNum].GetTimeToLive() < 0){
                BubbleList.RemoveAt(bubbleNum);
            }
        }
    }
    public override void Draw(RenderWindow window, RenderStates states, bool shaderon)
    {
        foreach(Bubble bubble in BubbleList){
            bubble.Draw(window, states, shaderon);
        }
    }
    public void SetFish(List<Fish> _FishList){
        FishList = _FishList;
    }
}