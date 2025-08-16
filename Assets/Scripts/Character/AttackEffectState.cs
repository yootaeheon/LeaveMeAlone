using Assets.Scripts.Item;
using UnityEngine;

public class AttackEffectState : StateMachineBehaviour
{
    [Header("����Ʈ ������")]
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

    // ���� ���� �� ������ ȣ��
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isPlaying)
            return;
        if (_model.ElementType == ElementTypeEnum.ElementType.None)
            return;

        _timer += Time.deltaTime;

        if (_timer >= PlayTiming)
        {
            //elementTypeEnum�� ��Ʈ ����ũ �뵵�̱� ������ Log�� �̿��Ͽ� ���������� ��ȯ�������
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

    // ���� ���� �� �ʱ�ȭ
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer = 0f;
        _isPlaying = false;
    }
}
