using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandAnimatorController : MonoBehaviour
{
    private Animator handAnimator;
    private InputDevice targetDevice;
    [SerializeField]
    private InputDeviceCharacteristics deviceCharacteristics;

    private void Start()
    {
        handAnimator = GetComponent<Animator>();
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
        else
        {
            Debug.LogError("Device not found");
        }
    }

    private void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }
        
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }
    }

    private void Update()
    {
        UpdateHandAnimation();
    }
}
