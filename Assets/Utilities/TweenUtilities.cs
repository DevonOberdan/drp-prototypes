using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweenUtilities : MonoBehaviour
{
    [SerializeField] private float LOOP_TIME = 0.5f;
    [SerializeField] private float TIME_BETWEEN_LOOPS = 1.5f;

    [Header("Scale")]
    [SerializeField] private float scaleFactor = 1.1f;
    
    #region Scale Fields
    private Vector3 startScale;
    private Sequence scaleSequence;
    private Tween scaleTween;

    private float ScaleBottomRange => 1 - (scaleFactor - 1);
    #endregion

    [Header("Color")]
    [SerializeField] private Color colorToTweenTo = Color.gray;
    
    #region Color Fields
    private Color startColor;
    private Graphic graphic;
    private Material mat;

    private Sequence colorSequence;
    #endregion

    private void Awake()
    {
        startScale = transform.localScale;

        graphic = GetComponent<Graphic>();
        mat = null;
        if (graphic == null && TryGetComponent(out MeshRenderer rend))
        {
            mat = rend.material;
        }

        if(mat != null)
            startColor = mat.color;
        else if(graphic != null)
            startColor = graphic.color;
    }

    #region Color
    public void SetColorLoop(bool loop)
    {
        if (loop)
            ColorLoop();
        else
            ColorLoopEnd();
    }

    private void ColorLoop()
    {
        if (mat == null && graphic == null)
        {
            Debug.LogError("Trying to Tween color on an object with no visual component.");
            return;
        }

        Kill(colorSequence);

        colorSequence = DOTween.Sequence();

        Tween tween;

        if (mat != null)
            tween = mat.DOBlendableColor(colorToTweenTo, LOOP_TIME).SetLoops(2, LoopType.Yoyo);
        else
            tween = graphic.DOBlendableColor(colorToTweenTo, LOOP_TIME).SetLoops(2, LoopType.Yoyo);

        SetupLoopingSequence(colorSequence, tween);
    }

    private void ColorLoopEnd()
    {
        Kill(colorSequence);

        if (mat != null)
            mat.color = startColor;
        else if (graphic != null)
            graphic.color = startColor;
    }

    #endregion

    public void SetScaleLoop(bool loop)
    {
        if (loop)
            ScaleLoop();
        else
            ScaleLoopEnd();
    }

    public void Scale(bool scale)
    {
        Kill(scaleTween);

        scaleTween = transform.DOScale(scale ? startScale * scaleFactor : startScale, 0.1f);
    }

    private void ScaleLoop()
    {
        Kill(scaleSequence);

        transform.localScale = startScale * ScaleBottomRange;

        scaleSequence = DOTween.Sequence();
        Tween tween = transform.DOScale(startScale * scaleFactor, LOOP_TIME).SetLoops(2, LoopType.Yoyo);

        SetupLoopingSequence(scaleSequence, tween);
    }

    private void ScaleLoopEnd()
    {
        Kill(scaleSequence);
        transform.localScale = startScale;
    }

    private void SetupLoopingSequence(Sequence s, Tween tween)
    {
        s.Append(tween);
        s.AppendInterval(TIME_BETWEEN_LOOPS);
        s.SetLoops(-1);
    }

    private void Kill(Sequence s)
    {
        if (s.IsActive())
            s.Kill();
    }

    private void Kill(Tween tween)
    {
        if(tween.IsActive())
            tween.Kill();
    }
}