using System;
using System.Collections.Generic;
using System.Linq;
using doma.Inputs;
using doma.Systems;
using UniRx;
using UnityEngine;

namespace doma.Interfaces{
	public enum CursoleOver{
		Top,Under,Right,Left,
	}

	public enum ListCreateOption{
		Horizontal,Vertical
	}
	public class InterfaceEventSystem : MonoBehaviour,ISystemProcess,IUikeyReciever{
		[SerializeField] private Vector2 selectedPoint;
		[SerializeField] private bool IsActive;
		[SerializeField] private bool selectLoop;
		public Vector2 GetSelectedPoint => selectedPoint;
		
		private List<ISelectablePanel> iSelectablePanels=new List<ISelectablePanel>();
		private List<List<ISelectablePanel>> activeIselectables=new List<List<ISelectablePanel>>();
		

		private readonly Subject<Unit> bootNotification=new Subject<Unit>();
		public Subject<Unit> BootNotification => bootNotification;

		private readonly Subject<Unit> submitKey=new Subject<Unit>();
		public Subject<Unit> SubmitKey => submitKey;
		
		private readonly Subject<Unit> cancelKey=new Subject<Unit>();
		public Subject<Unit> CancelKey => cancelKey;

		private readonly Subject<ISelectablePanel> focusSelectable=new Subject<ISelectablePanel>();
		public UniRx.IObservable<ISelectablePanel> FocusSelectable => focusSelectable;
		
		private readonly Subject<ISelectablePanel> enterSelectable=new Subject<ISelectablePanel>();
		public UniRx.IObservable<ISelectablePanel> EnterSelectable => enterSelectable;
		
		private readonly Subject<CursoleOver> overSubject=new Subject<CursoleOver>();
		public UniRx.IObservable<CursoleOver> OverSubject => overSubject;

		
		public void Awake (){
			iSelectablePanels.AddRange(transform.GetComponentsInChildren<ISelectablePanel>().ToList());
		}
		
		
		public void Select(Vector2 point){
			var target = activeIselectables[(int) selectedPoint.y][(int) selectedPoint.x];
			target.OnSelect();
			
			focusSelectable.OnNext(target);
		}

		private void Enter(){
			if (!IsActive){
				return;
			}
			var target = activeIselectables[(int) selectedPoint.y][(int) selectedPoint.x];
			target.Submit();
			enterSelectable.OnNext(target);
			IsActive = false;
		}


		public void SelectMove(Vector2 point){
			if (!IsActive){
				return;
			}
			activeIselectables[(int) selectedPoint.y][(int) selectedPoint.x].RemoveSelect();
			selectedPoint+=point;
			
			if (selectedPoint.y < 0){
				if (selectLoop){
					selectedPoint.y = activeIselectables.Count - 1;
				} else{
					overSubject.OnNext(CursoleOver.Top);
					selectedPoint.y = 0;
				}
			}else if (selectedPoint.y > activeIselectables.Count - 1){
				if (selectLoop){
					selectedPoint.y = 0; 
				}else{
					overSubject.OnNext(CursoleOver.Under);
					selectedPoint.y = activeIselectables.Count-1;	
				}
			}

			if (selectedPoint.x < 0){
				if (selectLoop){
					selectedPoint.x = activeIselectables[(int) selectedPoint.y].Count - 1;
				} else{
					overSubject.OnNext(CursoleOver.Left);
					selectedPoint.x = 0;
				}
			}else if (selectedPoint.x > activeIselectables[(int) selectedPoint.y].Count - 1){
				if (selectLoop){ selectedPoint.x = 0; } else{
					overSubject.OnNext(CursoleOver.Right);
					selectedPoint.x = activeIselectables[(int) selectedPoint.y].Count - 1;
				}
			}

			Select(selectedPoint);
		}


		public void CreateActiveSelectableList<Type>(int[] structure)where Type : ISelectablePanel{
			var target_selectables = new List<ISelectablePanel>();
			foreach (var item in iSelectablePanels){
				if (item is Type){
					item.IsActive = true;
					target_selectables.Add(item);
				} else{ item.IsActive = false; }
			}
			activeIselectables.Clear();
			foreach (var i in Enumerable.Range(0,structure.Length)){
				var list_row = new List<ISelectablePanel>();
				var brekafg = false;
				activeIselectables.Add(list_row);
				IEnumerable<int> repeat;
				try{
					repeat = Enumerable.Range(0,structure[i]);
				} catch (Exception e){
					DebugLogger.LogError(e);
					break;
				}
				foreach (var j in repeat){
					list_row.Add(target_selectables.Pop());
					if (target_selectables.Count == 0){
						brekafg = true;
						break;
					}
				}
				if(brekafg){break;}
			}
		}

		public void CreateActiveSelectableList<Type>(ListCreateOption option) where Type : ISelectablePanel{
			switch (option){
				case ListCreateOption.Horizontal:
					CreateActiveSelectableList<Type>(new int[]{100});
					break;
				case ListCreateOption.Vertical:
					CreateActiveSelectableList<Type>(Enumerable.Repeat(1,100).ToArray());
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(option), option, null);
			}
		}


		public void Launch(float x=0,float y=0){
			foreach (var i in Enumerable.Range(0,activeIselectables.Count)){
				activeIselectables[i] = activeIselectables[i].Where(n => n.IsActive).ToList();
			}
			activeIselectables.RemoveAll(n => n.Count == 0);
			foreach (var item in activeIselectables){
				foreach (var jtem in item){
					jtem.RemoveSelect();
				}
			}
			selectedPoint= new Vector2(x,y);
			Select(selectedPoint);
			IsActive = true;
		}

		public void Freeze(){
			IsActive = false;
		}
		public void ReBoot(){
			IsActive = true;
		}

		public void RegistrationSelectable(ISelectablePanel selectable){
			if (iSelectablePanels.Contains(selectable)){
				DebugLogger.LogError(selectable+" is contain in mylist");
				return;
			}
			iSelectablePanels.Add(selectable);
		}
		
		public void RegistrationSelectable(IEnumerable<ISelectablePanel> selectables){
			foreach (var item in selectables){
				RegistrationSelectable(item);
			}
		}


		public void StartInputRecieve(){
			bootNotification.OnNext(Unit.Default);
		}

		public void EndInputRecieve(){}
		
		public void UpKey(){
			SelectMove(Vector2.down);
		}

		public void DownKey(){
			SelectMove(Vector2.up);
		}

		public void RightKey(){
			SelectMove(Vector2.right);
		}

		public void LeftKey(){
			SelectMove(Vector2.left);
		}

		public void EnterKey(){
			Enter();
			submitKey.OnNext(Unit.Default);
		}

		void IUikeyReciever.CancelKey(){
			cancelKey.OnNext(Unit.Default);
		}
	}
}
