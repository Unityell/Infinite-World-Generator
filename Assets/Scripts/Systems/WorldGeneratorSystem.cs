using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WorldGeneratorSystem : MonoBehaviour
{
    [Inject] Factory                            Factory;
    [Inject] EventBus                           EventBus;
    [Header("Debug _bioms")]
    [SerializeField] private List<BiomsData>    _bioms;
    [ReadOnly] public BiomsData                 CurrentBiom;
    private string                              _biomLastName = "";
    [HideInInspector] public float              CurrentBiomLenght;

    [Header("Other stuff")]
    [SerializeField] Coin                       Coin;
    [SerializeField] PickUp[]                   PickUps;

    [Header("Ground")]
    public MeshRenderer                         Ground;
    List<Vector3> Points = new List<Vector3>();

    private void Start()
    {
        ChangeBiom();
        BuildStartRoad();
    }

    void BuildStartRoad()
    {
        float Number = 102;

        for (int i = 0; i < 35; i++)
        {
            Factory.Create<Road>(CurrentBiom.RoadsData.Roads[Random.Range(0, CurrentBiom.RoadsData.Roads.Count)].gameObject, Vector3.right * Number, Quaternion.Euler(0, 90, 0));
            Number -= 6;
        }
    }

    public void ChangeBiom()
    {
        if (_bioms.Count == 0) return;

        float totalChance = 0;

        foreach (var biom in _bioms)
        {
            if (biom.Name != _biomLastName)
            {
                totalChance += biom.Chance;
            }
        }

        float randomValue = Random.Range(0, totalChance);

        float cumulativeChance = 0;

        foreach (var biom in _bioms)
        {
            if (biom.Name != _biomLastName)
            {
                cumulativeChance += biom.Chance;

                if (randomValue < cumulativeChance)
                {
                    CurrentBiom = biom;
                    _biomLastName = CurrentBiom.Name;
                    break;
                }
            }
        }

        CurrentBiomLenght = Random.Range(CurrentBiom.MinBiomLenght, CurrentBiom.MaxBiomLenght);

        BiomInfoSignal signal = new BiomInfoSignal(CurrentBiom.Name);
        EventBus.Invoke(signal);
    }

    public List<Vector3> GetPoints()
    {
        Points.Clear();
        
        float Number = 3.75f;

        var Multiply = Application.platform != RuntimePlatform.Android ? 0.5f : 1; 

        for (int i = 1; i < CurrentBiom.LineLenght + 1; i++)
        {
            Number += 7.5f;

            Vector3 Point = Vector3.zero;
            Point.x = (Ground.transform.localScale.x * 10) / 2 - 3.75f;
            Point.z = Number + 7.5f;
            Points.Add(Point);
            Point.z *= -1;
            Points.Add(Point);
        }

        return Points;
    }

    public List<Decoration> GetDecorations()
    {
        if (_bioms.Count == 0) return null;

        List<DecorationSettings> decorations = CurrentBiom.DecorationData.Decorations;

        float totalChance = 0f;

        foreach (var decoration in decorations)
        {
            if (decoration != null)
            {
                totalChance += decoration.SpawnChance;
            }
        }

        int randomNumberOfObjectsInLine = Mathf.CeilToInt(Random.Range(CurrentBiom.DecorationData.MinCountInLine, CurrentBiom.DecorationData.MaxCountInLine) * 2f);

        List<Decoration> objectsInLine = new List<Decoration>();

        for (int i = 0; i < randomNumberOfObjectsInLine; i++)
        {
            float randomValue = Random.Range(0f, totalChance);
            float cumulativeChance = 0f;

            foreach (var decoration in decorations)
            {
                if (decoration != null)
                {
                    cumulativeChance += decoration.SpawnChance;

                    if (randomValue < cumulativeChance)
                    {
                        var Object = Factory.Create<Decoration>(decoration.Prefab.gameObject);
                        Object.SetupScale(decoration.ScaleMin, decoration.ScaleMax);
                        
                        objectsInLine.Add(Object);
                        break;
                    }
                }
            }
        }

        return objectsInLine;
    }

    public Road GetRoad()
    {
        if (_bioms.Count == 0) return null;
        return Factory.Create<Road>(CurrentBiom.RoadsData.Roads[Random.Range(0, CurrentBiom.RoadsData.Roads.Count)].gameObject);
    }

    public Coin GetCoin()
    {
        if(_bioms.Count == 0) return null;

        return Factory.Create<Coin>(Coin.gameObject);
    }

    public PickUp GetPickUp()
    {
        if(_bioms.Count == 0) return null;

        return Factory.Create<PickUp>(PickUps[Random.Range(0, PickUps.Length)].gameObject);
    }

    public Obstacle GetObstacle()
    {
        if(_bioms.Count == 0) return null;  

        return Factory.Create<Obstacle>(CurrentBiom.ObstacleData.Obstacles[Random.Range(0, CurrentBiom.ObstacleData.Obstacles.Count)].gameObject);
    }
}