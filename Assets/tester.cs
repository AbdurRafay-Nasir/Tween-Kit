using DOTweenModular;
using UnityEngine;

public class tester : MonoBehaviour
{
    private DOMove doMove;

    private void Awake()
    {
        doMove = GetComponent<DOMove>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            doMove.CreateTween();
            doMove.PlayTween();
        }
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
