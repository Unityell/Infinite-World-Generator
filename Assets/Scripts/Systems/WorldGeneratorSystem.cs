using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WorldGeneratorSystem : MonoBehaviour
{
    [Inject] EventBus                           EventBus;
    [Header("Debug _bioms")]
    [SerializeField] private List<BiomsData>    _bioms;
    private BiomsData                           CurrentBiom;
    private string                              _biomLastName = "";
    private List<string>                        _biomName = new List<string>();
    [HideInInspector] public float              CurrentBiomLenght;
    [Header("Pools")]
    public List<Rail>                           RailsPool;
    public List<Decoration>                     DecorationsPool;
    private List<string>                        DecorationsName = new List<string>();
    public List<Enemy>                          EnemiesPool;
    private List<string>                        EnemiesName = new List<string>();
    [Header("Ground")]
    public MeshRenderer Ground;

    List<Vector3> Points = new List<Vector3>();

    [SerializeField] Transform DecorationsParent;
    [SerializeField] Transform EnemiesParent;

    private void Start()
    {
        ChangeBiom();
    }

    public void ChangeBiom()
    {
        if(_bioms.Count == 0) return;

        _biomName.Clear();
        for (int i = 0; i < _bioms.Count; i++)
        {
            for (int a = 0; a < _bioms[i].Chance; a++)
            {
                if(_bioms[i].Name == _biomLastName) continue;
                _biomName.Add(_bioms[i].Name);
            }
        }
        string RandomName = _biomName[Random.Range(0, _biomName.Count)];
        if(_bioms.Count == 1)
        {
            CurrentBiom = _bioms[0];
        }
        else
        {
            CurrentBiom = _bioms.Find(x => x.Name == RandomName);
            _biomLastName = CurrentBiom.Name;            
        }

        CurrentBiomLenght = Random.Range(CurrentBiom.MinBiomLenght, CurrentBiom.MaxBiomLenght);    
        BiomInfoSignal Signal = new BiomInfoSignal(CurrentBiom.Name);
        EventBus.Invoke(Signal);      
    }

    public List<Vector3> GetPoints()
    {
        Points.Clear();
        
        float Number = 3.75f;

        for (int i = 1; i < CurrentBiom.LineLenght + 1; i++)
        {
            Number += 7.5f;

            Vector3 Point = Vector3.zero;
            Point.x = (Ground.transform.localScale.x * 10) / 2 - 3.75f;
            Point.z = Number;
            Points.Add(Point);
            Point.z *= -1;
            Points.Add(Point);
        }

        return Points;
    }

    public List<Decoration> GetDecorations()
    {
        if(_bioms.Count == 0) return null;

       DecorationsName.Clear();

        for (int i = 0; i < CurrentBiom.DecorationData.Decorations.Count; i++)
        {
            for (int a = 0; a < CurrentBiom.DecorationData.Decorations[i].SpawnChance; a++)
            {
               DecorationsName.Add(CurrentBiom.DecorationData.Decorations[i].Prefab.GetName());
            }
        }

        int RandomNumberOfObjectsInLine = Random.Range(CurrentBiom.DecorationData.MinCountInLine, CurrentBiom.DecorationData.MaxCountInLine) * 2;

        List<Decoration> ObjectsInLine = new List<Decoration>();

        List<string> RandomNames = new List<string>();

        for (int i = 0; i < RandomNumberOfObjectsInLine; i++)
        {
            RandomNames.Add(DecorationsName[Random.Range(0, DecorationsName.Count)]);
        }

        for (int i = 0; i < RandomNames.Count; i++)
        {
            var RandomUnit = CurrentBiom.DecorationData.Decorations.Find(x => x.Prefab.GetName() == RandomNames[i]);
            
            var Object = DecorationsPool.Find(x => x.GetName() == RandomUnit.Prefab.GetName() && !x.gameObject.activeInHierarchy);

            if(!Object)
            {
                Object = Instantiate(RandomUnit.Prefab);
                Object.Initialization(RandomUnit.ScaleMin, RandomUnit.ScaleMax);
                Object.gameObject.transform.parent = DecorationsParent;
                DecorationsPool.Add(Object);
                ObjectsInLine.Add(Object);
            }
            else
            {
                Object.gameObject.SetActive(true);
                ObjectsInLine.Add(Object);
            }            
        } 

        return ObjectsInLine;
    }

    public List<Enemy> GetEnemies()
    {
        if(_bioms.Count == 0) return null;

        EnemiesName.Clear();

        for (int i = 0; i < CurrentBiom.EnemyData.Enemies.Count; i++)
        {
            for (int a = 0; a < CurrentBiom.EnemyData.Enemies[i].SpawnChance; a++)
            {
               EnemiesName.Add(CurrentBiom.EnemyData.Enemies[i].Prefab.GetName());
            }
        }

        int RandomNumberOfObjectsInLine = Random.Range(CurrentBiom.EnemyData.MinCountInLine, CurrentBiom.EnemyData.MaxCountInLine) * 2;

        List<Enemy> ObjectsInLine = new List<Enemy>();

        List<string> RandomNames = new List<string>();

        for (int i = 0; i < RandomNumberOfObjectsInLine; i++)
        {
            RandomNames.Add(EnemiesName[Random.Range(0, EnemiesName.Count)]);
        }

        for (int i = 0; i < RandomNames.Count; i++)
        {
            var RandomUnit = CurrentBiom.EnemyData.Enemies.Find(x => x.Prefab.GetName() == RandomNames[i]);
            
            var Object = EnemiesPool.Find(x => x.GetName() == RandomUnit.Prefab.GetName() && !x.gameObject.activeInHierarchy);

            if(!Object)
            {
                Object = Instantiate(RandomUnit.Prefab);
                Object.Initialization(RandomUnit.ScaleMin, RandomUnit.ScaleMax);
                Object.gameObject.transform.parent = EnemiesParent;
                EnemiesPool.Add(Object);
                ObjectsInLine.Add(Object);
            }
            else
            {
                Object.gameObject.SetActive(true);
                ObjectsInLine.Add(Object);
            }            
        } 

        return ObjectsInLine;
    }
}