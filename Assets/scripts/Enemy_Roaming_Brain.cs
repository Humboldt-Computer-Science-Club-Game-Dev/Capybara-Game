using UnityEngine;
using System;

/* 
    Enemy movement brain is required because enemy roaming 
    brain is an extension of Enemy movement brain 
*/
[RequireComponent(typeof(Enemy_Movement_Brain))]

public class Enemy_Roaming_Brain : MonoBehaviour
{
    public float MovementSpeed = 4;
    public Transform[] _points;
    public float roamCooldown = 10f;
    public float roamCooldownTimer;
    Transform PathContainer;
    int _currentTargetIdx = 0;
    Enemy_Movement_Brain movementBrain;
    Health health;
    const string ROAMING_PREFAB_PATH_BASE = "prefabs/patrol_path_container_var_0";
    void Start()
    {
        getComponents();

        // this timer is initialized to a random value so that not all enemies roam at the same time
        roamCooldownTimer = Random_Number_Generator.randomPositiveFloatUnder(roamCooldown);
    }
    private void Update()
    {
        handleTimer();
        if (isTimeToRoam()) roam();

               
    }
    void roam(){
        //Will set movement brain's movement state to roaming
        movementBrain.startRoaming();

        roamCooldownTimer = 0;

        //Instantiates PathContainer prefab at this position
        if(PathContainer == null){
            PathContainer = Instantiate((GameObject)Resources.Load(ROAMING_PREFAB_PATH_BASE + createPathVariantNumber(), typeof(GameObject)), transform.position, transform.rotation).transform;
            _points = PathContainer.GetComponentsInChildren<Transform>();
        }

        // Moves the enemy towards the current target point in path array
        transform.position = Vector3.MoveTowards(transform.position, _points[_currentTargetIdx].position, MovementSpeed * Time.deltaTime);

        // Distance is the distance between the enemy and the current target point in path array
        var distance = Vector3.Distance(transform.position, _points[_currentTargetIdx].position);

        if (Mathf.Abs(distance) < 0.1f) _currentTargetIdx++;
        if (_currentTargetIdx >= _points.Length) finishRoaming();
    }
    void finishRoaming(){
        _currentTargetIdx = 0;

        //Will set movement brain's movement state to oscillating
        movementBrain.onFinishedRoaming();

        Destroy(PathContainer.gameObject);
    }
    void getComponents(){
        movementBrain = GetComponent<Enemy_Movement_Brain>();
        health = GetComponent<Health>();
    }
    /* 
        There are 6 different path variants, so this method will return a random number between 1 and 6. 
        This number will be used to instantiate the correct path prefab. 
    */
    private int createPathVariantNumber(){
        return (int)Math.Round((double)UnityEngine.Random.Range(1f, 6.4f));
    }

    // This method will increment the roamCooldownTimer by 1 every second if the enemy is oscillating
    private void handleTimer(){
        if (!(movementBrain.getMovementState() == Enemy_Movement_Brain.Movement.oscillating)) return;
        roamCooldownTimer += (1f * Time.deltaTime);
    }
    private bool isTimeToRoam(){
        return (roamCooldownTimer >= roamCooldown || movementBrain.isRoaming() && !health.isDead());
    }

    // This method will draw the path in the scene view
    private void OnDrawGizmosSelected()
    {
        if (_points == null || _points.Length == 0) return;
        var idx = 0;
        foreach(var point in _points){
            Gizmos.color = Color.yellow;
            if (idx < _currentTargetIdx)
            {
                Gizmos.color = Color.red;
            }
            
            if (idx > _currentTargetIdx)
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawWireSphere(point.position, 1f);
            idx++;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, _points[_currentTargetIdx].position);
    }
}