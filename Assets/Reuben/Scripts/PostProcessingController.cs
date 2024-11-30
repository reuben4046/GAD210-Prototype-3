using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] private VolumeProfile volumeProfile;
    // Start is called before the first frame update
    void Start()
    {
        volumeProfile = GetComponent<Volume>().profile;
    }

    // Update is called once per frame
    void Update()
    {
        float hueShiftValue = Mathf.Sin(Time.time / 6) * 180f;
        if (volumeProfile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.hueShift.value = hueShiftValue;
        }
    }
}
