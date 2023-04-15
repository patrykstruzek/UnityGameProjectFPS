using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseX, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion mainRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, mainRotation, smooth * Time.deltaTime);
    }

}
