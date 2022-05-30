using System.Collections;
using UnityEngine;

public class CamSlowMotionDelay : MonoBehaviour, ISlowMotionDelay<float, float>
{

	public void StartSlowMotionDelay(float duration, float scale){
		StopAllCoroutines();
		StartCoroutine(SlowMotionRoutine(duration, scale));
	}

	//slow motion delay
	IEnumerator SlowMotionRoutine(float duration, float scale) {

		//set timescale
		Time.timeScale = scale;

		//wait a moment...
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < (startTime + duration)) {
			yield return null;
		}

        //reset timescale
        Time.timeScale = 1;
    }
}

public interface ISlowMotionDelay<T1, T2>
{
	void StartSlowMotionDelay(float duration, float scale);
}