using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SlingshotController : Unit
{
    [SerializeField, ReadOnly] 
    private EnumBoxState    _state;
    [SerializeField, Range(0.1f, 1)] 
    private float           _maxDistanceMultiplier;
    [SerializeField] 
    private AnimationCurve  _curve = new AnimationCurve();
    [SerializeField] 
    private int             _projectileVelocity;
    [SerializeField] 
    private Transform       _aim;
    private LineRenderer    _lineRenderer;
    private List<Vector3>   _points = new List<Vector3>();
    private Vector3         _touchStartPos;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();      
    }

    public EnumBoxState GetState()
    {
        return _state;
    }

    private void OnMouseDown()
    {
        if(_state == EnumBoxState.Idle)
        _state = EnumBoxState.IsTapped;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            _state = EnumBoxState.OnGround;
        }
    }

    private List<Vector3> GetPoints(Vector3 startPos, Vector3 endPos, AnimationCurve yPos)
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 startingPosition = startPos;
        Vector3 inOneDirect = (endPos - startingPosition) / 100;

        AnimationCurve yPosCopy = new AnimationCurve(yPos.keys);

        float originalFirstInTangent = yPosCopy.keys[0].inTangent;
        float originalLastInTangent = yPosCopy.keys[yPosCopy.length - 1].inTangent;

        yPosCopy.keys[0].inTangent = originalFirstInTangent;
        yPosCopy.keys[yPosCopy.length - 1].inTangent = originalLastInTangent;

        for (int i = 0; i <= 100; i += _projectileVelocity)
        {
            float t = (float)i / 100;
            Vector3 pos = startPos + inOneDirect * i;
            points.Add(new Vector3(pos.x, yPosCopy.Evaluate(t), pos.z));
        }

        Vector3 lastPos = startPos + inOneDirect * 100;
        float originalLastOutTangent = yPosCopy.keys[yPosCopy.length - 1].outTangent;

        points[points.Count - 1] = new Vector3(lastPos.x, endPos.y, lastPos.z);

        yPos.MoveKey(0, new Keyframe(0, startPos.y, originalFirstInTangent, yPosCopy.keys[0].outTangent));
        yPos.MoveKey(yPosCopy.length - 1, new Keyframe(1.0f, endPos.y, originalLastInTangent, originalLastOutTangent));

        return points;
    }

    private void StartDrag(Vector3 mousePosition)
    {
        _aim.gameObject.SetActive(true);
        _lineRenderer.enabled = true;

        _touchStartPos = mousePosition;
    }

    private void ContinueDrag(Vector3 mousePosition)
    {
        Vector3 direction2D = (_touchStartPos - mousePosition).normalized;

        Vector3 direction3D = new Vector3(direction2D.x, 0, direction2D.y).normalized;

        RaycastHit hit;

        Vector3 origin = _aim.position + Vector3.up * 10;
        Vector3 rayDirection = Vector3.down;

        float distanceMultiplier = Vector2.Distance(_touchStartPos, mousePosition) * _maxDistanceMultiplier;
        Vector3 newPosition = transform.position + direction3D * distanceMultiplier;
        newPosition.y = 0.1f;
        newPosition.z = Mathf.Clamp(newPosition.z, transform.position.z + 5f, 1000f);       

        if (Physics.Raycast(origin, rayDirection, out hit, 100))
        {
            _aim.position = Vector3.Lerp(_aim.position, newPosition + Vector3.up * hit.point.y, Time.deltaTime * 10);
        }
        else
        {
            _aim.position = Vector3.Lerp(_aim.position, newPosition, Time.deltaTime * 10);
        }

        if(Vector3.SqrMagnitude(_aim.position - newPosition) > 0.1f * 0.1f)
        {
            _points.Clear();
            _points = GetPoints(transform.position, _aim.position, _curve);

            UpdateLineRenderer();
        }
    }

    private void EndDrag()
    {
        _aim.gameObject.SetActive(false);

        _lineRenderer.enabled = false;

        _state = EnumBoxState.InMove;

        StartCoroutine(MoveByPoints(_points));
    }

    private IEnumerator MoveByPoints(List<Vector3> points)
    {
        int currentIndex = 0;

        while (currentIndex < points.Count)
        {
            transform.position = points[currentIndex];

            if(Physics.OverlapBox(transform.position, Vector3.one * 0.7f, Quaternion.identity).Length > 1)
            {
                _state = EnumBoxState.Idle;
                StopAllCoroutines();
                points.Clear();
            }

            currentIndex++;
            yield return new WaitForFixedUpdate();
        }

        _state = EnumBoxState.Idle;
    }

    private void UpdateLineRenderer()
    {
        _lineRenderer.positionCount = _points.Count;

        for (int i = 0; i < _points.Count; i++)
        {
            _lineRenderer.SetPosition(i, _points[i]);
        }
    }

    void Update()
    {
        if(_state != EnumBoxState.IsTapped) return;

        if (Input.GetMouseButtonDown(0))
        {
            StartDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            ContinueDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }
}