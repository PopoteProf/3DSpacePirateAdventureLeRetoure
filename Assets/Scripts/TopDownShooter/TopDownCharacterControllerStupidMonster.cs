using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TopDownCharacterControllerStupidMonster : TopDownStupidMonsters
{
    private CharacterController _characterController;
    // protected override void Start()
    // {
    //     _characterController = GetComponent<CharacterController>();
    //     base.Start();
    // }
    // protected override void ManagedMovement()
    // {
    //     if (_isDead) return;
    //     if( Vector3.Distance(_player.transform.position, transform.position)<=_aggroDistance) {
    //
    //         _characterController.Move((_player.transform.position - transform.position).normalized * _moveSpeed *
    //                                   Time.deltaTime);
    //         
    //         if (_characterController.velocity.magnitude > 0.5f)
    //         {
    //             transform.forward = _characterController.velocity;
    //         }
    //         if (_animator)_animator.SetFloat("Speed", _characterController.velocity.magnitude);
    //
    //     }
    //     
    //
    //     if (Vector3.Distance(_player.transform.position, transform.position) <= _attackDistance && _canAttack) {
    //         if (_animator)_animator.SetTrigger("Attack");
    //         _attackTimer = 0;
    //     }
    //
    //     if (!_canAttack) {
    //         _attackTimer += Time.deltaTime;
    //     }
    // }

    // public override void SetDeath() {
    //     _characterController.enabled = false;
    //     base.SetDeath();
    // }
}