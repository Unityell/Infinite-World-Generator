using UnityEngine;

public class WorldMoveSystem : MonoBehaviour
{
    [SerializeField] 
    private Config                  _config;
    [SerializeField] 
    private WorldGeneratorSystem    _generator;
    [SerializeField] 
    private MeshRenderer            _ground;
    [Header("Right side")]
    [SerializeField] 
    private Transform               _parent;
    [SerializeField] 
    private float                   _distanceBetweenSides;
    [SerializeField] 
    private float                   _sidesWidth;
    [SerializeField] 
    private float                   _sidesHeight;
    [SerializeField, ReadOnly] 
    private float                   _velocity;

    [SerializeField] 
    private Color                   _color;
    private float                   ChangeBiomCurrentDistance;
    private float                   SpawnDecorationCurrentDistance;
    private float                   SpawnEnemyCurrentDistance;

    private void Start()
    {
        _config.CurrentGameSpeed = 0;
        _config.TargetGameSpeed = 0;
        _config.CurrentDistance = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 halfSize = new Vector3(_sidesHeight / 2f, 1f, _sidesWidth / 2f);

        // Первый прямоугольник
        Vector3 center1 = Vector3.forward * (_distanceBetweenSides / 2);
        Debug.DrawLine(center1 + new Vector3(-halfSize.x, 1f, -halfSize.z), center1 + new Vector3(halfSize.x, 1f, -halfSize.z), _color);
        Debug.DrawLine(center1 + new Vector3(halfSize.x, 1f, -halfSize.z), center1 + new Vector3(halfSize.x, 1f, halfSize.z), _color);
        Debug.DrawLine(center1 + new Vector3(halfSize.x, 1f, halfSize.z), center1 + new Vector3(-halfSize.x, 1f, halfSize.z), _color);
        Debug.DrawLine(center1 + new Vector3(-halfSize.x, 1f, halfSize.z), center1 + new Vector3(-halfSize.x, 1f, -halfSize.z), _color);

        // Второй прямоугольник
        Vector3 center2 = -Vector3.forward * (_distanceBetweenSides / 2);
        Debug.DrawLine(center2 + new Vector3(-halfSize.x, 1f, -halfSize.z), center2 + new Vector3(halfSize.x, 1f, -halfSize.z), _color);
        Debug.DrawLine(center2 + new Vector3(halfSize.x, 1f, -halfSize.z), center2 + new Vector3(halfSize.x, 1f, halfSize.z), _color);
        Debug.DrawLine(center2 + new Vector3(halfSize.x, 1f, halfSize.z), center2 + new Vector3(-halfSize.x, 1f, halfSize.z), _color);
        Debug.DrawLine(center2 + new Vector3(-halfSize.x, 1f, halfSize.z), center2 + new Vector3(-halfSize.x, 1f, -halfSize.z), _color);
    }

    private void MoveGround()
    {
        _ground.material.mainTextureOffset -= Vector2.right * _velocity * Time.fixedDeltaTime / (_ground.transform.localScale * 10);  

        foreach (var item in _generator.RailsPool)
        {
            item.transform.position -= Vector3.right * _velocity * Time.fixedDeltaTime / 25;

            if(Mathf.Abs(item.transform.position.x) > _sidesHeight / 2)
            {
                float Distance = Mathf.Abs(item.transform.position.x) - _sidesHeight / 2;
                item.transform.position = (Vector3.right * (_sidesHeight / 2)) - Vector3.right * Distance;
            }
        }        
    }

    private void MoveDecorations()
    {
        SpawnDecorationCurrentDistance += _velocity * Time.fixedDeltaTime / 25;
        
        if(SpawnDecorationCurrentDistance > _generator.CurrentDecorationLenght)
        {
            float Distance = SpawnDecorationCurrentDistance - _generator.CurrentDecorationLenght;
            var NewDecoration = _generator.GetDecoration();

            int RandomNumber = Random.Range(0,2);

            float DBS = RandomNumber == 0 ? _distanceBetweenSides : -_distanceBetweenSides;

            NewDecoration.gameObject.transform.SetParent(_parent);
            Vector3 Position = new Vector3(_sidesHeight / 2 - Distance, 0, DBS / 2 + Random.Range(-_sidesWidth/2, _sidesWidth/2));
            NewDecoration.transform.position = Position;
            NewDecoration.transform.eulerAngles += Vector3.up * Random.Range(0,360);  
            NewDecoration.transform.localScale = Vector3.one * Random.Range(NewDecoration.ScaleMin, NewDecoration.ScaleMax);  
            SpawnDecorationCurrentDistance = 0;      
        }

        foreach (var item in _generator.DecorationsPool)
        {
            item.transform.position -= Vector3.right * _velocity * Time.fixedDeltaTime / 25;

            if(Mathf.Abs(item.transform.position.x) > _sidesHeight / 2)
            {
                item.gameObject.SetActive(false);
            }
        }        
    }

    private void MoveEnemies()
    {
        SpawnEnemyCurrentDistance += _velocity * Time.fixedDeltaTime / 25;
        
        if(SpawnEnemyCurrentDistance > _generator.CurrentEnemyLenght)
        {
            float Distance = SpawnEnemyCurrentDistance - _generator.CurrentEnemyLenght;

            var NewEnemy = _generator.GetEnemy();

            int RandomNumber = Random.Range(0,2);

            float DBS = RandomNumber == 0 ? _distanceBetweenSides : -_distanceBetweenSides;

            NewEnemy.transform.SetParent(_parent);
            Vector3 Position = new Vector3(_sidesHeight / 2 - Distance, NewEnemy.transform.position.y, DBS / 2 + Random.Range(-_sidesWidth / 2 + 5, _sidesWidth / 2 - 5));
            NewEnemy.transform.position = Position;
            SpawnEnemyCurrentDistance = 0;      
        }

        foreach (var item in _generator.EnemiesPool)
        {
            item.transform.position -= Vector3.right * _velocity * Time.fixedDeltaTime / 25;

            if(Mathf.Abs(item.transform.position.x) > _sidesHeight / 2)
            {
                item.gameObject.SetActive(false);
            }
        }        
    }

    private void ChangeBiom()
    {
        ChangeBiomCurrentDistance += _velocity * Time.fixedDeltaTime / 25;

        if(ChangeBiomCurrentDistance > _generator.CurrentBiomLenght)
        {
            _generator.ChangeBiom();
            ChangeBiomCurrentDistance = 0;
        }        
    }

    private void FixedUpdate()
    {
        _velocity = Mathf.Lerp(_velocity, _config.TargetGameSpeed, Time.fixedDeltaTime * _config.Acceleration);

        _config.CurrentGameSpeed = _velocity;
        _config.CurrentDistance += _velocity * Time.fixedDeltaTime / 25;

        MoveGround();

        MoveDecorations();

        ChangeBiom();

        MoveEnemies();
    }
}