using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoulButton : MonoBehaviour
{
    public Sprite normalSpr;
    public Sprite selectedSpr;
    public GameObject rootMenu;
    public UnityEngine.Events.UnityEvent onClick;
    public RectTransform soul;
    public RectTransform buttonTransform;
    public Image image;
    
    void Update()
    {
        if (RectTransformExtensions.Overlaps(buttonTransform, soul))
        {
            image.sprite = selectedSpr;

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z))
            {
                onClick.Invoke();
                rootMenu.SetActive(false);
            }
        }
        else
        {
            image.sprite = normalSpr;
        }
    }
}

public static class RectTransformExtensions
{
    
        public static bool Overlaps(this RectTransform a, RectTransform b) {
            return a.WorldRect().Overlaps(b.WorldRect());
        }
        public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse) {
            return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
        }

        public static Rect WorldRect(this RectTransform rectTransform)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            Vector2 pivot = rectTransform.pivot;
            
            float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
            float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

            //With this it works even if the pivot is not at the center
            Vector3 position =rectTransform.TransformPoint(rectTransform.rect.center);
            float x = position.x - rectTransformWidth * 0.5f;
            float y = position.y - rectTransformHeight * 0.5f;
            
            return new Rect(x,y, rectTransformWidth, rectTransformHeight);
        }
    
}
