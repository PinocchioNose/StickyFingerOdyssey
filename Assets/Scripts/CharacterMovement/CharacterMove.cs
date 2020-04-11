using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : CharacterAction
{

    protected new void Start()
    {
        base.Start();

        if (CharacterTransform == null)
            CharacterTransform = gameObject.transform;
    }


    protected new void Update()
    {
        GetDirection();
        
        base.Update();
    }



    private void GetDirection()
    {
        #region move through keyboard
        // reset to 0
        speedForward = Vector3.zero;
        speedBack = Vector3.zero;
        speedLeft = Vector3.zero;
        speedRight = Vector3.zero;
        //speedUp = Vector3.zero;
        speedDown = Vector3.zero;

        // get keyboard input
        if (Input.GetKey(KeyCode.W)) speedForward = CharacterTransform.forward;
        if (Input.GetKey(KeyCode.S)) speedBack = -CharacterTransform.forward;
        if (Input.GetKey(KeyCode.A)) speedLeft = -CharacterTransform.right;
        if (Input.GetKey(KeyCode.D)) speedRight = CharacterTransform.right;

        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            if(jumpStrength < maxStrength) ++jumpStrength;
        }
        else if (!Input.GetKey(KeyCode.Space) && jumpStrength > 10 && !isJumping)
        {
            isJumping = true;
            initPosY = CharacterTransform.transform.position.y;

            originJumpSpeed = jumpStrength / 20.0f;
            //Debug.Log(CharacterTransform.up);
            speedUp = CharacterTransform.up * (jumpStrength / 20.0f);
            jumpStrength = 0;
        }

        


        direction = speedForward + speedBack + speedLeft + speedRight + speedUp + speedDown;
        #endregion

        #region rotate
        // camera toward
        CharacterTransform.RotateAround(CharacterTransform.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        //CharacterTransform.RotateAround(CharacterTransform.position, CharacterTransform.right, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime);

        // change direction
        direction = V3RotateAround(direction, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        //direction = V3RotateAround(direction, CharacterTransform.right, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime);
        #endregion
    }
}
