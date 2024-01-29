using DOTweenModular;
using UnityEngine;

public class tester : MonoBehaviour
{
    private DOMove doMove;

    private void OnEnable()
    {
        doMove = GetComponent<DOMove>();

        doMove.onTweenCreated.AddListener(OnTweenCreated);
        doMove.onTweenPlayed.AddListener(OnTweenPlayed);
        doMove.onTweenUpdated.AddListener(OnTweenUpdated);
        doMove.onTweenCompleted.AddListener(OnTweenCompleted);
        doMove.onTweenKilled.AddListener(OnTweenKilled);
    }

    private void OnDestroy()
    {
        doMove.onTweenCreated.RemoveListener(OnTweenCreated);
        doMove.onTweenPlayed.RemoveListener(OnTweenPlayed);
        doMove.onTweenUpdated.RemoveListener(OnTweenUpdated);
        doMove.onTweenCompleted.RemoveListener(OnTweenCompleted);
        doMove.onTweenKilled.RemoveListener(OnTweenKilled);
    }

    public void OnTweenCreated()
    {
        print(gameObject.name + " Created");
    }
    public void OnTweenPlayed()
    {
        print(gameObject.name + " Started");
    }
    public void OnTweenUpdated()
    {
        print(gameObject.name + " Updating");
    }
    public void OnTweenCompleted()
    {
        print(gameObject.name + " Completed");
    }
    public void OnTweenKilled()
    {
        print(gameObject.name + " Killed"); 
    }
}
