using System;
using System.Linq;
using Battles.Players;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Battles.Attack {
    public class AttackJudge : MonoBehaviour {
       
        public void Set(IPlayerBinder first_player, IPlayerBinder second_player) {
            var first_root = first_player.PlayerRoot;
            var second_root = second_player.PlayerRoot;

            AttackDamageBox? first_damage = null;
            first_root.AttackControll.OnAttackHit.Subscribe(n=> {
                if (n.attackType == AttackType.Shot) {
                    second_root.DamageControll.Hit(n);
                } else {
                    first_damage = n;
                }
            });

            AttackDamageBox? second_damage = null;
            second_root.AttackControll.OnAttackHit.Subscribe(n=> {
                if (n.attackType == AttackType.Shot) {
                    first_root.DamageControll.Hit(n);
                } else {
                    second_damage = n;
                }
            });

            var check_normal_attack=new Func<AttackType,bool>(
                (n)=>n==AttackType.Weak||n==AttackType.Strong);
            
            this.LateUpdateAsObservable()
                .Where(n => !(first_damage == null && second_damage == null))
                .Subscribe(n => {
                    if (first_damage != null && second_damage != null) {
                        var ft = ((AttackDamageBox)first_damage).attackType;
                        var st = ((AttackDamageBox)second_damage).attackType;

                        if ((check_normal_attack(ft) && st == AttackType.Grab)) {
                            if (!first_root.AttackControll.AttackEnable) {
                                first_root.AttackControll.AttackEnd();
                            }
                            second_root.AttackControll.AttackEnd();
                        }else if (check_normal_attack(st) && ft == AttackType.Grab) {
                            if (!second_root.AttackControll.AttackEnable) {
                                second_root.AttackControll.AttackEnd();
                            }
                            first_root.AttackControll.AttackEnd();
                        }
                    } else {
                        if (first_damage != null) {
                            second_player.PlayerRoot.DamageControll.Hit((AttackDamageBox)first_damage);
                        }

                        if (second_damage != null) {
                            first_player.PlayerRoot.DamageControll.Hit((AttackDamageBox)second_damage);
                        }
                    }
                    first_damage = null;
                    second_damage = null;
                });
        }
    }
}