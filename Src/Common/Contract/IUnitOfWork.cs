using System;
using System.Collections.Generic;
using System.Text;

namespace QTrans.Contract
{
    public interface IUnitOfWork<TContext>
    {
       bool Commit();

        bool Rollback();
    }
}
