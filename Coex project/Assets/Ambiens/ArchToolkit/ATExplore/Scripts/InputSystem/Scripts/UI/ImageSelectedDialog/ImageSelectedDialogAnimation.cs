using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;


public class ImageSelectedDialogAnimation : MonoBehaviour
{
    public LineRenderer line1, line2;
    public Transform CanvasWrapper;//TODO: Manage height of the dialog based on player's height in the scene

    public AnimationCurve line1Curve, line2Curve, CanvasCurve, textCompositionCurve;
    public TextMeshProUGUI title, description;

    public CanvasGroup TextCanvasGroup;

    public float titleAnimationTime, descriptionAnimationTime;

    private float EnableTime;

    private Vector3 secondPointLine1, firstPointLine2, secondPointLine2;

    private string targetText1, targetText2;
    private float TextStartTime;

    private Vector3 textCanvasStartPos, textCanvasAnimationPos;

    private void OnEnable()
    {
        EnableTime = Time.time;

        secondPointLine1 = this.line1.GetPosition(1);
        firstPointLine2= this.line2.GetPosition(0);
        secondPointLine2 = this.line2.GetPosition(1);
        
        description.text = "";
        
        this.SetTitleText("Title");
        this.SetDescriptionText("Description");

        this.textCanvasStartPos = this.TextCanvasGroup.transform.localPosition;
        this.textCanvasAnimationPos = this.textCanvasStartPos - Vector3.up * 0.05f;
        this.TextCanvasGroup.transform.localPosition = this.textCanvasAnimationPos;

    }
    private void Update()
    {
        this.AnimateLine2();
        this.AnimateTextCanvas();
        this.AnimateLine1();
        AnimateText();
    }

    float AnimateLine1()
    {
        var e = this.line1Curve.Evaluate((Time.time - EnableTime));
        if (e <= 1.1f)
        {
            var points = new Vector3[2] {
            Vector3.zero,
            secondPointLine1*e };

            this.line1.SetPositions(points);
        }
        return e;
    }
    void AnimateLine2()
    {
        var e = this.line2Curve.Evaluate((Time.time - EnableTime));
        
        if (e <= 1.1f)
        {
            var points = new Vector3[2] {
            firstPointLine2,
            firstPointLine2 + (secondPointLine2-firstPointLine2)*e };

            this.line2.SetPositions(points);
        }
    }
    private void OnDisable()
    {
        this.line2.SetPositions(new Vector3[2] { firstPointLine2, secondPointLine2 });
        this.line1.SetPositions(new Vector3[2] { Vector3.zero, secondPointLine1 });
        this.TextCanvasGroup.transform.localPosition = this.textCanvasStartPos;
    }
    public void SetTitleText(string text)
    {
        this.targetText1 = text;
        this.title.text = "";
        this.TextStartTime = Time.time;
    }
    public void SetDescriptionText(string text)
    {
        this.targetText2 = text;
        this.description.text = "";
        this.TextStartTime = Time.time;
    }
    void AnimateText()
    {
        var e1 = this.textCompositionCurve.Evaluate((Time.time - TextStartTime));

        this.title.text = this.targetText1.Substring(0, (int)Mathf.Lerp(0f, this.targetText1.Length, 
            e1));
        this.description.text = this.targetText2.Substring(0, (int)Mathf.Lerp(0f, this.targetText2.Length, 
            e1));

    }
    void AnimateTextCanvas()
    {
        var e= this.CanvasCurve.Evaluate((Time.time - EnableTime));

        this.TextCanvasGroup.alpha= e;
        this.TextCanvasGroup.transform.localPosition = Vector3.Lerp(this.textCanvasAnimationPos, this.textCanvasStartPos, e);
    }
}
