using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldMoveSystem : MonoBehaviour
{
    [SerializeField] 
    private Config                  _config;
    [SerializeField] 
    private WorldGeneratorSystem    Generator; 
    
    [Header("Sides")]
    [SerializeField, ReadOnly] 
    private float                   Velocity;

    [SerializeField] 
    private Color                   Color;
    private float                   ChangeBiomCurrentDistance;
    float NextLineDistance;
    float FrameDistance;
    private void Start()
    {
        _config.CurrentGameSpeed = 0;
        _config.TargetGameSpeed = _config.MaxGameSpeed;
        _config.CurrentDistance = 0;
    }


    private void MoveGround()
    {
        Generator.Ground.material.mainTextureOffset -= Vector2.right * FrameDistance / (Generator.Ground.transform.localScale * 10);  

        foreach (var item in Generator.RailsPool)
        {
            item.transform.position -= Vector3.right * FrameDistance / 20;

            if(item.transform.position.x <= -(Generator.Ground.transform.localScale.x * 10) / 2)
            {
                float Distance = Mathf.Abs(item.transform.position.x) - (Generator.Ground.transform.localScale.x * 10) / 2;
                item.transform.position = (Vector3.right * ((Generator.Ground.transform.localScale.x * 10) / 2)) - Vector3.right * Distance;
            }
        }        
    }

    private void MoveDecorations()
    {
        foreach (var item in Generator.DecorationsPool)
        {
            item.transform.position -= Vector3.right * FrameDistance / 20;

            if(Mathf.Abs(item.transform.position.x) > (Generator.Ground.transform.localScale.x * 10) / 2)
            {
                item.gameObject.SetActive(false);
            }
        }        
    }

    private void MoveEnemies()
    {
        foreach (var item in Generator.EnemiesPool)
        {
            item.transform.position -= Vector3.right * FrameDistance / 20;

            if(Mathf.Abs(item.transform.position.x) > (Generator.Ground.transform.localScale.x * 10) / 2)
            {
                item.gameObject.SetActive(false);
            }
        }        
    }

    private void ChangeBiom()
    {
        ChangeBiomCurrentDistance += FrameDistance / 20;

        if(ChangeBiomCurrentDistance > Generator.CurrentBiomLenght)
        {
            Generator.ChangeBiom();
            ChangeBiomCurrentDistance = 0;
        }        
    }

    private void FixedUpdate()
    {
        Velocity = Mathf.Lerp(Velocity, _config.TargetGameSpeed, Time.fixedDeltaTime * _config.Acceleration);

        _config.CurrentGameSpeed = Velocity;

        FrameDistance = Velocity * Time.fixedDeltaTime;

        NextLineDistance += FrameDistance / 20;

        _config.CurrentDistance += FrameDistance / 20;

        if (NextLineDistance >= 7.5f)
        {
            NextLineDistance -= 7.5f;

            List<Vector3> Points = new List<Vector3>();
            Points.AddRange(Generator.GetPoints());
            Points = Points.OrderBy(p => System.Guid.NewGuid()).ToList();

            List<Decoration> Decorations = new List<Decoration>();
            Decorations.AddRange(Generator.GetDecorations());

            for (int i = 0; i < Points.Count && Decorations.Count > 0;)
            {
                int RandomNumber = Random.Range(0, Decorations.Count);
                Vector3 newPosition = Points[0] + Vector3.right * (FrameDistance / 20 - NextLineDistance);
                Decorations[RandomNumber].transform.position = new Vector3(newPosition.x, Decorations[RandomNumber].transform.position.y, newPosition.z);
                Decorations[RandomNumber].transform.eulerAngles += Vector3.up * Random.Range(0, 360);  
                Decorations[RandomNumber].transform.localScale = Vector3.one * Random.Range(Decorations[RandomNumber].ScaleMin, Decorations[RandomNumber].ScaleMax);  
                Decorations.RemoveAt(RandomNumber);
                Points.RemoveAt(0);
            }

            List<Enemy> Enemies = new List<Enemy>();
            Enemies.AddRange(Generator.GetEnemies());

            for (int i = 0; i < Points.Count && Enemies.Count > 0;)
            {
                int RandomNumber = Random.Range(0, Enemies.Count);
                Vector3 newPosition = Points[0] + Vector3.right * (FrameDistance / 20 - NextLineDistance);
                Enemies[RandomNumber].transform.position = new Vector3(newPosition.x, Enemies[RandomNumber].transform.position.y, newPosition.z);
                Enemies[RandomNumber].transform.eulerAngles += Vector3.up * Random.Range(0, 360);  
                Enemies[RandomNumber].transform.localScale = Vector3.one * Random.Range(Enemies[RandomNumber].ScaleMin, Enemies[RandomNumber].ScaleMax);  
                Enemies.RemoveAt(RandomNumber);                
                Points.RemoveAt(0);
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].gameObject.SetActive(false);
            } 
        }
    

        MoveGround();

        MoveDecorations();

        ChangeBiom();

        MoveEnemies();
    }
}