﻿using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.CommunicationBL.Interfaces;

public interface IServiceTransaction<TModel, TIdType> : IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<int> DeleteUseExistTransactionAsync(IDbContextTransaction transaction, TIdType id);
}