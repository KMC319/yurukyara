using System.Collections.Generic;
using System.Linq;

namespace doma{
	namespace Systems{
		/// <summary>
		/// http://baba-s.hatenablog.com/entry/2015/07/26/100000
		/// </summary>
		public static class ListExtensions
		{
			/// <summary>
			/// 先頭にあるオブジェクトを削除せずに返します
			/// </summary>
			public static T Peek<T>( this IList<T> self )
			{
				return self[ 0 ];
			}

			/// <summary>
			/// 先頭にあるオブジェクトを削除し、返します
			/// </summary>
			public static T Pop<T>( this IList<T> self )
			{
				var result = self[ 0 ];
				self.RemoveAt( 0 );
				return result;
			}

			/// <summary>
			/// 末尾にオブジェクトを追加します
			/// </summary>
			public static void Push<T>( this IList<T> self, T item )
			{
				self.Insert( 0, item );
			}

			
			/// <summary>
			/// slide elements 
			/// </summary>
			/// <param name="vec"></param>
			public static void Slide<T>(this IList<T> self, int vec){
				if (vec >= 0){
					var temp = self[self.Count - 1];
					foreach (var i in Enumerable.Range(0, self.Count - 1).Reverse()){
						self[i + 1] = self[i];
					}
					self[0] = temp;
				} else{
					var temp = self[0];
					foreach (var i in Enumerable.Range(1, self.Count-1)){
						self[i -1] = self[i];
					}
					self[self.Count-1] = temp;

				}
			}
		}
	}
}