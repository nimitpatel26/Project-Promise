using UnityEngine;
using System.Collections;

/*
NOTE: In order for this script to work, several things must be done. First, create an child object of this object. This object will be where this object holds other object. Color coat the child
    and move to the grab location desired. Then, drag the child into the "Holdpoint" in the this script's component. Objects that want to be grabbed must have a tag set to "Grabbable". Currently, 
    ONLY BOX COLLIDERS work for this. NEED TO BE FIXED EVENTUALLY

   P.S. This code is very ugly. Prepare yourself if you're reading it.
*/
public class Grab : MonoBehaviour {

    private bool grabbed;
    RaycastHit2D hit;
    RaycastHit2D hit1;
    public float distance = 2f;
    public Transform holdpoint;
    float originalMass;
    public float lowerRayPosition =.5f;
    bool upperIsGrabbed;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        //this is ugly code....very inefficient, look back at this later please.
        if (Input.GetKeyDown(KeyCode.B))
        {

            if (!grabbed)
            {
                //grab
                
                Physics2D.queriesStartInColliders = false;
                hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);
                hit1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - lowerRayPosition), Vector2.right * transform.localScale.x, distance);
                //if upper collider hit something
                if (hit.collider != null && hit.collider.gameObject.tag.Equals("Grabbable"))
                {
                    grabbed = true;
                    upperIsGrabbed = true;
                }
                //if lower collider hit something
                if (hit1.collider != null && hit1.collider.gameObject.tag.Equals("Grabbable"))
                {

                    grabbed = true;
                    upperIsGrabbed = false;
                }
                //had trouble combining both 'if' statements because if one hits something and the other is null, it will still go on to check to see if 
                //the object is grabbable which will result in a NullReferenceException. Fix later. 
            }
            else
            {
                //throw
                if (upperIsGrabbed)
                    hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                else
                    hit1.collider.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                grabbed = false;
                


            }
        }

        //lol this is not efficient at all but i have no time so this will do for now. 
        if (grabbed)
        {
            //if both collider hit something, randomly choose one of the object it hits. 
            if (hit.collider != null && hit.collider != null && hit.collider.gameObject.tag.Equals("Grabbable") && hit1.collider.gameObject.tag.Equals("Grabbable"))
            {
                float decider = Random.Range(0, 10f);
                if (decider >= 5f)
                {
                    hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    hit.collider.gameObject.GetComponent<Transform>().rotation.Set(0f, 0f, 0f, 0f);
                    hit.collider.gameObject.transform.position = holdpoint.position;
                }
                else
                {
                    hit1.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    hit1.collider.gameObject.GetComponent<Transform>().rotation.Set(0f, 0f, 0f, 0f);
                    hit1.collider.gameObject.transform.position = holdpoint.position;
                }

            }

            //if its just the top collider
            else if (hit.collider != null && hit.collider.gameObject.tag.Equals("Grabbable"))
            {

                hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                hit.collider.gameObject.GetComponent<Transform>().rotation.Set(0f, 0f, 0f, 0f);
                hit.collider.gameObject.transform.position = holdpoint.position;

            }

            //if its just the bottom collider
            else
            {
                hit1.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                hit1.collider.gameObject.GetComponent<Transform>().rotation.Set(0f, 0f, 0f, 0f);
                hit1.collider.gameObject.transform.position = holdpoint.position;

            }
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,transform.position + Vector3.right * transform.localScale.x * distance);
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - lowerRayPosition, transform.position.z), new Vector3(transform.position.x, transform.position.y - lowerRayPosition, transform.position.z) + Vector3.right * transform.localScale.x * distance);
    }
}
