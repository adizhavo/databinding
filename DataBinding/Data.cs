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
            return $"\nbranch: {branch}, value: {value}";
        }
    }

    public class DataTypeMap
    {
        public string branch;
        public string type;

        public override string ToString()
        {
            return $"\nbranch: {branch}, type: {type}";
        }
    }
}