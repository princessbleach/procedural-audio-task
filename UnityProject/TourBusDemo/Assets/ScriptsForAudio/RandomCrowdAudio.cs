
using UnityEngine;
using AK.Wwise;
using System.Collections;

public class RandomCrowdAmbience : MonoBehaviour
{
    public AK.Wwise.Event crowdEvent;

    public float minSilence = 15f;
    public float maxSilence = 30f;

    public float fadeInTime = 4f;
    public float audibleTime = 25f;
    public float fadeOutTime = 6f;

    private IEnumerator Start()
    {
        while (true)
        {
            // Schedule the next crowd event.
            float silence = Random.Range(minSilence, maxSilence);

            if (AmbienceMonitorUI.Instance != null)
            {
                AmbienceMonitorUI.Instance.SetStatus(
                    "Crowd", "WAITING"
                );

                AmbienceMonitorUI.Instance.SetNextEvent(
                    "Crowd", silence
                );
            }

            yield return new WaitForSeconds(silence);

            // Reset the crowd volume before playback.
            AkSoundEngine.SetRTPCValue("CrowdVolume", 0f);

            // Post the crowd event.
            crowdEvent.Post(gameObject);

            if (AmbienceMonitorUI.Instance != null)
            {
                AmbienceMonitorUI.Instance.SetStatus(
                    "Crowd", "FADING IN"
                );

                AmbienceMonitorUI.Instance.SetNextEvent(
                    "Crowd", -1f
                );
            }

            yield return FadeVolume(0f, 100f, fadeInTime);

            if (AmbienceMonitorUI.Instance != null)
            {
                AmbienceMonitorUI.Instance.SetStatus(
                    "Crowd", "PLAYING"
                );
            }

            yield return new WaitForSeconds(audibleTime);

            if (AmbienceMonitorUI.Instance != null)
            {
                AmbienceMonitorUI.Instance.SetStatus(
                    "Crowd", "FADING OUT"
                );
            }

            yield return FadeVolume(100f, 0f, fadeOutTime);

            if (AmbienceMonitorUI.Instance != null)
            {
                AmbienceMonitorUI.Instance.SetStatus(
                    "Crowd", "WAITING"
                );
            }
        }
    }

    private IEnumerator FadeVolume(
        float start,
        float end,
        float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float value = Mathf.Lerp(
                start, end, elapsed / duration
            );

            AkSoundEngine.SetRTPCValue("CrowdVolume", value);

            yield return null;
        }

        AkSoundEngine.SetRTPCValue("CrowdVolume", end);
    }
}
