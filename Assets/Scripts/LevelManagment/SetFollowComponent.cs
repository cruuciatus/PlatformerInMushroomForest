
using Cinemachine;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(CinemachineVirtualCamera))]
public class SetFollowComponent : MonoBehaviour
{
    CinemachineVirtualCamera vCamera;
    private void Start()
    {
        vCamera = GetComponent<CinemachineVirtualCamera>();
        //vCamera.Follow = FindObjectOfType<Hero>().transform;

        StartCoroutine(GetHero());
    }

    IEnumerator GetHero()
    {
        while (vCamera.Follow == null)
        {
            Hero hero = FindObjectOfType<Hero>();
            if (hero) vCamera.Follow = hero.transform;
            yield return new WaitForSeconds(0.1f);

            
        }

    }
}
