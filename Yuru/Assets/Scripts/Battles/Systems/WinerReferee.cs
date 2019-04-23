using System;
using System.Collections.Generic;
using System.Linq;

namespace Battles.Systems{
	public class WinerReferee{
		private class WinnerCount{
			public PlayerNum playerNum;
			public int count;

			public WinnerCount(PlayerNum player_num){
				playerNum = player_num;
				this.count = 0;
			}

			public void Reset(){
				count = 0;
			}
		}

		private const int WIN_BORDER = 2;

		private static List<WinnerCount> winnerCounters=new List<WinnerCount>(){
			new WinnerCount(PlayerNum.P1),new WinnerCount(PlayerNum.P2)
		};
		
		public WinerReferee(PlayerNum winner){
			if (winner == PlayerNum.None){
				foreach (var item in winnerCounters){
					item.count++;
				}	
			} else{
				winnerCounters.Find(n => n.playerNum == winner).count++;
			}
		}

		public PlayerNum GetRoundWinner(){
			var border_pver_player_count = winnerCounters.Count(n=>n.count>=WIN_BORDER);
			if (border_pver_player_count == 0){
				return PlayerNum.None;
			}

			var res=winnerCounters.Find(n => n.count >= WIN_BORDER).playerNum;
			
			foreach (var item in winnerCounters){
				item.Reset();
			}

			if (border_pver_player_count == 1){
				return res;
			}
			throw new Exception();
		}

		public static int GetCount(PlayerNum player_num){
			return winnerCounters.Find(n => n.playerNum == player_num).count;
		}

		public static void Reset() {
			foreach (var item in winnerCounters){
				item.Reset();
			}
		}
		
	}
}