using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using System.Collections;

public class WorldMoveSystem : MonoBehaviour, IFixedUpdate
{
    [Inject] Pool Pool;
    [Inject] WorldUpdateSystem WorldUpdateSystem;
    [SerializeField] private Config Config;
    [SerializeField] private WorldGeneratorSystem Generator; 
    [SerializeField, ReadOnly] private float ChangeBiomCurrentDistance;
    private Queue<(Transform, Transform)> RepositionQueue = new Queue<(Transform, Transform)>();
    float NextLineDistance;
    float FrameDistance;

    float CoinTimer = 10;
    float ObstacleTimer;
    int Coin;
    int Obstacle;

    Vector3 CoinPosition;

    void Start()
    {
        WorldUpdateSystem.Subscribe(this);
        Config.CurrentDistance = 0;
        Config.CurrentGameSpeed = Config.StartGameSpeed;
    } 

    void MoveCoins()
    {
        foreach (var item in Pool.GetAllOfType<Coin>())
        {
            if(!item.gameObject.activeSelf) continue;

            item.transform.position -= Vector3.right * FrameDistance / 20;

            if(Mathf.Abs(item.transform.position.x) > (Generator.Ground.transform.localScale.x * 10) / 2)
            {
                item.gameObject.SetActive(false);
            }
        }         
    }

    void MoveObstacles()
    {
        foreach (var item in Pool.GetAllOfType<Obstacle>())
        {
            if(!item.gameObject.activeSelf) continue;

            item.transform.position -= Vector3.right * FrameDistance / 20;

            if(Mathf.Abs(item.transform.position.x) > (Generator.Ground.transform.localScale.x * 10) / 2)
            {
                item.gameObject.SetActive(false);
            }
        }         
    }

    void MovePickUp()
    {
        foreach (var item in Pool.GetAllOfType<PickUp>())
        {
            if(!item.gameObject.activeSelf) continue;

            item.transform.position -= Vector3.right * FrameDistance / 20;

            if(Mathf.Abs(item.transform.position.x) > (Generator.Ground.transform.localScale.x * 10) / 2)
            {
                item.gameObject.SetActive(false);
            }
        }         
    }

    void MoveGround()
    {
        Generator.Ground.material.mainTextureOffset -= Vector2.right * FrameDistance / (Generator.Ground.transform.localScale * 10);  
    }

    void MoveRoad()
    {
        foreach (var item in Pool.GetAllOfType<Road>())
        {
            if (!item.gameObject.activeSelf) continue;

            item.transform.position -= Vector3.right * FrameDistance / 20;

            if (Mathf.Abs(item.transform.position.x) > (Generator.Ground.transform.localScale.x * 10) / 2)
            {
                var Road = Generator.GetRoad();
                Road.transform.position = Vector3.up * 1000;
                RepositionQueue.Enqueue((Road.transform, item.transform));
            }
        }

        StartCoroutine(ProcessRepositionQueue());
    }

    IEnumerator ProcessRepositionQueue()
    {
        yield return new WaitForFixedUpdate();

        while (RepositionQueue.Count > 0)
        {
            var (T1, T2) = RepositionQueue.Dequeue();

            T1.position = T2.position;

            float Distance = Mathf.Abs(T1.position.x) - (Generator.Ground.transform.localScale.x * 10) / 2;
            T1.position = (Vector3.right * ((Generator.Ground.transform.localScale.x * 10) / 2)) - Vector3.right * Distance;

            T1.eulerAngles = new Vector3(0, 90, 0);
            T2.gameObject.SetActive(false);
        }
    }

    void MoveDecorations()
    {
        foreach (var item in Pool.GetAllOfType<Decoration>())
        {
            if(!item.gameObject.activeSelf) continue;

            item.transform.position -= Vector3.right * FrameDistance / 20;

            if(Mathf.Abs(item.transform.position.x) > (Generator.Ground.transform.localScale.x * 10) / 2)
            {
                item.gameObject.SetActive(false);
            }
        }        
    }

