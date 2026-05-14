using UnityEngine;
using UnityEngine.UI;
public class UIUAlphaController : MonoBehaviour
{
    public Image myImage;

    public void SetAlpha(float alphValue)
    {
        Color color = myImage.color;
        color.a = alphValue;
        myImage.color = color;
    }
}
