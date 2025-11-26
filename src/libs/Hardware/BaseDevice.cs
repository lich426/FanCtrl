
namespace FanCtrl
{
    public class BaseDevice
    {
        public LIBRARY_TYPE Type { get; set; }

        public string Name { get; set; }

        public string ID { get; set; }

        public BaseDevice()
        {

        }

        public virtual void update()
        {

        }
    }
}
