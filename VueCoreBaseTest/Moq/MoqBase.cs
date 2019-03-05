using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VueCoreBaseTest.Moq
{
    class MoqBase
    {
    }

    public class FakeDbSet<T> : Mock<DbSet<T>> where T : class
    {

        public void SetData(IEnumerable<T> data)
        {
            var mockDataQueryable = data.AsQueryable();

            As<IQueryable<T>>().Setup(x => x.Provider).Returns(mockDataQueryable.Provider);
            As<IQueryable<T>>().Setup(x => x.Expression).Returns(mockDataQueryable.Expression);
            As<IQueryable<T>>().Setup(x => x.ElementType).Returns(mockDataQueryable.ElementType);
            As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(mockDataQueryable.GetEnumerator());

        }
    }
}
