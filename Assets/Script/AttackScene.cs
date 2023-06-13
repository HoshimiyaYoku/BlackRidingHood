using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScene : MonoBehaviour
{
    private static AttackScene instance;//私有静态实例
    //公有静态属性
    public static AttackScene Instance
    {
        get
        {
            if(instance == null)
                instance = Transform.FindObjectOfType<AttackScene>();
                return instance;

        }
    }

    private bool isShake;
    //相机震动携程
    IEnumerator Shake(float duration,float strength)//持续时间，震动强度
    {
        isShake = true;
        Transform camera = Camera.main.transform;
        Vector3 startPosition = camera.position;

        while (duration > 0)
        {
            camera.position = Random.insideUnitSphere * strength + startPosition;
            duration -= Time.deltaTime;
            yield return null;
        }
        camera.position = startPosition;
        isShake = false;
    }

    public void CameraShake(float duration,float strength)
    {
        if (!isShake)
        {
            StartCoroutine(Shake(duration, strength));
        }
    }
    public void HitPause(int durtion)//调用攻击暂停携程
    {
        StartCoroutine(Pause(durtion));
    }

    IEnumerator Pause(int durtion)//传入值为帧数(等待时间)
    {
        float pauseTime = durtion / 60f;//pauseTime为真实等待时间
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }

    
}
