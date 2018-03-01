using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GestureTouch : MonoBehaviour
{

    private GestureTouch() { }
    private static GestureTouch _instance = null;
    public static GestureTouch Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    private GameObject _curModel;


    private Vector3 _ModelScreenPos;
    private Vector3 _ModelWorldPos;
    private Vector3 _MouseScreenPos;
    private Vector3 _Offset;

    private bool _Moved;

    private bool _Selected;

    private float _Scale = 5.0f;
    private float _ZoomDifference = 0;
    private float _ZoomSpeed = 100f;
    private float _ZoomMin = 1.0f;
    private float _ZoomMax = 2.0f;

    private float _StartAngle;
    private float _LastAngle;
    private Quaternion _curModelRotation;
    private Quaternion q = Quaternion.identity;
    private float _TurnAngle;

    private float _xMove;
    private float _yMove;

    public float _UpdateRotate = 220.00f;

    private Vector2 _TouchBegan = Vector2.zero;
    private Vector2 _TouchEnd = Vector2.zero;
    [Header("滑动误差")]
    [Range(0, 10)]
    public float _DragTolerance = 10.0f;
#if UNITY_EDITOR
    private float _MoveSpeed = 1.0f;
#elif UNITY_ANDROID
        private float _MoveSpeed = 2.0f;  
#elif UNITY_IPHONE
            private float _MoveSpeed = 1.0f;  
#endif



    public bool OneFingerRotate = true;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        _TurnAngle = 0;
        subscribeEvent();
        Input.gyro.enabled = true;
    }

    public static void subscribeEvent()
    {

    }
    public static void UnsubscribeEvent()
    {

    }

    void OnDestroy()
    {
        UnsubscribeEvent();
        Input.gyro.enabled = false;
    }

    GameObject getClosestObject(Vector3 _MousePos)
    {
        GameObject _go = null;

        List<GameObject> OnCardModelList = new List<GameObject>();

        if (OnCardModelList.Count > 0)
        {
            float _dis = 0;
            Vector3 p0 = new Vector3(_MousePos.x, _MousePos.y, 0f);

            foreach (var dic in OnCardModelList)
            {

                GameObject _tempGo = dic;

                Vector3 p1 = Camera.main.WorldToScreenPoint(_tempGo.transform.position);
                p0.z = p1.z;

                float _tempDis = Vector3.Distance(p0, p1);


                if (_dis == 0 || _dis > _tempDis)
                {

                    _dis = _tempDis;
                    _go = _tempGo;
                }

            }

        }
        else
        {
        }

        return _go;
    }



    void EXLateUpdate_AR_Touch_Controller()
    {

#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000))
            {

                _curModel = hit.collider.gameObject;


                _ModelScreenPos = Camera.main.WorldToScreenPoint(_curModel.transform.position);

                _MouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _ModelScreenPos.z);

                _Offset = _curModel.transform.position - Camera.main.ScreenToWorldPoint(_MouseScreenPos);

                _Selected = true;
                _Moved = false;

                Debug.DrawLine(ray.origin, hit.point, Color.red);

            }
        }
