using UnityEngine;
using System;

[RequireComponent(typeof(Enemy_Movement_Brain))]


public class Enemy_Roaming_Brain : MonoBehaviour
{
    public float MovementSpeed = 4;
    Transform PathContainer;

    public Transform[] _points;

    private int _currentTargetIdx = 1;

    Enemy_Movement_Brain movementBrain;

    public float roamCooldown = 10f;
    public float roamCooldownTimer;

    Health health;

    const string ROAMING_PREFAB_PATH_BASE = "prefabs/patrol_path_container_var_0";

    private void Awake()
    {
    }

    void Start()
    {
        movementBrain = GetComponent<Enemy_Movement_Brain>();
        //TODO: make roamCooldownTimer init to a random value between 0 and roamCooldown
        roamCooldownTimer = Random_Number_Generator.randomPositiveFloatUnder(roamCooldown);
        health = GetComponent<Health>();
    }

    private void Update()
    {
        handleTimer();
        if (!isTimeToRoam()) return;

        roamCooldownTimer = 0;
        //Instantiates PathContainer prefab at this position
        if(PathContainer == null){
            PathContainer = Instantiate((GameObject)Resources.Load(ROAMING_PREFAB_PATH_BASE + createPathVariantNumber(), typeof(GameObject)), transform.position, transform.rotation).transform;
            _points = PathContainer.GetComponentsInChildren<Transform>();
        }
        //Will set movement brain's movement state to roaming and make this function return false
        movementBrain.startRoaming();

        var distance = Vector3.Distance(transform.position, _points[_currentTargetIdx].position);
        if (Mathf.Abs(distance) < 0.1f)
        {
            _currentTargetIdx++;
            if (_currentTargetIdx >= _points.Length)
            {
                // we reached the end of the path
                _currentTargetIdx = 1;
                movementBrain.onFinishedRoaming();
                Destroy(PathContainer.gameObject);
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, _points[_currentTargetIdx].position, MovementSpeed * Time.deltaTime);       
    }
    private int createPathVariantNumber(){
        return (int)Math.Round((double)UnityEngine.Random.Range(1f, 6.4f));
    }
    private void handleTimer(){
        if (!(movementBrain.getMovementState() == Enemy_Movement_Brain.Movement.oscillating)) return;
        roamCooldownTimer += (1f * Time.deltaTime);
    }
    private bool isTimeToRoam(){
        return (roamCooldownTimer >= roamCooldown || movementBrain.isRoaming() && !health.isDead());
    }

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