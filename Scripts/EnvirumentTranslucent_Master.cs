using System.Collections.Generic;
using UnityEngine;

public class EnvirumentTranslucent_Master : MonoBehaviour, IScriptHubOneSecondUpdateFunction
{
    [SerializeField] Transform mainCameraTransform;
    [SerializeField] Transform playerTransform;
    [SerializeField] List<EnvirumentTranslucent_Slave> previousRaycastHit;//= new List<EnvirumentTranslucent_Slave>();
    public void OnEnable()
    {
        ScriptHub.OnAddToScriptsList?.Invoke(this);
    }

    public void OnDisable()
    {
        ScriptHub.OnRemoveFromScriptsList?.Invoke(this);
    }

    public void Start()
    {
        mainCameraTransform = Camera.main.gameObject.transform;
        playerTransform=GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ScriptHubOneSecondUpdate()
    {
        FunctionsOfOrientationTriggerColliderOnPlayer();
    }

    public void FunctionsOfOrientationTriggerColliderOnPlayer()
    {
        if (mainCameraTransform == null)
            Debug.LogError(" mainCamera не назначена в скрипте.\nmainCamera is not assigned in the script.");
        if (playerTransform == null)
            Debug.LogError("playerTransform не назначена в скрипте.\nplayerTransform is not assigned in the script.");

        Vector3 direction = mainCameraTransform.position- playerTransform.position;
        Debug.DrawRay(playerTransform.position, direction, Color.green, 1f);
        RaycastHit[] hits = Physics.RaycastAll(playerTransform.position, direction, Vector3.Distance(playerTransform.position, mainCameraTransform.position));

        List<EnvirumentTranslucent_Slave> thisRaycastHit = new List<EnvirumentTranslucent_Slave>();

        foreach (var item in hits)
        {
            GameObject obj = item.collider.gameObject;
            if (obj.tag != "Player")
            {
                if (obj.transform.parent != null)
                {
                    EnvirumentTranslucent_Slave slave = obj.transform.parent.GetComponent<EnvirumentTranslucent_Slave>();
                    if (slave != null)
                    { 
                        thisRaycastHit.Add(slave);
                        slave.TranslucentON();
                    }
                }
            }
        }
        foreach (var item in previousRaycastHit)
        {
            if (thisRaycastHit.Contains(item)==false)
            {
                item.TranslucentOFF();
            }
        }
        previousRaycastHit = thisRaycastHit;

        //Debug.DrawRay(playerTransform.position, direction, Color.red, Vector3.Distance(playerTransform.position, mainCameraTransform.position));
    }



    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag!="Player")
    //    {
    //        if (other.gameObject.transform.parent != null)
    //        {
    //            EnvirumentTranslucent_Slave slave = other.gameObject.transform.parent.GetComponent<EnvirumentTranslucent_Slave>();
    //            if (slave != null)
    //                slave.TranslucentON();
    //            Debug.Log("her");
    //        } 
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.transform.parent != null)
    //    {
    //        EnvirumentTranslucent_Slave slave = other.gameObject.transform.parent.GetComponent<EnvirumentTranslucent_Slave>();
    //        if (slave != null)
    //            slave.TranslucentOFF();
    //        Debug.Log("her");
    //    }
    //}
}

