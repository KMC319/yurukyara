using System;
using System.Collections.Generic;
using Players;

namespace Battles.Attack{
	[Serializable]
	public struct AttackInputInfo{
		public List<PlayerKeyCode> keyCodes;
		public CommandType commandType;
		public ApplyPhase applyPhase;

		public AttackInputInfo(List<PlayerKeyCode> key_codes, CommandType command_type, ApplyPhase apply_phase){
			keyCodes = key_codes;
			commandType = command_type;
			applyPhase = apply_phase;
		}
	}
}