using AutoFixture;
using AutoFixture.NUnit3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementTests
{
    public class AutoDataNoRecursionAttribute : AutoDataAttribute
    {
        public AutoDataNoRecursionAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        })
        {
        }

    }
}
