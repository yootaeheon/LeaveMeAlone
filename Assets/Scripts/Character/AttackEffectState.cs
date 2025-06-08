using Assets.Scripts.Item;
using UnityEngine;

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
            //elementTypeEnum이 비트 마스크 용도이기 때문에 Log를 이용하여 정수형으로 변환해줘야함
            int index = (int)Mathf.Log((int)_model.ElementType, 2);
            if (index >= 0 && index < _effectData.AttackEffects.Length)
            {
                if (_controller == null || _controller._monster == null)
                    return;

                GameObject instance = GameObject.Instantiate
                    (
                        _effectData.AttackEffects[index],
                        _controller._monster.transform.position,
                        Quaternion.identity

                    );
                GameObject.Destroy(instance, 1.3f);
                /*EffectManager.Instance.PlayEffect(_effectData.AttackEffects[index], _controller._monster.transform.position + new Vector3(0, 1f, 0));*/

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
