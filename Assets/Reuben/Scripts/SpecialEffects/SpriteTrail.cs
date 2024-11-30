using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrail : MonoBehaviour
{

    [Header("SpriteTrail Related")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float meshRefreshRate = 0.1f;

    [Header("ShaderRelated")]
    [SerializeField] private Material trailMaterial;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ActivateTrail());
    }


    IEnumerator ActivateTrail()
    {
        while (true)
        {
            GameObject spriteTrail = new GameObject();
            spriteTrail.transform.position = transform.position;
            spriteTrail.transform.rotation = transform.rotation;

            SpriteRenderer sr = spriteTrail.AddComponent<SpriteRenderer>();

            sr.sprite = spriteRenderer.sprite;
            sr.material = trailMaterial;
            sr.sortingOrder = 3;


            Destroy(spriteTrail, duration);
            yield return new WaitForSeconds(meshRefreshRate);
        }
    }
}
