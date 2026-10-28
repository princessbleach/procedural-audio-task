
using UnityEngine;
using AK.Wwise;
using System.Collections;

public class RandomClubMusic : MonoBehaviour
{
    public AK.Wwise.Event clubEvent;

    public float minSilence = 25f;
    public float maxSilence = 45f;

    public float minPlayTime = 15f;
    public float maxPlayTime = 30f;

    private IEnumerator Start()
    {
        while (true)
        {
            // Schedule the next club music event.
            float silence = Random.Range(minSilence, maxSilence);

            if (AmbienceMonitorUI.Instance != null)
            {
                AmbienceMonitorUI.Instance.SetStatus(
                    "Club Music", "WAITING"
                );

                AmbienceMonitorUI.Instance.SetNextEvent(
                    "Club Music", silence
                );
            }

            // Wait before the club music starts.
            yield return new WaitForSeconds(silence);

            // Play the club music.
            uint playingId = clubEvent.Post(gameObject);

            if (AmbienceMonitorUI.Instance != null)
            {
                AmbienceMonitorUI.Instance.SetStatus(
                    "Club Music", "PLAYING"
                );

                AmbienceMonitorUI.Instance.SetNextEvent(
                    "Club Music", -1f
                );
            }

            // Let it play for a random duration.
            float playTime = Random.Range(minPlayTime, maxPlayTime);

            yield return new WaitForSeconds(playTime);

            // Stop the music with a fade-out.
            AkSoundEngine.ExecuteActionOnPlayingID(
                AkActionOnEventType.AkActionOnEventType_Stop,
                playingId,
                2
            );

            if (AmbienceMonitorUI.Instance != null)
            {
                AmbienceMonitorUI.Instance.SetStatus(
                    "Club Music", "WAITING"
                );
            }
        }
    }
}

