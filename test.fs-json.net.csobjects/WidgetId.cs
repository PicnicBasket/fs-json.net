using System;
using System.ComponentModel;

namespace test.fs_json.net.csobjects
{
    [Serializable]
    [TypeConverter(typeof(IdentityTypeConverter<WidgetId>))]
    public class WidgetId : BaseIdentity
    {
        public WidgetId(Guid id)
            : base(id)
        {
        }

        public WidgetId(String id)
            : base(id)
        {
        }

        public WidgetId()
        {
        }
    }
}