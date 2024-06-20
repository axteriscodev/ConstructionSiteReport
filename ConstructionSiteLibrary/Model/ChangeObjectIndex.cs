

namespace ConstructionSiteLibrary.Model
{
    public class ChangeObjectIndex
    {
        public int OldIndex { get; set; }
        public int NewIndex { get; set; }
        public int GroupNumber { get; set; }

        public ChangeObjectIndex(){ }

        public ChangeObjectIndex(int oldIndex, int newIndex) : this(oldIndex, newIndex, 0) { }

        public ChangeObjectIndex(int oldIndex, int newIndex, int groupNumber)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            GroupNumber = groupNumber;
        }
    }
}
