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

    public static float PlayerBulletTimeSpeed = 0.6f;
    public static float EnemyBulletTimeSpeed = 0.3f;

    public delegate void Notifier();

    //enemy script will subscribe to this delegate with a 
    public static Notifier EnemyNotifier;

    public static void EnableBulletTime()
    {
        Debug.Log("BT ACTIVE");
        if (IsBulletTimeActive)
        {
            DisableBulletTime();
            return;
        }

        Time.timeScale = 0.5f;
        PlayerCurrentSpeed = PlayerBulletTimeSpeed;
        //EnemyCurrentSpeed = EnemyBulletTimeSpeed;
        IsBulletTimeActive = true;
        //EnemyNotifier.Invoke();
    }
    public static void DisableBulletTime()
    {
        Debug.Log("BT INACTIVE");
        Time.timeScale = 1f;
        PlayerCurrentSpeed = 1f;
        IsBulletTimeActive = false;

    }
    public static void EnablePause()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        PlayerCurrentSpeed = 0f;
        IsGamePaused = true;

    }
    public static void DisablePause()
    {
        if (IsBulletTimeActive)
        {
            Time.timeScale = 0.5f;
            PlayerCurrentSpeed = PlayerBulletTimeSpeed;



        }
        else
        {
            Time.timeScale = 1f;
            PlayerCurrentSpeed = 1f;

        }
        Cursor.visible = false;
        IsGamePaused = false;

    }
}
