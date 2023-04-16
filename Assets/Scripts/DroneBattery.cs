using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneBattery : MonoBehaviour
{
    [SerializeField] DroneMovement droneMovement;
    [SerializeField] DroneCamera droneCamera;
    [SerializeField] private Image batteryInner;
    [SerializeField] private Image screenSaver;
    public ClawGrabber clawGrabber;

    private float batteryMaxTime = 120f;
    private float batteryTime;
    private float batteryCooldownTime = 4f;

    public bool isTimerOn;

    void Start()
    {
        TimerReset();
        screenSaver.enabled = false;
    }

    void FixedUpdate()
    {
        if (isTimerOn)
        {
            if (batteryTime > 0)
            {
                screenSaver.enabled = false;
                batteryTime -= Time.deltaTime;
                batteryInner.fillAmount = batteryTime/batteryMaxTime;
            }
            else
            {
                DisableDrone();
                clawGrabber.DropObject();
                clawGrabber.grabIndicator.color = Color.red;
            }
        }
    }

    public void TimerReset()
    {
        //batteryFillIndicator.transform.localScale = new Vector3(.1f, batteryFillIndicator.transform.localScale.y, batteryFillIndicator.transform.localScale.z);
        batteryInner.fillAmount = 1;
        batteryTime = batteryMaxTime;
        isTimerOn = true;
        droneCamera.changeDroneState(isTimerOn);
        droneMovement.changeDroneState(isTimerOn);
    }
    public void DisableDrone(float cooldown)
    {
        batteryInner.fillAmount = 0;
        batteryTime = 0;
        screenSaver.enabled = true;
        isTimerOn = false;
        droneCamera.changeDroneState(isTimerOn);
        droneMovement.changeDroneState(isTimerOn);
        StartCoroutine(activeDrone(cooldown));
    }
    private void DisableDrone()
    {
        batteryTime = 0;
        screenSaver.enabled = true;
        isTimerOn = false;
        droneCamera.changeDroneState(isTimerOn);
        droneMovement.changeDroneState(isTimerOn);
        StartCoroutine(activeDrone(batteryCooldownTime));
    }
    public float getBattery()
    {
        return batteryTime;
    }
    IEnumerator activeDrone(float delay)
    {
        yield return new WaitForSeconds(delay);
        TimerReset();
    }
}
