using System;
using System.Collections;
using ScriptableData;
using UnityEngine;
using Utils;

namespace View
{
    public class MissileView : MonoBehaviour
    {
        public event Action<MissileView> OnFlyEnd;
        
        [SerializeField] private ParticleSystem engineParticleSystem;
        [SerializeField] private GameObject _body;
        [SerializeField] private MissileData _data;
        private GameObject _explosionParticleSystem;
        private Rigidbody _rigidbody;
        private Vector3 _target;
        private float _timeBeforeStartEngine;
        private float _timeBeforeEngineStop;
        private float _initialImpulse;
        private float _engineAcceleration;
        private float _rotationSpeed;
        private float _explosionArea;
        private float _explosionForce;
        private float _explosionDelay;
        private float _scaleModifier;
        private float _explosionDestroy;
        private Transform _planetTransform;

        private void Awake()
        {
            _rigidbody = GetComponentInParent<Rigidbody>();
            _timeBeforeStartEngine = _data.timeBeforeStartEngine;
            _timeBeforeEngineStop = _data.timeBeforeEngineStop;
            _initialImpulse = _data.initialImpulse;
            _engineAcceleration = _data.engineAcceleration;
            _rotationSpeed = _data.rotationSpeed;
            _explosionArea = _data.explosionArea;
            _explosionForce = _data.explosionForce;
            _explosionDelay = _data.explosionDelay;
            _explosionParticleSystem = _data.explosionParticleSystem;
            _scaleModifier = _data.scaleModifier;
            _explosionDestroy = _data.explosionDestroy;
        }

        private void OnEnable()
        {
            _body.SetActive(true);
            StartCoroutine(BeforeStartingEngine());
        }

        private void OnDisable()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            engineParticleSystem.Stop();
            StopAllCoroutines();
        }

        public void SetTarget(Vector3 target, Transform planetTransform)
        {
            _target = target;
            _planetTransform = planetTransform;
        }
        
        private IEnumerator Explosion()
        {
            _body.SetActive(false);
            for (float i = 0; i < _explosionDelay; i += Time.deltaTime)
            {
                yield return null;
            }
            var hitsSphereCast = Physics.SphereCastAll(transform.position, _explosionArea, transform.forward,
                _explosionArea, GlobalData.LayerForExplosion);
            foreach (var hitSphereCast in hitsSphereCast)
            {
                if (hitSphereCast.rigidbody)
                {
                    if (hitSphereCast.rigidbody.isKinematic)
                    {
                        hitSphereCast.rigidbody.isKinematic = false;
                    }

                    var localScale = hitSphereCast.transform.localScale;
                    hitSphereCast.transform.localScale = new Vector3(localScale.x * _scaleModifier, localScale.y * _scaleModifier, localScale.z * _scaleModifier);
                    
                    Vector3 dirNorm = (hitSphereCast.transform.position - _planetTransform.position).normalized;
                    hitSphereCast.rigidbody.AddForce(dirNorm * _explosionForce, ForceMode.Impulse);
                }
                        
                if (hitSphereCast.transform.CompareTag("Chelik"))
                {
                    var chelikMoveScript = hitSphereCast.collider.GetComponent<ChelikMove>();
                    chelikMoveScript.DeactivateChelikMove();
                }
                else if (hitSphereCast.transform.CompareTag("Tree"))
                {
                    var treeScript = hitSphereCast.collider.GetComponent<ObjectOnPlanet>();
                    treeScript.AddBlastForce();
                }
            }
            OnFlyEnd?.Invoke(gameObject.GetComponent<MissileView>());
        }

        private IEnumerator BeforeStartingEngine()
        {
            for (float i = 0; i < _timeBeforeStartEngine; )
            {
                var deltaTime = Time.deltaTime;
                i += deltaTime;
                _rigidbody.velocity = _rigidbody.transform.forward * _initialImpulse * Time.deltaTime;
                yield return null;
            }
            StopCoroutine(BeforeStartingEngine());
            StartCoroutine(BeforeStopEngine());
        }

        private IEnumerator BeforeStopEngine()
        {
            engineParticleSystem.Play();
            for (float i = 0; i < _timeBeforeEngineStop; )
            {
                var deltaTime = Time.deltaTime;
                i += deltaTime;
                var direction = _target - transform.position;
                var rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * deltaTime);
                _rigidbody.velocity = _rigidbody.transform.forward * _engineAcceleration * deltaTime;
                yield return null;
            }
            engineParticleSystem.Stop();
            StopCoroutine(BeforeStopEngine());
        }

        private void OnCollisionEnter(Collision collision)
        {
            StopAllCoroutines();
            StartCoroutine(Explosion());
            var explosion = Instantiate(_explosionParticleSystem, transform.position, Quaternion.identity);
            Destroy(explosion, _explosionDestroy);
        }
    }
}
