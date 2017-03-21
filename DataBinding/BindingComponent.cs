namespace DataBinding
{
    public interface BindingComponent<T>
    {
        void OnValueChanged(string branch, T value);
    }
}