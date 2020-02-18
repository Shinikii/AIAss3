using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    public GameController main;
    public int zoneNum;
    // Start is called before the first frame update
    void Start()
    {
        string temp = this.name;
        string[] temp2 = temp.Split('e');
        zoneNum = int.Parse(temp2[1]);
        Debug.Log(zoneNum);

        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)){
            Debug.Log(this.gameObject.name);
            main.PlaneClick(zoneNum);
        }
    }
}
