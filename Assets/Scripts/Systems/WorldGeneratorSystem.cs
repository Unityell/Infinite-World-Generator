using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WorldGeneratorSystem : MonoBehaviour
{
    [Inject] EventBus                           EventBus;
    [Header("Debug _bioms")]
    [SerializeField] private List<BiomsData>    _bioms;
    private BiomsData                           _currentBiom;
    private string                              _biomLastName = "";
    private List<string>                        _biomName = new List<string>();
    [HideInInspector] public float              CurrentBiomLenght;
    [Header("Pools")]
    public List<Rail>                           RailsPool;
    public List<Decoration>                     DecorationsPool;
    [HideInInspector] public float              CurrentDecorationLenght;
    private List<string>                        _decorationsName = new List<string>();

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
            _currentBiom = _bioms[0];
        }
        else
        {
            _currentBiom = _bioms.Find(x => x.Name == RandomName);
            _biomLastName = _currentBiom.Name;            
        }

        CurrentBiomLenght = Random.Range(_currentBiom.MinBiomLenght, _currentBiom.MaxBiomLenght);    
        BiomInfoSignal Signal = new BiomInfoSignal(_currentBiom.Name);
        EventBus.Invoke(Signal);      
    }

    public Decoration GetDecoration()
    {
        if(_bioms.Count == 0) return null;

       _decorationsName.Clear();

        for (int i = 0; i < _currentBiom.DecorationData.Decorations.Count; i++)
        {
            for (int a = 0; a < _currentBiom.DecorationData.Decorations[i].SpawnChance; a++)
            {
               _decorationsName.Add(_currentBiom.DecorationData.Decorations[i].GetName());
            }
        }

        string RandomName =_decorationsName[Random.Range(0,_decorationsName.Count)];

        var RandomUnit = _currentBiom.DecorationData.Decorations.Find(x => x.GetName() == RandomName);
        
        var Tree = DecorationsPool.Find(x => x.GetName() == RandomUnit.GetName() && !x.gameObject.activeInHierarchy);

        if(!Tree)
        {
            Tree = Instantiate(RandomUnit);
            DecorationsPool.Add(Tree);
        }
        else
        {
            Tree.gameObject.SetActive(true);
        }

        CurrentDecorationLenght = Random.Range(Tree.SpawnDistanceMin, Tree.SpawnDistanceMax);  

        return Tree;
    }
}