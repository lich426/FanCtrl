
namespace FanCtrl
{
    public class PluginDevice
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public PluginDevice(string key, string name)
        {
            Key = key;
            Name = name;
        }
    }
}
