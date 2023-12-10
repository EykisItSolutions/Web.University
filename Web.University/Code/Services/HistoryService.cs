using System.Data;
using System.Text.Json;
using Web.University.Domain;

namespace Web.University;

#region Interface

public interface IHistoryService
{
    Task SaveAsync(int id, Type type, string recordName, string operation);
}
#endregion

public class HistoryService(UniversityContext db, ICurrentUser currentUser) : IHistoryService
{
    #region Handlers

    public async Task SaveAsync(int id, Type type, string recordName, string operation)
    {
        // Serialize and save a history record

        string sql = "SELECT * FROM [" + type.Name + "] WHERE Id = @0";
        var row = db.DataRow(sql, id);

        if (row != null)
        {
            var dictionary = new Dictionary<string, object?>();
            foreach (DataColumn col in row.Table.Columns)
                dictionary.Add(col.ColumnName, row.IsNull(col) ? null : row[col]);

            var json = JsonSerializer.Serialize(dictionary); 

            var history = new History()
            {
                UserId = currentUser.Id,
                WhatId = id,
                What = type.Name,
                Name = recordName,
                Operation = operation,
                HistoryDate = DateTime.Now,
                Content = json,
                Txn = db.Database.CurrentTransaction?.TransactionId
            };

            db.Histories.Add(history);

            await db.SaveChangesAsync();
        }
    }

    #endregion
}