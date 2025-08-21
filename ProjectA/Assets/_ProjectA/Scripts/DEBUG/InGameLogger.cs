using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _BazookaBrawl.Scripts.Debugging
{
    public class InGameLogger : MonoBehaviour
    {
        
        [FormerlySerializedAs("maxMessages")]
        [Header("UI Settings")]
        [SerializeField] private int _maxMessages = 50; // Maximum number of log messages to display
        [SerializeField] private Rect _windowRect = new Rect(10, 10, 600, 300); // Position and size of the log window
        [SerializeField] private bool _enableDragging = true; // Allow dragging the log window
        [SerializeField] private KeyCode _toggleKey = KeyCode.F1; // Key to toggle log visibility

        private List<string> logMessages = new List<string>();
        private Vector2 scrollPosition;
        private bool showLog = true;

        private void OnEnable()
        {
            // Subscribe to Unity's log message callback
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            // Unsubscribe from Unity's log message callback
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            // Format the log message
            string formattedMessage = $"[{type}] {logString}";
            if (type == LogType.Exception || type == LogType.Error)
            {
                formattedMessage += $"\n{stackTrace}";
            }

            // Add the log message to the list
            logMessages.Add(formattedMessage);

            // Remove the oldest message if exceeding maxMessages
            if (logMessages.Count > _maxMessages)
            {
                logMessages.RemoveAt(0);
            }
        }

        private void OnGUI()
        {
            if (!showLog) return;

            // Make the window draggable
            if (_enableDragging)
            {
                _windowRect = GUI.Window(0, _windowRect, DrawLogWindow, "In-Game Logger");
            }
            else
            {
                DrawLogWindow(0);
            }
        }

        private void DrawLogWindow(int windowID)
        {
            // Start scroll view
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(_windowRect.width - 20), GUILayout.Height(_windowRect.height - 40));

            // Display all log messages
            foreach (string message in logMessages)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                if (message.StartsWith("[Error]") || message.StartsWith("[Exception]"))
                    style.normal.textColor = Color.red;
                else if (message.StartsWith("[Warning]"))
                    style.normal.textColor = Color.yellow;
                else
                    style.normal.textColor = Color.white;

                GUILayout.Label(message, style);
            }

            GUILayout.EndScrollView();

            // Allow dragging the window
            if (_enableDragging)
                GUI.DragWindow(new Rect(0, 0, _windowRect.width, 20));
        }

        private void Update()
        {
            // Toggle log visibility with the specified key
            if (Input.GetKeyDown(_toggleKey))
            {
                showLog = !showLog;
            }
            if(Input.GetKeyDown(KeyCode.P))
                Application.logMessageReceived -= HandleLog;
            if(Input.GetKeyUp(KeyCode.O))
                Application.logMessageReceived += HandleLog;


        }
    }
}
