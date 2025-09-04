using UnityEngine;
using TMPro;

namespace CX.Utilty
{
    public class FPS : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fpsText;

        private float m_DeltaTime;

        // Update is called once per frame
        void Update()
        {
            m_DeltaTime += (Time.deltaTime - m_DeltaTime) * 0.1f;
            fpsText.text = "FPS: " + Mathf.Ceil(1f / m_DeltaTime).ToString();
        }
    }
}
