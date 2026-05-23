using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class RadialLayoutGroup : LayoutGroup//, ILayoutGroup
{

    public enum RadialLayoutStart { top, left, right, bottom };
    public RadialLayoutStart StartFrom;

    public float Offset;

    public float Radius = 1.0f;
    public float Arc = 360.0f;

    public override void SetLayoutHorizontal()
    {
        UpdateChildren();
    }

    public override void SetLayoutVertical()
    {

        UpdateChildren();
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        UpdateChildren();
    }
    public override void CalculateLayoutInputVertical()
    {
        UpdateChildren();
    }
    private RectTransform[] GetActiveChildren()
    {
        List<RectTransform> list = new();
        foreach (RectTransform t in transform)
        {
            if (t.gameObject.activeSelf)
            {
                list.Add(t);
            }
        }
        return list.ToArray();
    }
    void UpdateChildren()
    {
        var children = GetActiveChildren();
        int i = 0;
        float angleStep = Arc / (children.Length * 2);
        Vector3 direction;


        if (StartFrom == RadialLayoutStart.bottom) direction = -transform.up;
        else if (StartFrom == RadialLayoutStart.right) direction = transform.right;
        else if (StartFrom == RadialLayoutStart.left) direction = -transform.right;
        else direction = transform.up;

        int j = 0;
        for (i = 1; j < children.Length; i += 2)
        {
            RectTransform t = children[j];
            j++;
            t.position = transform.position + Quaternion.Euler(0, 0, Offset + angleStep * i) * direction * Radius * rectTransform.lossyScale.magnitude;
        }
        //i = 0;
        //foreach (RectTransform t in children)
        //{
        //    t.position = transform.position + Quaternion.Euler(0, 0, Offset + angleStep * i) * direction * Radius;
        //    i++;
        //}

    }

}
