using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePowerUp : MonoBehaviour
{

	#region Settings
	float rotationSpeed = 99.0f;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.player))
        {
            Destroy(gameObject.transform.parent.gameObject);
            ApplyPowerUp(other.gameObject.GetComponent<Player>());
        }
    }

    protected virtual void ApplyPowerUp(Player player) { }

    void Update ()
	{
		transform.Rotate(Vector3.back * Time.deltaTime * this.rotationSpeed);
	}
}
