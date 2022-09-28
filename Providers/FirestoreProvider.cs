using CoombsBank.Interfaces;
using Google.Cloud.Firestore;

namespace CoombsBank.Providers;

public class FirestoreProvider
{
    private readonly FirestoreDb _fireStoreDb;

    public FirestoreProvider(FirestoreDb fireStoreDb)
    {
        _fireStoreDb = fireStoreDb;
    }

    public async Task AddOrUpdate<T>(object entity, CancellationToken ct, string docId) where T : IFirebaseEntity
    {
        var document = _fireStoreDb.Collection(typeof(T).Name).Document(docId);
        await document.SetAsync(entity, cancellationToken: ct);
    }

    public async Task<T> Get<T>(string docId, CancellationToken ct) where T : IFirebaseEntity
    {
        var document = _fireStoreDb.Collection(typeof(T).Name).Document(docId);
        var snapshot = await document.GetSnapshotAsync(ct);
        return snapshot.ConvertTo<T>();
    }

    public async Task<IReadOnlyCollection<T>> GetAll<T>(CancellationToken ct) where T : IFirebaseEntity
    {
        var collection = _fireStoreDb.Collection(typeof(T).Name);
        var snapshot = await collection.GetSnapshotAsync(ct);
        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    public async Task<IReadOnlyCollection<T>> WhereEqualTo<T>(string fieldPath, object value, CancellationToken ct) where T : IFirebaseEntity
    {
        return await GetList<T>(_fireStoreDb.Collection(typeof(T).Name).WhereEqualTo(fieldPath, value), ct);
    }
    
    private static async Task<IReadOnlyCollection<T>> GetList<T>(Query query, CancellationToken ct) where T : IFirebaseEntity
    {
        var snapshot = await query.GetSnapshotAsync(ct);
        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }
}
