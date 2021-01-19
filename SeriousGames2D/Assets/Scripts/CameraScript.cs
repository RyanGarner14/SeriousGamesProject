using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform holder;
    Camera camObj;

    Vector3 originPos;
    float originZoom;
    
    public float lerpSpeed;
    public float zoomAmount;

    void Start()
    {
        camObj = GetComponent<Camera>();
        originPos = holder.position;
        originZoom = camObj.orthographicSize;
    }

    public void MoveToPos(Vector3 newPos)
    {
        StartCoroutine(lerpTo(newPos, zoomAmount));
    }

    public void MoveToOrigin()
    {
        StartCoroutine(lerpTo(originPos, originZoom));
    }

    IEnumerator lerpTo(Vector3 newPos, float newZoom)
    {
        float oldZoom = 0;
        if(camObj != null)
            oldZoom = camObj.orthographicSize;
        Vector3 oldPos = holder.position;
        newPos.z = originPos.z;
        float t = 0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / lerpSpeed);

            holder.position = Vector3.Lerp(oldPos, newPos, t);
            if(camObj)
                camObj.orthographicSize = Mathf.Lerp(oldZoom, newZoom, t);

            yield return 0;
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
