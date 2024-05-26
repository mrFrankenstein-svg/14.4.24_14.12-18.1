using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvirumentTranslucent_Slave : MonoBehaviour
{
    [SerializeField] GameObject normalObj;
    [SerializeField] GameObject translucentObj;
    private void Start()
    {
        normalObj = gameObject.transform.Find("Normal").gameObject;
        translucentObj = gameObject.transform.Find("Translucent").gameObject;
    }

    public void TranslucentON()
    {
        normalObj.SetActive(false);
        translucentObj.SetActive(true);
    }
    public void TranslucentOFF()
    {
        normalObj.SetActive(true);
        translucentObj.SetActive(false);
    }
}
