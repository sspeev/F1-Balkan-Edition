using UnityEngine;

public class BackRedLightScript : MonoBehaviour
{
    private new Light light;
    private float timer = 2f;


    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private void Start()
    {
        light = GetComponent<Light>();
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0.5f;
            if (light.enabled == true)
            {
                light.enabled = false;
            }
            else light.enabled = true;
        }
    }
    void FixedUpdate()
    {
        var desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.LookAt(target);
    }
}
