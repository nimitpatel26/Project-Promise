using UnityEngine;
using System.Collections;

/*
NOTES:
-For the jumping mechanism to work, add an empty object under the parent of this object and place it on the very bottom-middle point of the object. 
    Drag the new child object to "Ground Point" in this script component. After that, set "Ground Mask" to the same layer as the ground object. 
    The child object is suppose to act as a sensor to detect if the object is on the ground.

*/
public class Movement : MonoBehaviour
{

    public int movespeed;
    public int jumpHeight;
    public SpriteRenderer spriteRen;

    public Transform groundPoint;
    public float radius;
    public LayerMask groundMask;
    bool isGrounded;
    bool facing = true;

    Rigidbody2D rb2D;
    Animator anim;
    // Use this for initialization
    void Start()
    {

        rb2D = GetComponent<Rigidbody2D>(); //gets the rigidbody off of the character
        spriteRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal") * movespeed, rb2D.velocity.y);
        rb2D.velocity = moveDir;
        anim.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, radius, groundMask);


        if (Input.GetAxisRaw("Horizontal") == -1 && facing == true)
        {

            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            facing = false;

        }
        else if (Input.GetAxisRaw("Horizontal") == 1 && facing == false)
        {

            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            facing = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2D.AddForce(new Vector2(0, jumpHeight));
            anim.SetBool("Jump", true);
            
        }

        else if (isGrounded)
        {
            anim.SetBool("Jump", false);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundPoint.position, radius);
    }
}