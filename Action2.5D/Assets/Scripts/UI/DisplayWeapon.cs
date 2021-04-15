using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWeapon : MonoBehaviour
{
    [SerializeField] private GameObject player = null;

    private TMPro.TMP_Text text;
    private GameObject firstActiveChild;

    void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    void Update()
    {
        for (int i = 0; i < player.transform.GetChild(0).childCount; i++)
        {
            if (player.transform.GetChild(0).GetChild(i).gameObject.activeSelf == true)
                firstActiveChild = player.transform.GetChild(0).GetChild(i).gameObject;
        }

        text.text = firstActiveChild.name.ToString();
    }
}
