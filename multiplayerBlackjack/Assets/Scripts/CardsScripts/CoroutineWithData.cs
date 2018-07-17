using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Custom code to execute coroutines and get values from coroutine.
 */

public class CoroutineWithData {

    public Coroutine coroutine { get; private set; }
    public CardModel result;
    private IEnumerator target;
    public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = (CardModel) target.Current;
            yield return result;
        }
    }
}
