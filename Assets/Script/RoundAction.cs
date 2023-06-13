using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RoundAction : MonoBehaviour
{
    public float _radius_length;
    public float _angle_speed;
    public float timer = 10f;
    private float temp_angle;
    public Transform target;
    private Vector3 _pos_new;
    public Vector3 _center_pos;

    public bool _round_its_center;

    // Use this for initialization
    void Start()
    {
        if (_round_its_center)
        {
            _center_pos = transform.localPosition;
        }
        target = GameObject.Find("Player").transform;
        _center_pos.x = target.position.x;
        _center_pos.y = target.position.y;
        _center_pos.z = target.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
        temp_angle += _angle_speed * Time.deltaTime; // 

        _pos_new.x = _center_pos.x + Mathf.Cos(temp_angle) * _radius_length;
        _pos_new.y = _center_pos.y + Mathf.Sin(temp_angle) * _radius_length;

        transform.localPosition = _pos_new;
    }
}