using DesignTimeData.Code;
using DesignTimeData.ViewModel;

namespace DesignTimeData.Design
{
    public class DesignMainVM:MainVM
    {
        public DesignMainVM()
        {
            new DesignTimeDataLoader().Populate(this);
        }
    }
}