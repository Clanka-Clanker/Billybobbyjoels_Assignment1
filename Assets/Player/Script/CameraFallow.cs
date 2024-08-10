using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{
    public Transform player;

    public float minXClamp = 3.32f;
    public float maxXClamp = 230.0f;

    public float minYClamp = -0.95f;
    public float maxYClamp = 230.9f;

    //this function always runs after fixed update - unity specifies this is where camera movement should happen
    private void LateUpdate()
    {
        Vector3 cameraPos = transform.position;

        cameraPos.x = Mathf.Clamp(player.transform.position.x, minXClamp, maxXClamp);
        cameraPos.y = Mathf.Clamp(player.transform.position.y, minYClamp, maxYClamp);

        transform.position = cameraPos;
    }
}
