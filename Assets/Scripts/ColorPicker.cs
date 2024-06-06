using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public delegate void ColorEvent(Color c);

    public bool done = true;

    private ColorEvent onCC;
    private ColorEvent onCS;

    public Color32 originalColor;
    public Color32 modifiedColor;

    public HSV modifiedHsv;

    private bool interact;

    public RectTransform positionIndicator;
    public Slider mainComponent;
    public Slider rComponent;
    public Slider gComponent;
    public Slider bComponent;
    public InputField hexaComponent;
    public RawImage colorComponent;

    public bool noLoad = false;

    private void Start()
    {
        if (!noLoad)
            FindObjectOfType<PlayerStats>().TryLoad();

        IEnumerator FixAttempt()
        {
            yield return new WaitForSecondsRealtime(0.4f);

            float r = FindObjectOfType<PlayerStats>().data.playercolor_r;
            float g = FindObjectOfType<PlayerStats>().data.playercolor_g;
            float b = FindObjectOfType<PlayerStats>().data.playercolor_b;
            
            Create(new Color(r / 255, g / 255, b / 255), (c) => OnColorUpdate(), (c) => OnColorChanged());
        }

        StartCoroutine(FixAttempt());
    }

    public DynamicCosmeticManager cosmeticManager;

    public void OnColorUpdate()
    {
        cosmeticManager.playerShirt.color = modifiedColor;
    }

    public void OnColorChanged()
    {
        if (!noLoad)
            FindObjectOfType<PlayerStats>().TryLoad();

        FindObjectOfType<PlayerStats>().data.playercolor_r = modifiedColor.r;
        FindObjectOfType<PlayerStats>().data.playercolor_g = modifiedColor.g;
        FindObjectOfType<PlayerStats>().data.playercolor_b = modifiedColor.b;

        if (!noLoad)
            FindObjectOfType<PlayerStats>().Save();
    }

    public void OnColorReset()
    {
        modifiedColor.r = 124;
        modifiedColor.g = 217;
        modifiedColor.b = 150;
        FindObjectOfType<PlayerStats>().data.playercolor_r = modifiedColor.r;
        FindObjectOfType<PlayerStats>().data.playercolor_g = modifiedColor.g;
        FindObjectOfType<PlayerStats>().data.playercolor_b = modifiedColor.b;

        if (!noLoad)
            FindObjectOfType<PlayerStats>().Save();

        RecalculateMenu(true);

        OnColorUpdate();
    }
    
    public bool Create(Color original, ColorEvent onColorChanged, ColorEvent onColorSelected)
    {   
        if(done)
        {
            done = false;
            originalColor = original;
            modifiedColor = original;
            onCC = onColorChanged;
            onCS = onColorSelected;
            gameObject.SetActive(true);
            RecalculateMenu(true);
            hexaComponent.placeholder.GetComponent<Text>().text = "RRGGBB";
            return true;
        }
        else
        {
            Done();
            return false;
        }
    }

    private void RecalculateMenu(bool recalculateHSV)
    {
        interact = false;

        if(recalculateHSV)
            modifiedHsv = new HSV(modifiedColor);
        else
            modifiedColor = modifiedHsv.ToColor();

        rComponent.value = modifiedColor.r;
        rComponent.transform.GetChild(3).GetComponent<InputField>().text = modifiedColor.r.ToString();
        gComponent.value = modifiedColor.g;
        gComponent.transform.GetChild(3).GetComponent<InputField>().text = modifiedColor.g.ToString();
        bComponent.value = modifiedColor.b;
        bComponent.transform.GetChild(3).GetComponent<InputField>().text = modifiedColor.b.ToString();
        mainComponent.value = (float)modifiedHsv.H;
        rComponent.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(255, modifiedColor.g, modifiedColor.b, 255);
        rComponent.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = new Color32(0, modifiedColor.g, modifiedColor.b, 255);
        gComponent.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(modifiedColor.r, 255, modifiedColor.b, 255);
        gComponent.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = new Color32(modifiedColor.r, 0, modifiedColor.b, 255);
        bComponent.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(modifiedColor.r, modifiedColor.g, 255, 255);
        bComponent.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = new Color32(modifiedColor.r, modifiedColor.g, 0, 255);
        positionIndicator.parent.GetChild(0).GetComponent<RawImage>().color = new HSV(modifiedHsv.H, 1d, 1d).ToColor();
        positionIndicator.anchorMin = new Vector2((float)modifiedHsv.S, (float)modifiedHsv.V);
        positionIndicator.anchorMax = positionIndicator.anchorMin;
        hexaComponent.text = ColorUtility.ToHtmlStringRGB(modifiedColor);
        colorComponent.color = modifiedColor;
        onCC?.Invoke(modifiedColor);
        interact = true;
    }

    public void SetChooser()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(positionIndicator.parent as RectTransform, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out Vector2 localpoint);
        localpoint = Rect.PointToNormalized((positionIndicator.parent as RectTransform).rect, localpoint);
        if (positionIndicator.anchorMin != localpoint)
        {
            positionIndicator.anchorMin = localpoint;
            positionIndicator.anchorMax = localpoint;
            modifiedHsv.S = localpoint.x;
            modifiedHsv.V = localpoint.y;
            RecalculateMenu(false);
        }
    }

    public void SetMain(float value)
    {
        if (interact)
        {
            modifiedHsv.H = value;
            RecalculateMenu(false);
        }
    }

    public void SetR(float value)
    {
        if (interact)
        {
            modifiedColor.r = (byte)value;
            RecalculateMenu(true);
        }
    }

    public void SetR(string value)
    {
        if(interact)
        {
            modifiedColor.r = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
            RecalculateMenu(true);
        }
    }

    public void SetG(float value)
    {
        if(interact)
        {
            modifiedColor.g = (byte)value;
            RecalculateMenu(true);
        }
    }
 
    public void SetG(string value)
    {
        if (interact)
        {
            modifiedColor.g = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
            RecalculateMenu(true);
        }
    }

    public void SetB(float value)
    {
        if (interact)
        {
            modifiedColor.b = (byte)value;
            RecalculateMenu(true);
        }
    }

    public void SetB(string value)
    {
        if (interact)
        {
            modifiedColor.b = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
            RecalculateMenu(true);
        }
    }

    //gets hexa InputField value
    public void SetHexa(string value)
    {
        if (interact)
        {
            if (ColorUtility.TryParseHtmlString("#" + value, out Color c))
            {
                c.a = 1;
                modifiedColor = c;
                RecalculateMenu(true);
            }
            else
            {
                hexaComponent.text = ColorUtility.ToHtmlStringRGB(modifiedColor);
            }
        }
    }

    public void CCancel()
    {
        Cancel();
    }

    public void Cancel()
    {
        modifiedColor = originalColor;
        Done();
    }

    public void CDone()
    {
        Done();
    }

    public void Done()
    {
        done = true;
        onCC?.Invoke(modifiedColor);
        onCS?.Invoke(modifiedColor);
    }

    public sealed class HSV
    {
        public double H = 0, S = 1, V = 1;
        public byte A = 255;
        public HSV () { }
        public HSV (double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }
        public HSV (Color color)
        {
            float max = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
            float min = Mathf.Min(color.r, Mathf.Min(color.g, color.b));

            float hue = (float)H;

            if (min != max)
            {
                if (max == color.r)
                    hue = (color.g - color.b) / (max - min);
                else if (max == color.g)
                    hue = 2f + (color.b - color.r) / (max - min);
                else
                    hue = 4f + (color.r - color.g) / (max - min);

                hue *= 60;
                if (hue < 0) hue += 360;
            }

            H = hue;
            S = (max == 0) ? 0 : 1d - ((double)min / max);
            V = max;
            A = (byte)(color.a * 255);
        }
        public Color32 ToColor()
        {
            int hi = Convert.ToInt32(Math.Floor(H / 60)) % 6;
            double f = H / 60 - Math.Floor(H / 60);

            double value = V * 255;
            byte v = (byte)Convert.ToInt32(value);
            byte p = (byte)Convert.ToInt32(value * (1 - S));
            byte q = (byte)Convert.ToInt32(value * (1 - f * S));
            byte t = (byte)Convert.ToInt32(value * (1 - (1 - f) * S));

            switch(hi)
            {
                case 0:
                    return new Color32(v, t, p, A);
                case 1:
                    return new Color32(q, v, p, A);
                case 2:
                    return new Color32(p, v, t, A);
                case 3:
                    return new Color32(p, q, v, A);
                case 4:
                    return new Color32(t, p, v, A);
                case 5:
                    return new Color32(v, p, q, A);
                default:
                    return new Color32();
            }
        }
    }
}