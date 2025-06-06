using Assets.Scripts.Item;
using UnityEngine;
using UnityEngine.Rendering;

public class AttackEffectState : StateMachineBehaviour
{
    [Header("이펙트 데이터")]
    [SerializeField] EffectSO _effectData;

    [Range(0, 1)]
    [SerializeField] float PlayTiming = 0.3f;

    private CharacterModel _model;
    private CharacterController _controller;

    public Vector3 _targetPos;

    private float _timer = 0f;

    bool _isPlaying = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_model == null)
        {
            _model = animator.transform.GetComponent<CharacterModel>();
        }
        if (_controller == null)
        {
            _controller = animator.transform.GetComponent<CharacterController>();
        }

        _isPlaying = false;

        _timer = 0f;
    }

    // 상태 동안 매 프레임 호출
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isPlaying)
            return;
        if (_model.ElementType == ElementTypeEnum.ElementType.None)
            return;

        Debug.Log($"속성 인트 : {(int)_model.ElementType}");
        Debug.Log("생성 1");
        _timer += Time.deltaTime;

        if (_timer >= PlayTiming)
        {
            /* Debug.Log("생성 2");
             GameObject instance = Instantiate(
                                         _effectData.AttackEffects[(int)_model.ElementType], 
                                         _controller._monster.transform.position, 
                                         Quaternion.identity);
             Destroy(instance, 1.3f);
             Debug.Log("생성 3");
             _isPlaying = true;*/
            int index = (int)Mathf.Log((int)_model.ElementType, 2);
            if (index >= 0 && index < _effectData.AttackEffects.Length)
            {
                GameObject instance = GameObject.Instantiate(
                    _effectData.AttackEffects[index],
                    _controller._monster.transform.position,
                    Quaternion.identity
                );
                GameObject.Destroy(instance, 1.3f);
                _isPlaying = true;
            }
        }
    }

    // 상태 종료 시 초기화
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer = 0f;
        _isPlaying = false;
    }
}
