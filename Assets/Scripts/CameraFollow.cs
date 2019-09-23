using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private GameObject followObj;

    public void SetFollow(GameObject obj)
    {
        if (obj == null)
        {
            transform.position = new Vector3(0, 2.15f, -10);
            Camera.main.orthographicSize = 7.25f;
        }
        else
        {
            Camera.main.orthographicSize = 3.15f;
        }
        followObj = obj;
    }

    void LateUpdate()
    {
        if (followObj == null) return;

        Vector2 followPos = followObj.transform.position;
        transform.position = new Vector3(followPos.x, followPos.y, transform.position.z);
    }
}