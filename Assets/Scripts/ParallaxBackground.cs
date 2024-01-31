using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    Transform cameraTransform;
    Vector3 lastCameraPosition;
    float textureUnitSize;

    [SerializeField] private float parallaxEffectMultiplier;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSize = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;
    }

    //--------------------------------------------------------------------------
    private void LateUpdate()
    {
        // Not designed to work for vertical movement or negative / backwards movement
        Vector3 dM = (cameraTransform.position - lastCameraPosition) * parallaxEffectMultiplier;
        Vector3 tP = transform.position;
        transform.position = new Vector3(tP.x + dM.x, tP.y + dM.y, 1);
        lastCameraPosition = cameraTransform.position;

        if (cameraTransform.position.x - transform.position.x >= textureUnitSize)
        {
            float offsetPosition = (cameraTransform.position.x - transform.position.x) % textureUnitSize;
            transform.position = new Vector3(cameraTransform.position.x + offsetPosition, transform.position.y,1);
        }
    }
}
