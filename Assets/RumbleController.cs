using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class RumbleController : MonoBehaviour
{
    PlayerInput playerInput;
    Gamepad gamepad;
    float pulseDuration;
    float rumbleStep;
    float lowF;//left
    float highF;//right
    bool isMotorActive = false;
    bool isRumblePulseActive = false;

    void Start()
    {
        playerInput = InputManagerController.instance.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRumblePulseActive)
        {
            if (Time.time > pulseDuration)
            {
                playerInput = InputManagerController.instance.GetComponent<PlayerInput>();
                isMotorActive = !isMotorActive;
                pulseDuration = Time.time + rumbleStep;
                if (!isMotorActive)
                {
                    gamepad = GetGamepad();
                    if(gamepad is null) return;
                    gamepad.SetMotorSpeeds(0, 0);
                }
                else
                {
                    gamepad = GetGamepad();
                    if(gamepad is null) return;
                    gamepad.SetMotorSpeeds(lowF, highF);
                }
            }
        }
    }

    public void RumbleConstant()
    {
        gamepad = GetGamepad();
        if(gamepad is null) return;
        gamepad.SetMotorSpeeds(0.5f, 0.5f);
    }

    public void RumblePulse(float lowValue, float highValue)
    {
        isRumblePulseActive = true;
        lowF = lowValue;
        highF = highValue;
        rumbleStep = 0.25f;
        pulseDuration = Time.time + rumbleStep;
    }

    public void StopRumble()
    {
        isRumblePulseActive = false;
        gamepad = GetGamepad();
        if(gamepad is null) return;
        else gamepad.SetMotorSpeeds(0f, 0f);
    }

    private Gamepad GetGamepad()
    {
        playerInput = InputManagerController.instance.GetComponent<PlayerInput>();
        return Gamepad.all.FirstOrDefault(g => playerInput.devices.Any(d => d.deviceId == g.deviceId));
    }

    public bool GamepadPresent()
    {
        playerInput = InputManagerController.instance.GetComponent<PlayerInput>();
        if(GetGamepad() is null) return false;
        else return true;
    }
}
