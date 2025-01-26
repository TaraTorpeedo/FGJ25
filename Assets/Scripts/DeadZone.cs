using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public CinemachineFreeLook CineCam;

    Transform FollowTransform;
    Transform Spawnpoint;

    private void Start()
    {
        FollowTransform = CineCam.Follow;
        Spawnpoint = CineCam.Follow;
    }

    private void OnTriggerEnter(Collider other)
    {
        CineCam.Follow = null;

        StartCoroutine(Respawn(other.gameObject));

    }

    IEnumerator Respawn(GameObject gameObject)
    {
        yield return new WaitForSeconds(4);

        CineCam.Follow = FollowTransform;
        gameObject.transform.position = new Vector3(8, 0, -17);

    }
}

