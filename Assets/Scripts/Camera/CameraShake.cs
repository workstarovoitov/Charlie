using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour, IShake<HitObject>
{
	public static CameraShake Instance { get; private set; }

	private CinemachineVirtualCamera cinemachineVC;


    private void Awake()
    {
		Instance = this;
		cinemachineVC = GetComponent<CinemachineVirtualCamera>();
    }
    public void Shake(HitObject settings)
	{
		CinemachineBasicMultiChannelPerlin cinemachineMCP = cinemachineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		cinemachineMCP.m_AmplitudeGain = settings.cameraShakeAmplitude;
		cinemachineMCP.m_FrequencyGain = settings.cameraShakeFrequency;
		StopAllCoroutines();
		StartCoroutine(DoShake(settings.cameraShakeDuration));
	}

	IEnumerator DoShake(float duration)
	{
		float timer = duration;
		CinemachineBasicMultiChannelPerlin cinemachineMCP = cinemachineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		while (timer > 0)
		{
			cinemachineMCP.m_AmplitudeGain = Mathf.Lerp(cinemachineMCP.m_AmplitudeGain, 0f, 1 - (timer / duration));

			timer -= Time.unscaledDeltaTime;
			yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
		}
		cinemachineMCP.m_AmplitudeGain = 0;
	}
}

public interface IShake<T>
{
	void Shake(T settings);
}