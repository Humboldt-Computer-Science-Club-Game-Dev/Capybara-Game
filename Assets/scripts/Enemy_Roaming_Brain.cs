using UnityEngine;

[RequireComponent(typeof(Enemy_Movement_Brain))]
public class Enemy_Roaming_Brain : MonoBehaviour
{
    public float MovementSpeed;
    Transform PathContainer;

    public bool LoopThroughPoints;
    public bool StartAtFirstPointOnAwake;

    private Transform[] _points;

    private int _currentTargetIdx;

    Enemy_Movement_Brain movementBrain;

    public float roamCooldown = 20f;
    public float roamCooldownTimer = 0f;

    private void Awake()
    {
    }

    void Start()
    {
        movementBrain = GetComponent<Enemy_Movement_Brain>();
    }

    private void Update()
    {
        roamCooldownTimer += (1f * Time.deltaTime);
        if (!isTimeToRoam()) return;

        roamCooldownTimer = 0;
        //Instantiates PathContainer prefab at this position
        PathContainer = Instantiate((GameObject)Resources.Load("prefabs/patrol_path_container_var_01", typeof(GameObject)), transform.position, transform.rotation).transform;
        _points = PathContainer.GetComponentsInChildren<Transform>();
        //Will set movement brain's movement state to roaming and make this function return false
        movementBrain.startRoaming();

        var distance = Vector3.Distance(transform.position, _points[_currentTargetIdx].position);
        if (Mathf.Abs(distance) < 0.1f)
        {
            _currentTargetIdx++;
            if (_currentTargetIdx >= _points.Length)
            {
                // we reached the end of the path
                _currentTargetIdx = LoopThroughPoints ? 0 : _points.Length - 1;
                if(!LoopThroughPoints) {
                    _currentTargetIdx = 0;
                    movementBrain.onFinishedRoaming();
                    Destroy(PathContainer.gameObject);
                }
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, _points[_currentTargetIdx].position, MovementSpeed * Time.deltaTime);       
    }
    private void handleTimer(){
        if (!movementBrain.getMovementState() == Enemy_Movement_Brain.Movement.oscillating) return;
        roamCooldownTimer += (1f * Time.deltaTime);
    }
    private bool isTimeToRoam(){
        if (!_points == null 
            || _points.Length == 0 
            && roamCooldownTimer >= roamCooldown) return true;
        return false;
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