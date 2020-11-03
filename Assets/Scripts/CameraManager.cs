using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform characterTransform;
    [SerializeField]
    private float offSetZ;
    [SerializeField]
    private float offSetY;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(characterTransform.position.x, characterTransform.position.y + offSetY, characterTransform.position.z + offSetZ);
        this.transform.position = targetPosition;
    }
}
