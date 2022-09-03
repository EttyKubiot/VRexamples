using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimatorHandsController : MonoBehaviour
{
    private Animator animator;
    private InputDevice targetDevice;

    [SerializeField] private InputDeviceCharacteristics deviceCharacteristics;

    private void Start()
    {
        animator = GetComponent<Animator>();
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

    private void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            animator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            animator.SetFloat("Trigger", 0);
        }
        
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            animator.SetFloat("Grip", gripValue);
        }
        else
        {
            animator.SetFloat("Grip", 0);
        }
    }
}
