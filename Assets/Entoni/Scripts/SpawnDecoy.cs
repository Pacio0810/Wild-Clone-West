using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnDecoy : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject decoy;

    [Header("Timers")]
    [SerializeField] float decoyDuration = 6;
    [SerializeField] bool spawned;
    [SerializeField] float decoyCD;
    [SerializeField] float decoyTimer = 10;


    void Start()
    {
        decoyCD = decoyTimer;
    }

    void Update()
    {
        StartCoroutine(Courutine(0.1f));
    }

    IEnumerator Courutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        decoyCD -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) && decoyCD <= 0)
        {
            GameObject dec = Instantiate(decoy, transform.position, Quaternion.identity);
            Destroy(dec, decoyDuration);
            decoyCD = decoyTimer;
        }
    }
}
