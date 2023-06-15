using System.Collections;
using UnityEngine;

namespace My
{
    public class Move : MonoBehaviour
    {
        public delegate void CoroutineEndAction();
        public static event CoroutineEndAction OnCoroutineEnd;



        public static IEnumerator MoveToTargetByTime(Transform target, Vector3 startPosition, float speed)
        {
            float journeyLength = Vector3.Distance(startPosition, target.position);
            float journeyTime = journeyLength / speed;
            float elapsedTime = 0f;

            while (elapsedTime < journeyTime)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / journeyTime);
                Vector3 newPosition = Vector3.Lerp(startPosition, target.position, t);
                target.position = newPosition;
                yield return null;
            }

            // Ensure reaching the exact target position
            target.position = target.position;

        }

        public static bool CoroutineEnded = true;
        public static IEnumerator MoveToTarget(Transform target, Vector3 startPosition, Vector3 targetPosition, float speed)
        {
            float journeyLength = Vector3.Distance(startPosition, targetPosition);
            float startTime = Time.time;

            while (target.position != targetPosition)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;
                target.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

                yield return null;
            }

            CoroutineEnded = true;
            //OnCoroutineEnd?.Invoke();
        }

        public static bool IsEnded()
        {
            if (CoroutineEnded)
            {
                CoroutineEnded = false;
                return true;
            }
            else return false;
        }
    }

}