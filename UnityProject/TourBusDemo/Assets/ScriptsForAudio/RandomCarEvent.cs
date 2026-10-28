
using UnityEngine;
using AK.Wwise;

public class RandomWwiseEvent : MonoBehaviour
{
    public AK.Wwise.Event soundEvent;

    public float minimumInterval = 8f;
    public float maximumInterval = 20f;

    // Choose which dashboard row this sound updates.
    public string monitorLayerName = "Car Drive-By";

    private float timer;

    private void Start()
    {
        ScheduleNextSound();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Play the Wwise car sound.
            soundEvent.Post(gameObject);

            // Notify the correct dashboard row.
            if (AmbienceMonitorUI.Instance != null)
            {
                AmbienceMonitorUI.Instance.SetStatus(
                    monitorLayerName, "TRIGGERED", 1.2f
                );
            }

            // Schedule the next sound.
            ScheduleNextSound();
        }
    }

    private void ScheduleNextSound()
    {
        timer = Random.Range(minimumInterval, maximumInterval);

        // Update the countdown for the correct dashboard row.
        if (AmbienceMonitorUI.Instance != null)
        {
            AmbienceMonitorUI.Instance.SetNextEvent(
                monitorLayerName, timer
            );
        }
    }
}
