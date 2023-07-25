//Shady
using TMPro;
using UnityEngine;

namespace Shady.Utils
{
    [RequireComponent(typeof (TMP_Text))]
    public class FPSCounter : MonoBehaviour
    {
        //===================================================
        // PRIVATE FIELDS
        //===================================================
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        const string display = "{00} FPS";
        private TMP_Text m_Text;

        //===================================================
        // METHODS
        //===================================================
        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            m_Text = GetComponent<TMP_Text>();
        }//Start() end

        private void Update()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if(Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
                m_Text.text = string.Format(display, m_CurrentFps);
            }//if end
        }//Update() end

    }//class end

}//namespace end