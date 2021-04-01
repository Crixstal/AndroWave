using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHeart : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemList = null;
    private Transform Epos;

    // Start is called before the first frame update
    void Start()
    {
        Epos = GetComponent<Transform>();
    }

    public void ItemDrop()
    {
        Instantiate(itemList[0], Epos.position, Quaternion.identity);
    }
}
