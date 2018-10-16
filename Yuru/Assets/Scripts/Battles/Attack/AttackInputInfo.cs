using System;
using System.Collections.Generic;

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
	
	public enum PlayerKeyCode{
		A,B,X,Y,RArrow,LArrow,UArrow,DArrow
	}
	public enum CommandType{
		Normal,Jump,Chain
	}
	public enum ApplyPhase{
		Both,P2D,P3D
	}
}