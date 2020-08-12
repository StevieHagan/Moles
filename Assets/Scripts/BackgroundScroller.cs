using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.5f;
    [SerializeField] Material[] materials;

    Renderer bgRenderer;

    Vector2 offset;

    void Start()
    {
        offset = new Vector2(0, scrollSpeed);
        bgRenderer = GetComponent<Renderer>();

        //Pick a background material at random
        Material material = materials[Random.Range(0, materials.Length)];
        bgRenderer.material = material;

        //Rotate the background a random amount
        gameObject.transform.rotation = Quaternion.Euler(0, 0,
                                        Random.Range(0, 360));
    }

    void Update()
    {
        bgRenderer.material.mainTextureOffset += offset * Time.deltaTime;
    }
}
