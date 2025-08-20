using TMPro;
using UnityEngine;

namespace _ProjectA.Scripts.DEBUG
{
    public class DEBUGGINGMANAGER : MonoBehaviour
    {
        [SerializeField] private TMP_Text fpsText;
        [SerializeField] private float updateRate = 0.5f; // how often to refresh, in seconds

        private float timer;
        private int frameCount;

        private void Update()
        {
            frameCount++;
            timer += Time.deltaTime;

            if (timer >= updateRate)
            {
                float fps = frameCount / timer;
                fpsText.text = $"{Mathf.RoundToInt(fps)} FPS";

                frameCount = 0;
                timer = 0f;
            }
        }
    }
}
