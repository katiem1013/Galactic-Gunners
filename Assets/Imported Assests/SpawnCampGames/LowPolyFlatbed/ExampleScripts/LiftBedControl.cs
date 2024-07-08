using UnityEngine;

namespace SpawnCampGames
{
    // Just drop script on truck, assign the liftBed gameObject variable to the "Flatbed" gameobject
    // To lift or lower bed just modify the LiftBedControl.tilt float variable

    public class LiftBedControl : MonoBehaviour
    {
        public GameObject liftBed; // the parent gameobject "Flatbed"
        public float minAngle = 0f; // lift gate sits flat at 0deg on its local X
        public float maxAngle = -75f; // lift gate is straight up and down @ -90deg on its local X

        [Range(0f,1f)]
        public float tilt; // 0f is minAngle or 0 % of the maxAngle
                           // 1f is maxAngle or 100 % of the maxAngle

        private void Update()
        {
            var liftBedRot = Vector3.zero; // set a new Vector3 of (0,0,0) on the (x,y,z)
            liftBedRot.x = Mathf.Lerp(minAngle, maxAngle, tilt); // set X according to liftBedControl (0f - 1f)
            liftBed.transform.localRotation = Quaternion.Euler(liftBedRot); // set local rotation of "Flatbed" to the new rotation
        }
    }
}
