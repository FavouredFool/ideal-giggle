using System.Collections;
using UnityEngine;

public class PathfinderCoroutine
{
    public Coroutine coroutine { get; private set; }
    public object result;
    private IEnumerator target;

    public PathfinderCoroutine(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }
}
