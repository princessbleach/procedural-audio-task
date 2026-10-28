
using UnityEngine;
using System.Collections.Generic;

public class AmbienceMonitorUI : MonoBehaviour
{
    public static AmbienceMonitorUI Instance { get; private set; }

    // Dark pink and black colour palette
    private readonly Color backgroundColour = new Color(0.025f, 0.015f, 0.025f);
    private readonly Color panelColour = new Color(0.09f, 0.035f, 0.065f);
    private readonly Color hotPink = new Color(1f, 0.20f, 0.55f);
    private readonly Color softPink = new Color(1f, 0.55f, 0.75f);
    private readonly Color lightPink = new Color(1f, 0.78f, 0.87f);
    private readonly Color mutedPink = new Color(0.68f, 0.40f, 0.53f);
    private readonly Color playingPink = new Color(1f, 0.45f, 0.72f);
    private readonly Color triggeredPink = new Color(1f, 0.30f, 0.45f);

    private class AmbienceLayer
    {
        public string name;
        public string status = "WAITING";
        public float nextEventTime = -1f;
        public float statusResetTime = -1f;
        public float fadeProgress;
        public bool showFade;
    }

    private readonly Dictionary<string, AmbienceLayer> layers =
        new Dictionary<string, AmbienceLayer>();

    private readonly List<string> layerOrder = new List<string>();

    private GUIStyle titleStyle;
    private GUIStyle headingStyle;
    private GUIStyle layerStyle;
    private GUIStyle statusStyle;
    private GUIStyle footerStyle;
    private GUIStyle fadeStyle;

    private void Awake()
    {
        Instance = this;

        AddLayer("City Atmosphere");
        AddLayer("Car Rev");
        AddLayer("Car Drive-By");
        AddLayer("Crowd");
        AddLayer("Club Music");

        SetStatus("City Atmosphere", "PLAYING");
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void AddLayer(string name)
    {
        layers[name] = new AmbienceLayer { name = name };
        layerOrder.Add(name);
    }

    public void SetStatus(
        string layerName,
        string status,
        float displayDuration = 0f)
    {
        if (!layers.TryGetValue(layerName, out AmbienceLayer layer))
            return;

        layer.status = status;
        layer.statusResetTime = displayDuration > 0f
            ? Time.time + displayDuration
            : -1f;
    }

    public void SetNextEvent(string layerName, float seconds)
    {
        if (!layers.TryGetValue(layerName, out AmbienceLayer layer))
            return;

        // A negative value clears the countdown.
        layer.nextEventTime = seconds < 0f
            ? -1f
            : Time.time + seconds;
    }

    public void SetFadeProgress(string layerName, float progress)
    {
        if (!layers.TryGetValue(layerName, out AmbienceLayer layer))
            return;

        layer.fadeProgress = Mathf.Clamp01(progress);
        layer.showFade = true;
    }

    private void Update()
    {
        foreach (AmbienceLayer layer in layers.Values)
        {
            if (layer.statusResetTime >= 0f &&
                Time.time >= layer.statusResetTime)
            {
                layer.status = "WAITING";
                layer.statusResetTime = -1f;
            }
        }
    }

    private void InitialiseStyles()
    {
        titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 32,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        headingStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 18,
            fontStyle = FontStyle.Bold
        };

        layerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 22,
            alignment = TextAnchor.MiddleLeft
        };

        statusStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 18,
            alignment = TextAnchor.MiddleRight,
            fontStyle = FontStyle.Bold
        };

        footerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter
        };

        fadeStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold
        };
    }

    private void DrawBar(Rect rect, float progress)
    {
        // Dark pink bar background
        GUI.color = new Color(0.16f, 0.045f, 0.09f);
        GUI.DrawTexture(rect, Texture2D.whiteTexture);

        // Pink progress fill
        GUI.color = softPink;
        GUI.DrawTexture(
            new Rect(
                rect.x,
                rect.y,
                rect.width * Mathf.Clamp01(progress),
                rect.height
            ),
            Texture2D.whiteTexture
        );

        GUI.color = Color.white;
    }

    private Color GetStatusColor(string status)
    {
        if (status == "PLAYING")
            return playingPink;

        if (status == "TRIGGERED")
            return triggeredPink;

        if (status == "FADING IN" || status == "FADING OUT")
            return softPink;

        return mutedPink;
    }

    private void OnGUI()
    {
        if (titleStyle == null)
            InitialiseStyles();

        // Full-screen near-black background
        GUI.color = backgroundColour;
        GUI.DrawTexture(
            new Rect(0, 0, Screen.width, Screen.height),
            Texture2D.whiteTexture
        );

        GUI.color = Color.white;

        float margin = Screen.width * 0.07f;
        float contentWidth = Screen.width - margin * 2f;

        // Main title
        GUI.color = hotPink;
        GUI.Label(
            new Rect(margin, Screen.height * 0.04f,
                contentWidth, 50),
            "AMBIENCE MONITOR",
            titleStyle
        );

        // Subtitle
        GUI.color = softPink;
        GUI.Label(
            new Rect(margin, Screen.height * 0.12f,
                contentWidth, 35),
            "PROCEDURAL AUDIO SYSTEM  /  UNITY + WWISE",
            footerStyle
        );

        float startY = Screen.height * 0.24f;

        // Column headings
        GUI.color = lightPink;
        GUI.Label(
            new Rect(margin, startY, contentWidth * 0.40f, 35),
            "AUDIO LAYER",
            headingStyle
        );

        GUI.Label(
            new Rect(margin + contentWidth * 0.43f,
                startY, contentWidth * 0.25f, 35),
            "STATUS",
            headingStyle
        );

        GUI.Label(
            new Rect(margin + contentWidth * 0.73f,
                startY, contentWidth * 0.27f, 35),
            "NEXT EVENT",
            headingStyle
        );

        float rowY = startY + 42f;
        float rowHeight = Screen.height * 0.105f;

        foreach (string key in layerOrder)
        {
            AmbienceLayer layer = layers[key];

            // Black-plum panel for each audio layer
            GUI.color = panelColour;
            GUI.DrawTexture(
                new Rect(margin, rowY, contentWidth, rowHeight - 6f),
                Texture2D.whiteTexture
            );

            // Pink accent strip along the left edge
            GUI.color = hotPink;
            GUI.DrawTexture(
                new Rect(margin, rowY, 4f, rowHeight - 6f),
                Texture2D.whiteTexture
            );

            GUI.color = lightPink;

            float labelY = rowY + 4f;

            GUI.Label(
                new Rect(margin + 18f, labelY,
                    contentWidth * 0.40f, 38f),
                layer.name,
                layerStyle
            );

            // Status text
            GUI.color = GetStatusColor(layer.status);
            GUI.Label(
                new Rect(margin + contentWidth * 0.43f,
                    labelY, contentWidth * 0.25f, 38f),
                layer.status,
                statusStyle
            );

            // Event countdown
            GUI.color = softPink;

            string countdown = layer.nextEventTime < 0f
                ? "--"
                : Mathf.Max(
                    0f, layer.nextEventTime - Time.time
                ).ToString("F1") + " s";

            GUI.Label(
                new Rect(margin + contentWidth * 0.73f,
                    labelY, contentWidth * 0.25f, 38f),
                countdown,
                statusStyle
            );

            // Crowd fade progress bar
            if (layer.name == "Crowd" && layer.showFade)
            {
                float barY = rowY + rowHeight * 0.62f;

                Rect barRect = new Rect(
                    margin + 18f,
                    barY,
                    contentWidth * 0.64f,
                    12f
                );

                DrawBar(barRect, layer.fadeProgress);

                GUI.color = lightPink;
                GUI.Label(
                    new Rect(
                        barRect.x + barRect.width + 12f,
                        barY - 5f,
                        80f,
                        22f
                    ),
                    Mathf.RoundToInt(
                        layer.fadeProgress * 100f
                    ) + "%",
                    fadeStyle
                );
            }

            rowY += rowHeight;
        }

        // Footer
        GUI.color = mutedPink;
        GUI.Label(
            new Rect(margin, Screen.height - 55f,
                contentWidth, 30f),
            "LIVE DEBUG VIEW  |  PROCEDURAL AMBIENCE",
            footerStyle
        );

        GUI.color = Color.white;
    }
}
