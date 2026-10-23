using TMPro;
using UnityEngine;

public class DMGText : MonoBehaviour
{
    public Color color_i, color_f;
    public Vector3 pos_i, pos_f;
    public float fadeDur;
    private float fadeStart;

    public TMP_Text text;

    private void Start()
    {
        fadeStart = Time.time;
    }

    private void Update()
    {
        float progress = (Time.time - fadeStart) / fadeDur;
        if (progress <= 1)
        {
            transform.localPosition = Vector3.Lerp(pos_i, pos_f, progress);
            text.color = Color.Lerp(color_i, color_f, progress);
        }
        else Destroy(gameObject);
    }
}
