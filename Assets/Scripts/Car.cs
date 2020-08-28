using UnityEngine;
 
public class Car : MonoBehaviour
{
    Vector3 pos;
    Vector3 forward;
    Vector3 left;
    Vector3 right;
    public float ForwardDistance = 0;
    public float LeftDistance = 0;
    public float RightDistance = 0;

    // Update is called once per frame
    private void Update()
    {
        forward = transform.TransformDirection(Vector3.forward) * 20;
        left = transform.TransformDirection(new Vector3(.5f,0,1)) * 20;
        right = transform.TransformDirection(new Vector3(-.5f, 0, 1)) * 20;
        pos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Debug.DrawRay(pos, forward, Color.red);
        Debug.DrawRay(pos, left, Color.red);
        Debug.DrawRay(pos, right, Color.red);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        RaycastHit hit2;
        RaycastHit hit3;
        Ray RF = new Ray(pos, forward);
        Ray RL = new Ray(pos, left);
        Ray RR = new Ray(pos, right);

        ForwardDistance = 20;
        LeftDistance = 20;
        RightDistance = 20;

        if (Physics.Raycast(pos, forward, out hit, 20))
        {
            ForwardDistance = hit.distance;
        }
        if (Physics.Raycast(pos, left, out hit2, 20))
        {
            RightDistance = hit2.distance;
        }
        if (Physics.Raycast(pos, right, out hit3, 20))
        {
            LeftDistance = hit3.distance;
        }
        //Debug.Log("L: " + LeftDistance + "   F: " + ForwardDistance + "   R: " + RightDistance);
    }
}
