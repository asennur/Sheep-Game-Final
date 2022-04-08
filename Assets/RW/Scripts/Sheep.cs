using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float runSpeed;
    public float gotHayDestroyDelay;
    private bool hitByHay;

    public float dropDestroyDelay;
    private Collider myCollider;
    private Rigidbody myRigidbody;

    private SheepSpawner sheepSpawner;

    public float heartOffset; 
    public GameObject heartPrefab; 


    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }

    private void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
    }


    private void HitByHay()
    {
        sheepSpawner.RemoveSheepFromList(gameObject);
        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
        TweenScale tweenScale = gameObject.AddComponent<TweenScale>(); ; 
        tweenScale.targetScale = 0; 
        tweenScale.timeToReachTarget = gotHayDestroyDelay; 
        hitByHay = true;
        runSpeed = 0;
        SoundManager.Instance.PlaySheepHitClip();
        GameStateManager.Instance.SavedSheep();

        Destroy(gameObject, gotHayDestroyDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hay") && !hitByHay)
        {
            Destroy(other.gameObject);
            HitByHay();
        }
        else if (other.CompareTag("DropSheep"))
        {
            Drop();
        }
    }

    private void Drop()
    {
        sheepSpawner.RemoveSheepFromList(gameObject);
        GameStateManager.Instance.DroppedSheep();
        myRigidbody.isKinematic = false;
        myCollider.isTrigger = false;
        Destroy(gameObject, dropDestroyDelay);
        SoundManager.Instance.PlaySheepDroppedClip();
    }
}