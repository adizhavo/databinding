using System.Text;

namespace DataBinding
{
    public class Data<T>
    {
        public string branch;
        public T value;

        public override string ToString()
        {
            StringBuilder text = new StringBuilder($"\nbranch: {branch}");
            text.Append($"\nvalue: {value.ToString()}");
            return text.ToString();
        }
    }

    public class PseudoData
    {
        public string branch;
        public string json;

        public override string ToString()
        {
            return $"branch: {branch}, json: {json}";
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