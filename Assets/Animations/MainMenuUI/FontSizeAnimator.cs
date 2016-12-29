using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontSizeAnimator : MonoBehaviour {

    private enum FontSize
    {
        Selected = 18,
        Default = 12
    }

    private FontSize fontSize = FontSize.Default;
    private FontSize coroutineTarget = FontSize.Default;

    private Text text;
    private Button btn;

    public void Start()
    {
        text = GetComponent<Text>();
        btn = GetComponentInParent<Button>();
    }

    public void UpscaleText()
    {
        if (btn.interactable)
            fontSize = FontSize.Selected;
    }

    public void DownscaleText()
    {
        if (btn.interactable)
            fontSize = FontSize.Default;
    }

    public void Update()
    {
        if (coroutineTarget != fontSize)
        {
            StopAllCoroutines();
            coroutineTarget = fontSize;
            StartCoroutine(AnimateFontSize((int)fontSize));
        }
    }

    IEnumerator AnimateFontSize(int size)
    {
        int currentSize = text.fontSize;
        int difference = Mathf.Abs(size - currentSize);


        for (int i = 0; i < difference; i++)
        {
            text.fontSize = (int)Mathf.Lerp(currentSize, size, ((float)i / (float)difference));
            yield return null;
        }

    }
}
