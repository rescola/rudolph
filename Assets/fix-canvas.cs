using UnityEngine;

public class CanvasFixer : MonoBehaviour
{
    private Transform parentTransform;

    void Start()
    {
        parentTransform = transform.parent; // Guarda el parent original
    }

    void LateUpdate()
    {
        if (parentTransform != null)
        {
            // Manté la mateixa rotació global (identitat)
            transform.rotation = Quaternion.identity;

            // Corregeix l'escala només si el personatge està girant
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * Mathf.Sign(parentTransform.localScale.x);
            transform.localScale = localScale;
        }
    }
}
