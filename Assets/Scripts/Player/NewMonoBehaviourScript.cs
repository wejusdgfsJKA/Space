using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    Quaternion baseSpin; // set once at start
    public float yDegrees = 0;

    public float speed = 4; // allow us to adjust as we run

    void Start()
    {
        baseSpin = transform.rotation;
    }

    void Update()
    {
        Quaternion ySpin = Quaternion.Euler(0, yDegrees, 0);
        yDegrees += speed;

        transform.rotation = baseSpin * ySpin;
        // or, to test global: =yspin*baseSpin;
    }
}
