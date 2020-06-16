using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class savepoint : MonoBehaviour
{
    void OnTriggerEnter(Collider co)
    {
        Debug.Log("trigger");
        if (co.gameObject.tag == "character")
        {
            co.gameObject.GetComponent<character_recover>().recover_pos = gameObject.transform.position + Vector3.up * 10;
            Destroy(gameObject);
        }
    }
}
