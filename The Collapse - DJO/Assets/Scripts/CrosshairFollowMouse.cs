using UnityEngine;

public class CrosshairFollowMouse : MonoBehaviour
{
    public RectTransform crosshair; // Referência à imagem da mira
    public Canvas canvas;          // Referência ao Canvas

    void Update()
    {
        Vector2 cursorPosition;

        // Converte a posição do mouse para coordenadas locais no Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, 
            Input.mousePosition, 
            canvas.worldCamera, 
            out cursorPosition);

        // Atualiza a posição da mira
        crosshair.localPosition = cursorPosition;
    }
}