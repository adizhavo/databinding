using System.Text;

namespace DataBinding
{
    public class Branch
    {
        public string branch;
    }

    public class Data<T> : Branch
    {
        public T value;

        public override string ToString()
        {
            StringBuilder text = new StringBuilder($"\nbranch: {branch}");
            text.Append($"\nvalue: {value.ToString()}");
            return text.ToString();
        }
    }

    public class DataTypeMap
    {
        public string branch;
        public string type;

        public override string ToString()
        {
            return $"branch: {branch}, type: {type}";
        }
    }
}