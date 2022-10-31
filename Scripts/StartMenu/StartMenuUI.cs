using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuUI : MonoBehaviour
{

    Transform leftPanel;
    // Start is called before the first frame update
    void Start()
    {
        leftPanel = GameObject.Find("LeftPanel").GetComponent<Transform>();

        foreach (Transform child in leftPanel) {
            if (child.name != "Team Logo") {
                StartCoroutine(floatGameObject(child));
            }
        }

    }

    IEnumerator floatGameObject(Transform obj) {

        float speed = 6.0f;
        while (true) {
            Vector3 oldPos = obj.transform.position;

            float obj_x = obj.transform.position.x;
            float obj_y = obj.transform.position.y;
            float obj_z = obj.transform.position.z;
            
            while (obj_y > (oldPos.y - 5f)) {
                obj_y -= speed * Time.deltaTime;
                obj.transform.position = new Vector3(obj_x, obj_y, obj_z);
                yield return null;
            }

            while (obj_y < (oldPos.y)) {
                obj_y += speed * Time.deltaTime;
                obj.transform.position = new Vector3(obj_x, obj_y, obj_z);
                yield return null;
            }

            obj.transform.position = oldPos;
        }
    }

 }
