namespace doma.Interfaces{
	public interface ISelectablePanel{
		void OnSelect();
		void RemoveSelect();
		void Submit();
		bool IsActive{ get; set; }
	}
}