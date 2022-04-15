using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum PickUpType
{
    Coin,
    Health,
    LevelGoal
}

public class Pickable : MonoBehaviour
{

    public PickUpType pickUpType;
    public int value;
    public UnityEvent onPick;
    public Collider2D col;
    public AnimationCurve scaleCurve;
    

    public void PickUp()
    {
        onPick?.Invoke();
    }

    public void Effect_PickUp(float time)
    {
        StartCoroutine(Effect_PickUp_Co(time));
    }

    IEnumerator Effect_PickUp_Co(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            transform.localScale = scaleCurve.Evaluate(t / time) * Vector3.one;
            yield return null;
        }
        Destroy();
    }
    
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