    void ChangeBiom()
    {
        ChangeBiomCurrentDistance += FrameDistance / 20;

        if(ChangeBiomCurrentDistance > Generator.CurrentBiomLenght)
        {
            Generator.ChangeBiom();
            ChangeBiomCurrentDistance = 0;
        }        
    }

    void NextCellCalculate()
    {
        FrameDistance = Config.CurrentGameSpeed * Time.fixedDeltaTime;

        NextLineDistance += FrameDistance / 20;
    }

    void CurrentGameSpeedCalculate()
    {
        Config.CurrentGameSpeed = Mathf.MoveTowards(Config.CurrentGameSpeed, Config.MaxGameSpeed, Time.fixedDeltaTime * Config.Acceleration);
    }

    void CurrentDistanceCalculate()
    {
        Config.CurrentDistance += FrameDistance / 20;
    }

    void CoinLineCalculate()
    {
        if(Coin <= 0) CoinTimer -= Time.fixedDeltaTime;

        if(CoinTimer <= 0 && Coin == 0)
        {
            Coin = Random.Range(3, 7);
            CoinTimer = Random.Range(1, 3);
            Obstacle = 1;
            ObstacleTimer = CoinTimer / 2;

            switch (Random.Range(0,3))
            {
                case 0 : CoinPosition = Vector3.forward * 6; break;
                case 1 : CoinPosition = Vector3.forward * -6; break;
                case 2 : CoinPosition = Vector3.zero; break;
                default: break;
            }
        }
    }

    List<Vector3> GetPoints()
    {
        List<Vector3> Points = new List<Vector3>();
        Points.AddRange(Generator.GetPoints());
        return  Points.OrderBy(p => System.Guid.NewGuid()).ToList();        
    }

    public void FixedRefresh()
    {
        CurrentGameSpeedCalculate();

        NextCellCalculate();

        CurrentDistanceCalculate();

        CoinLineCalculate();

        if (NextLineDistance >= 7.5f)
        {
            NextLineDistance -= 7.5f;

            if(Coin > 0)
            {
                var Coin = Generator.GetCoin();
                Coin.transform.position = CoinPosition + Vector3.up * 2.5f + Vector3.right * 72f;
                this.Coin--;

                if(Random.value < 0.05f && this.Coin == 0)
                {
                    var PickUp = Generator.GetPickUp();

                    PickUp.transform.position = CoinPosition + Vector3.up * 2.5f + Vector3.right * (72f + 7.5f);
                }
            }
            else if(Obstacle > 0 && ObstacleTimer > CoinTimer)
            {
                var ObstaclePoints = Generator.CurrentBiom.ObstacleData.Points;
                Vector3 ObstaclePos = ObstaclePoints[Random.Range(0, ObstaclePoints.Count)];

                Generator.GetObstacle().transform.position = ObstaclePos + Vector3.right * 72f;
                this.Obstacle --;
            }

            var Points = GetPoints();

            List<Decoration> Decorations = new List<Decoration>(Generator.GetDecorations());

            for (int i = 0; i < Points.Count && Decorations.Count > 0;)
            {
                int RandomNumber = Random.Range(0, Decorations.Count);
                Vector3 newPosition = Points[0] + Vector3.right * (FrameDistance / 20 - NextLineDistance);
                Decorations[RandomNumber].transform.position = new Vector3(newPosition.x, Decorations[RandomNumber].transform.position.y, newPosition.z);
                Decorations[RandomNumber].transform.eulerAngles = Vector3.up * Random.Range(0, 360);
                Decorations[RandomNumber].transform.localScale = Vector3.one * Random.Range(Decorations[RandomNumber].ScaleMin, Decorations[RandomNumber].ScaleMax);  
                Decorations.RemoveAt(RandomNumber);
                Points.RemoveAt(0);
            }
        }
        
        MoveGround();

        MoveRoad();

        MoveDecorations();

        ChangeBiom();

        MoveCoins();

        MoveObstacles();

        MovePickUp();
    }

    void OnDestroy() => WorldUpdateSystem.Unsubscribe(this);
}