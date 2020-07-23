#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace Sources
{
    public class JoystickBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public bool Pushed { get; set; }
        public Vector3 Direction { get; private set; }
        public float Power { get; private set; }

        [SerializeField] private RectTransform _readyTransform;
        [SerializeField] private RectTransform _powerRootTransform;
        [SerializeField] private RectTransform _powerTransform;
        [SerializeField] private Image[] _images;
        [SerializeField] private float _radius;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Vector2 _readySizeFull;
        [SerializeField] private AnimationCurve _readySizeCurve;
        [SerializeField] private AnimationCurve _powerMultiply;
        [SerializeField] private Transform _camera;
        private bool _dragged;
        private RectTransform _transform;
        private Vector2 _position;
        private float _distance;
        private bool _ready;

        public void SetReady()
        {
            _readyTransform.gameObject.SetActive(true);
            _readyTransform.sizeDelta = _readySizeFull;
            _ready = true;
        }

        void Awake()
        {
            _transform = (RectTransform) transform;
            Game.JoystickBehaviour = this;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _dragged = false;
            if (!_ready) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_transform, eventData.position,
                eventData.pressEventCamera, out _position)) return;

            _distance = Vector3.Distance(Vector3.zero, _position);
            if (_distance > _readySizeFull.x) return;

            _dragged = true;
            _powerRootTransform.gameObject.SetActive(true);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (!_ready || !_dragged) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_transform, eventData.position,
                eventData.pressEventCamera, out _position)) return;

            //_draggedTransform.localPosition = position;
            if (Vector3.Dot(Vector3.right, _position.normalized) >= 0)
                _powerRootTransform.eulerAngles = new Vector3(0, 0, Vector3.Angle(Vector3.down, _position));
            else
                _powerRootTransform.eulerAngles = new Vector3(0, 0, -Vector3.Angle(Vector3.down, _position));

            _distance = Vector3.Distance(Vector3.zero, _position);
            if (_distance > _radius) _distance = _radius;
            var sd = _powerTransform.sizeDelta;
            sd.y = _distance;
            _powerTransform.sizeDelta = sd;

            foreach (var img in _images)
            {
                img.color = _gradient.Evaluate(_distance / _radius);
            }

            _readyTransform.sizeDelta = _readySizeFull * _readySizeCurve.Evaluate(_distance / _radius);
        }

        //private void Update()
        //{
        //    if (_draggedTransform.GetActive())
        //    {
        //        var pos = _draggedTransform.localPosition * -1;
        //        //var target = Vector3.RotateTowards(_camera.forward, new Vector3(pos.x, 0, pos.y), 0, 0);
        //        var target = Quaternion.Euler(_camera.localEulerAngles) * new Vector3(pos.x, 0, pos.y);
        //        //Debug.DrawRay(Game.Get<BallBehaviour>().transform.position, target, Color.red);
        //    }
        //}

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!_ready || !_dragged) return;

            Debug.Log(_dragged);

            _powerRootTransform.gameObject.SetActive(false);
            _readyTransform.gameObject.SetActive(false);

            var pos = _position * -1;
            Power = _distance / _radius;//Vector3.Distance(Vector3.zero, pos) / _radius;
            pos.Normalize();
            Direction = Quaternion.Euler(_camera.localEulerAngles) * new Vector3(pos.x, 0, pos.y);
            Power = _powerMultiply.Evaluate(Power);
            Pushed = true;
            _ready = false;
            _dragged = false;
        }
    }
}