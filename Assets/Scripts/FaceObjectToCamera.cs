using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObjectToCamera : MonoBehaviour
{
    // Update is called once per frame -> nameTag ở tất cả người chơi sẽ hướng về camera của người chơi local
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
