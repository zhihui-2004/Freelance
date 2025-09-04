using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class WaitExtension
{
   public static void Wait(this MonoBehaviour mono, float delay, UnityAction action)
   {
        mono.StartCoroutine(ExectuteAction(delay, action));
   }

    private static IEnumerator ExectuteAction(float delay, UnityAction action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action.Invoke();
        yield break;
    }
}
