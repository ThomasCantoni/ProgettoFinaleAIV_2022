using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class TimeManager 
{
    public static bool IsGamePaused = false;
    public static bool IsBulletTimeActive = false;
    public static float PlayerCurrentSpeed = 1f;
    public static float EnemyCurrentSpeed = 1f;

    public static float PlayerBulletTimeSpeed = 0.75f;
    public static float EnemyBulletTimeSpeed = 0.3f;
    
    public static void EnableBulletTime(InputAction.CallbackContext ctx)
    {
        PlayerCurrentSpeed = PlayerBulletTimeSpeed;
        EnemyCurrentSpeed = EnemyBulletTimeSpeed;
        IsBulletTimeActive = true;
    }
    public static void DisableBulletTime()
    {
        PlayerCurrentSpeed = 1f;
        EnemyCurrentSpeed = 1f;
        IsBulletTimeActive = false;
    }
    public static void EnablePause()
    {
        IsGamePaused = true;
        PlayerCurrentSpeed = 0f;
        EnemyCurrentSpeed = 0f;
    }
    public static void DisablePause()
    {
        if (IsBulletTimeActive)
        {
            PlayerCurrentSpeed = PlayerBulletTimeSpeed;
            EnemyCurrentSpeed = EnemyBulletTimeSpeed;
            

        }
        else
        {
            PlayerCurrentSpeed = 1f;
            EnemyCurrentSpeed = 1f;
        }
        IsGamePaused = false;
    }
}