#endif

        switch (Input.touchCount)
        {
            case 1:
                {

                    Touch touch = Input.GetTouch(0);

                    switch (touch.phase)
                    {

                        case TouchPhase.Began:
                            {
                                _TouchBegan = touch.position;

                                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                                RaycastHit hit;
                                //Physics.Raycast(ray, out hit, 10000);  
                                //Debug.DrawLine(ray.origin, hit.point, Color.red);  
                                if (Physics.Raycast(ray, out hit, 10000))
                                {

                                    _curModel = hit.collider.gameObject;


                                    _ModelScreenPos = Camera.main.WorldToScreenPoint(_curModel.transform.position);

                                    _MouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _ModelScreenPos.z);

                                    _Offset = _curModel.transform.position - Camera.main.ScreenToWorldPoint(_MouseScreenPos);

                                    _Selected = true;
                                    _Moved = false;

                                    Debug.DrawLine(ray.origin, hit.point, Color.red);

                                }
                                else if (OneFingerRotate && !_Selected)
                                {

                                    _Selected = false;
                                    _curModel = getClosestObject(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                                    _curModelRotation = _curModel.transform.localRotation;
                                    _xMove = 0;
                                    _yMove = 0;
                                }

                            }
                            break;

                        case TouchPhase.Ended:
                            {
                                if (_Selected)
                                {
                                    _TouchEnd = touch.position;

                                    if (!_Moved || (_DragTolerance > Vector2.Distance(_TouchBegan, _TouchEnd)))
                                    {
                                    }
                                }

                                _Selected = false;
                                _curModel = null;
                            }
                            break;

                        case TouchPhase.Moved:
                            {

                                if (_curModel != null)
                                {
                                    if (_Selected && (touch.deltaPosition.x != 0 || touch.deltaPosition.y != 0))
                                    {
                                        _Moved = true;
                                        _MouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _ModelScreenPos.z);
                                        _ModelWorldPos = Camera.main.ScreenToWorldPoint(_MouseScreenPos) + _Offset;
                                        _curModel.transform.position = _ModelWorldPos;

                                    }
                                    else if (OneFingerRotate && _Selected == false && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                                    {
                                        Vector2 touchDeltaPostion = touch.deltaPosition;

                                        _xMove += touchDeltaPostion.x * _MoveSpeed;
                                        _yMove += touchDeltaPostion.y * _MoveSpeed;

                                        q = Quaternion.AngleAxis(-_xMove, Vector3.up);


                                        var rotation = Quaternion.Euler(_yMove, _xMove, 0);
                                        _curModel.transform.localRotation = rotation;
                                        _curModel.transform.localRotation = _curModelRotation * q;//*******************************【改动】  

                                    }

                                }

                            }
                            break;
                    }
                }
                break;

            //双指  
            case 2:
                {

                    Touch touch0 = Input.GetTouch(0);
                    Touch touch1 = Input.GetTouch(1);

                    float _zoomTempDifference;

                    if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                    {

                        _curModel = getClosestObject(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                        if (_curModel != null)
                        {
                            _Scale = _curModel.transform.localScale.x;
                            _StartAngle = GetAngle(touch0.position, touch1.position);
                            _curModelRotation = _curModel.transform.rotation;
                        }
                        _Selected = false;
                    }

                    if ((touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved) && _curModel != null)
                    {

                        //缩放  
                        _zoomTempDifference = (touch0.position - touch1.position).magnitude;

                        if (_ZoomDifference == 0)
                            _ZoomDifference = _zoomTempDifference;

                        _Scale = _Scale - (_ZoomDifference - _zoomTempDifference) / _ZoomSpeed;
                        _ZoomDifference = _zoomTempDifference;
                        _Scale = Mathf.Clamp(_Scale, _ZoomMin, _ZoomMax);
                        _curModel.transform.localScale = new Vector3(_Scale, _Scale, _Scale);



                        if (!OneFingerRotate)
                        {
                            _TurnAngle = GetAngle(touch0.position, touch1.position);
                            _LastAngle = _TurnAngle - _StartAngle;
                            _LastAngle = _LastAngle * 2.0f;

                            q = Quaternion.AngleAxis(-_LastAngle, Vector3.up);

                            _curModel.transform.rotation = _curModelRotation * q;
                        }



                    }

                    if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended)
                    {
                        _ZoomDifference = 0;
                        _curModel = null;
                    }
                }

                break;
        }

        //if (CartoonPlot.CurrentModel != null && !DefaultTrackableEventHandlerSZQ.b_OnCard)  
        //{  
        //    Vector3 gravity = SystemInfo.supportsGyroscope ? Input.gyro.gravity : Input.acceleration;  
        //    //CartoonPlot.CurrentModel.transform.rotation = Quaternion.Euler(Vector3.right * (Vector3.Angle(Vector3.back, gyro.gravity) - 90)) * Quaternion.Euler(Vector3.up * 220);  
        //    CartoonPlot.CurrentModel.transform.rotation = Quaternion.Euler(Vector3.right * (Vector3.Angle(Vector3.back, gravity) - 90)) * Quaternion.Euler(Vector3.up * _UpdateRotate);  
        //}  
    }


    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }



    private float GetAngle(Vector2 pos1, Vector2 pos2)
    {
        Vector2 from = pos2 - pos1;

        Vector2 to = new Vector2(1, 0);

        float _angle = Vector2.Angle(from, to);

        Vector3 _cross = Vector3.Cross(from, to);

        if (_cross.z > 0)
        {
            _angle = 360f - _angle;
        }

        return _angle;
    }
}