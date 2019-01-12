namespace RV.Model.ViewModels
{
    public class Road : BaseViewModel
    {
        public int SourceId { get; set; }
        
        public int TargetId { get; set; }
        
        public bool IsView { get; set; }
    }
}