using UnityEngine;

public class tester : MonoBehaviour
{
    public void OnTweenCreated()
    {
        print("Created");
    }
    public void OnTweenStarted()
    {
        print("Started");
    }
    public void OnTweenCompleted()
    {
        print("Completed");
    }
    public void OnTweenKilled()
    {
        print("Killed");
    }
}
